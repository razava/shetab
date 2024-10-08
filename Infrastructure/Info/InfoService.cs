﻿using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Info.Common;
using Application.Info.Queries.GetInfo;
using ClosedXML.Excel;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using FluentResults;
using Infrastructure.Persistence;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Quartz.Util;
using SharedKernel.ExtensionMethods;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Info;

public class InfoService(
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IActorRepository actorRepository) : IInfoService
{
    //Status
    public async Task<InfoModel> GetReportsStatusPerCategory(GetInfoQueryParameters queryParameters)
    {
        int parentCategoryId;
        var result = new InfoModel();

        if (int.TryParse(queryParameters.Parameter, out int id))
        {
            parentCategoryId = id;
        }
        else
        {
            return result;
        }

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک دسته بندی", "", false, false);

        //var queryLimits = await createReportQuery(queryParameters);
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var groupedQuery = await query
            .GroupBy(q => new { q.CategoryId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
        .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var categories = await unitOfWork.DbContext.Set<Category>()
            .AsNoTracking()
            .Where(c => c.ShahrbinInstanceId == queryParameters.InstanceId)
            .ToListAsync();

        categories.Structure();

        Category? parentNode = null;
        if (parentCategoryId < 1)
            parentNode = categories.Where(c => c.ParentId == null).FirstOrDefault();
        else
            parentNode = categories.Where(c => c.Id == parentCategoryId).FirstOrDefault();

        if (parentNode is null)
            return result;

        var totalSerie = new InfoSerie("کل", "");
        var liveSerie = new InfoSerie("در جریان", "");
        var doneSerie = new InfoSerie("پایان یافته", "");
        var needAcceptanceSerie = new InfoSerie("در انتظار تأیید", "");
        var feedbackedSerie = new InfoSerie("بازخورد شهروند", "");
        var objectionedSerie = new InfoSerie("اعتراض شهروند", "");
        infoChart.Add(totalSerie);
        infoChart.Add(liveSerie);
        infoChart.Add(doneSerie);
        infoChart.Add(needAcceptanceSerie);
        infoChart.Add(feedbackedSerie);
        infoChart.Add(objectionedSerie);
        //var temp = new List<long>();

        foreach (var category in parentNode.Categories)
        {
            var decendantIds = category.Decendants;

            var done = groupedQuery.Where(g => decendantIds.Contains(g.Key.CategoryId) &&
            (g.Key.ReportState == ReportState.Finished || g.Key.ReportState == ReportState.AcceptedByCitizen))
                .Sum(g => g.Count);

            var live = groupedQuery.Where(g => decendantIds.Contains(g.Key.CategoryId) &&
            (g.Key.ReportState == ReportState.Live || g.Key.ReportState == ReportState.Review))
                .Sum(g => g.Count);

            var needAcceptance = groupedQuery.Where(g => decendantIds.Contains(g.Key.CategoryId) &&
            g.Key.ReportState == ReportState.NeedAcceptance)
                .Sum(g => g.Count);

            var total = done + live + needAcceptance;

            if (total == 0)
                continue;

            var feedbacked = groupedQuery.Where(g => decendantIds.Contains(g.Key.CategoryId) &&
            g.Key.IsFeedbacked == true)
                .Sum(g => g.Count);

            var objectioned = groupedQuery.Where(g => decendantIds.Contains(g.Key.CategoryId) &&
            g.Key.IsObjectioned == true)
                .Sum(g => g.Count);

            totalSerie.Add(new DataItem(
                category.Title,
                total.ToString(),
                GetPercent(total, total),
                category.Categories.Any() ? category.Id.ToString() : null));

            liveSerie.Add(new DataItem(
                category.Title,
                live.ToString(),
                GetPercent(live, total),
                category.Categories.Any() ? category.Id.ToString() : null));

            doneSerie.Add(new DataItem(
                category.Title,
                done.ToString(),
                GetPercent(done, total),
                category.Categories.Any() ? category.Id.ToString() : null));

            needAcceptanceSerie.Add(new DataItem(
                category.Title,
                needAcceptance.ToString(),
                GetPercent(needAcceptance, total),
                category.Categories.Any() ? category.Id.ToString() : null));

            feedbackedSerie.Add(new DataItem(
                category.Title,
                feedbacked.ToString(),
                GetPercent(feedbacked, total),
                category.Categories.Any() ? category.Id.ToString() : null));

            objectionedSerie.Add(new DataItem(
                category.Title,
                objectioned.ToString(),
                GetPercent(objectioned, total),
                category.Categories.Any() ? category.Id.ToString() : null));
        }

        result.Add(infoChart.Sort(0));

        return result;
    }

    public async Task<InfoModel> GetReportsStatusPerExecutive(GetInfoQueryParameters queryParameters)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک واحد اجرایی", "", false, false);

        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();
        query = await addRestrictions(query, queryParameters);

        var groupedQuery = await query
            .GroupBy(q => new { q.ExecutiveId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
            .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var executiveIds = (await userRepository.GetUsersInRole(RoleNames.Executive))
            .Where(u => u.ShahrbinInstanceId == queryParameters.InstanceId)
            .Select(u => u.Id).ToList();
        var executives = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => executiveIds.Contains(u.Id))
            .Select(u => new { u.Id, u.Title })
            .ToListAsync();

        var executiveActorIds = await unitOfWork.DbContext.Set<Actor>()
            .Where(a => executiveIds.Contains(a.Identifier))
            .ToListAsync();
        var waitedQuery = await query
            .Where(q => q.CurrentActor != null && executiveIds.Contains(q.CurrentActor.Identifier))
            .GroupBy(q => q.CurrentActor!.Identifier)
            .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var totalSerie = new InfoSerie("کل", "");
        var doneSerie = new InfoSerie("پایان یافته", "");
        var waitedSerie = new InfoSerie("در انتظار", "");
        var liveSerie = new InfoSerie("در جریان", "");
        var feedbackedSerie = new InfoSerie("بازخورد شهروند", "");
        var objectionedSerie = new InfoSerie("اعتراض شهروند", "");
        infoChart.Add(totalSerie);
        infoChart.Add(doneSerie);
        infoChart.Add(waitedSerie);
        infoChart.Add(liveSerie);
        infoChart.Add(feedbackedSerie);
        infoChart.Add(objectionedSerie);
        //var temp = new List<long>();


        foreach (var executive in executives)
        {
            var done = groupedQuery.Where(g => g.Key.ExecutiveId == executive.Id &&
            (g.Key.ReportState == ReportState.Finished || g.Key.ReportState == ReportState.AcceptedByCitizen))
                .Sum(g => g.Count);

            var live = groupedQuery.Where(g => g.Key.ExecutiveId == executive.Id &&
            (g.Key.ReportState == ReportState.Live || g.Key.ReportState == ReportState.Review))
                .Sum(g => g.Count);

            var waited = waitedQuery.Where(g => g.Key == executive.Id)
                .Sum(g => g.Count);

            var total = done + live + waited;

            if (total == 0)
                continue;

            var feedbacked = groupedQuery.Where(g => g.Key.ExecutiveId == executive.Id &&
            g.Key.IsFeedbacked == true)
                .Sum(g => g.Count);

            var objectioned = groupedQuery.Where(g => g.Key.ExecutiveId == executive.Id &&
            g.Key.IsObjectioned == true)
                .Sum(g => g.Count);

            totalSerie.Add(new DataItem(
                executive.Title,
                total.ToString(),
                GetPercent(total, total)));

            liveSerie.Add(new DataItem(
                executive.Title,
                live.ToString(),
                GetPercent(live, total)));

            doneSerie.Add(new DataItem(
                executive.Title,
                done.ToString(),
                GetPercent(done, total)));

            waitedSerie.Add(new DataItem(
                executive.Title,
                waited.ToString(),
                GetPercent(waited, total)));

            feedbackedSerie.Add(new DataItem(
                executive.Title,
                feedbacked.ToString(),
                GetPercent(feedbacked, total)));

            objectionedSerie.Add(new DataItem(
                executive.Title,
                objectioned.ToString(),
                GetPercent(objectioned, total)));
        }

        result.Add(infoChart.Sort(0));

        return result;
    }

    public async Task<InfoModel> GetReportsStatusPerContractor(GetInfoQueryParameters queryParameters)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک پیمانکار", "", false, false);

        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var groupedQuery = await query
            .GroupBy(q => new { q.ContractorId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
            .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var contractorIds = (await userRepository.GetUsersInRole(RoleNames.Contractor))
            .Select(u => u.Id).ToList();
        var contractors = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => contractorIds.Contains(u.Id))
            .Select(u => new { u.Id, u.Title })
            .ToListAsync();

        var contractorActorIds = await unitOfWork.DbContext.Set<Actor>()
            .Where(a => contractorIds.Contains(a.Identifier))
            .ToListAsync();
        var waitedQuery = await query
            .Where(q => q.CurrentActor != null && contractorIds.Contains(q.CurrentActor.Identifier))
            .GroupBy(q => q.CurrentActor!.Identifier)
            .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var totalSerie = new InfoSerie("کل", "");
        var doneSerie = new InfoSerie("پایان یافته", "");
        var waitedSerie = new InfoSerie("در انتظار", "");
        var liveSerie = new InfoSerie("در جریان", "");
        var feedbackedSerie = new InfoSerie("بازخورد شهروند", "");
        var objectionedSerie = new InfoSerie("اعتراض شهروند", "");
        infoChart.Add(totalSerie);
        infoChart.Add(doneSerie);
        infoChart.Add(waitedSerie);
        infoChart.Add(liveSerie);
        infoChart.Add(feedbackedSerie);
        infoChart.Add(objectionedSerie);
        //var temp = new List<long>();


        foreach (var contractor in contractors)
        {
            var done = groupedQuery.Where(g => g.Key.ContractorId == contractor.Id &&
            (g.Key.ReportState == ReportState.Finished || g.Key.ReportState == ReportState.AcceptedByCitizen))
                .Sum(g => g.Count);

            var live = groupedQuery.Where(g => g.Key.ContractorId == contractor.Id &&
            (g.Key.ReportState == ReportState.Live || g.Key.ReportState == ReportState.Review))
                .Sum(g => g.Count);

            var waited = waitedQuery.Where(g => g.Key == contractor.Id)
                .Sum(g => g.Count);

            var total = done + live + waited;

            if (total == 0)
                continue;

            var feedbacked = groupedQuery.Where(g => g.Key.ContractorId == contractor.Id &&
            g.Key.IsFeedbacked == true)
                .Sum(g => g.Count);

            var objectioned = groupedQuery.Where(g => g.Key.ContractorId == contractor.Id &&
            g.Key.IsObjectioned == true)
                .Sum(g => g.Count);

            totalSerie.Add(new DataItem(
                contractor.Title,
                total.ToString(),
                GetPercent(total, total)));

            liveSerie.Add(new DataItem(
                contractor.Title,
                live.ToString(),
                GetPercent(live, total)));

            doneSerie.Add(new DataItem(
                contractor.Title,
                done.ToString(),
                GetPercent(done, total)));

            waitedSerie.Add(new DataItem(
                contractor.Title,
                waited.ToString(),
                GetPercent(waited, total)));

            feedbackedSerie.Add(new DataItem(
                contractor.Title,
                feedbacked.ToString(),
                GetPercent(feedbacked, total)));

            objectionedSerie.Add(new DataItem(
                contractor.Title,
                objectioned.ToString(),
                GetPercent(objectioned, total)));
        }

        result.Add(infoChart.Sort(0));

        return result;
    }

    public async Task<InfoModel> GetReportsStatusPerRegion(GetInfoQueryParameters queryParameters)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک منطقه", "", false, false);

        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var groupedQuery = await query
            .GroupBy(q => new { q.Address.RegionId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
            .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var cityId = await unitOfWork.DbContext.Set<ShahrbinInstance>()
            .AsNoTracking().Where(s => s.Id == queryParameters.InstanceId)
            .Select(s => s.CityId).SingleOrDefaultAsync();

        var regions = await unitOfWork.DbContext.Set<Region>()
            .AsNoTracking()
            .Where(r => r.CityId == cityId)
            .Select(e => new { e.Id, Title = e.Name })
            .ToListAsync();

        var totalSerie = new InfoSerie("کل", "");
        var liveSerie = new InfoSerie("در حال رسیدگی", "");
        var doneSerie = new InfoSerie("رسیدگی شده", "");
        var needAcceptanceSerie = new InfoSerie("در انتظار تأیید", "");
        var feedbackedSerie = new InfoSerie("بازخورد شهروند", "");
        var objectionedSerie = new InfoSerie("اعتراض شهروند", "");
        infoChart.Add(totalSerie);
        infoChart.Add(liveSerie);
        infoChart.Add(doneSerie);
        infoChart.Add(needAcceptanceSerie);
        infoChart.Add(feedbackedSerie);
        infoChart.Add(objectionedSerie);
        //var temp = new List<long>();


        foreach (var region in regions)
        {
            var done = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            (g.Key.ReportState == ReportState.Finished || g.Key.ReportState == ReportState.AcceptedByCitizen))
                .Sum(g => g.Count);

            var live = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            (g.Key.ReportState == ReportState.Live || g.Key.ReportState == ReportState.Review))
                .Sum(g => g.Count);

            var needAcceptance = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            g.Key.ReportState == ReportState.NeedAcceptance)
                .Sum(g => g.Count);

            var total = done + live + needAcceptance;

            if (total == 0)
                continue;

            var feedbacked = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            g.Key.IsFeedbacked == true)
                .Sum(g => g.Count);

            var objectioned = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            g.Key.IsObjectioned == true)
                .Sum(g => g.Count);

            totalSerie.Add(new DataItem(
                region.Title,
                total.ToString(),
                GetPercent(total, total)));

            liveSerie.Add(new DataItem(
                region.Title,
                live.ToString(),
                GetPercent(live, total)));

            doneSerie.Add(new DataItem(
                region.Title,
                done.ToString(),
                GetPercent(done, total)));

            needAcceptanceSerie.Add(new DataItem(
                region.Title,
                needAcceptance.ToString(),
                GetPercent(needAcceptance, total)));

            feedbackedSerie.Add(new DataItem(
                region.Title,
                feedbacked.ToString(),
                GetPercent(feedbacked, total)));

            objectionedSerie.Add(new DataItem(
                region.Title,
                objectioned.ToString(),
                GetPercent(objectioned, total)));
        }

        result.Add(infoChart.Sort(0));

        return result;
    }


    //Statistics
    public async Task<InfoModel> GetUsersStatistics(GetInfoQueryParameters queryParameters)
    {
        var result = new InfoModel();
        var userContext = unitOfWork.DbContext.Set<ApplicationUser>().AsNoTracking();

        var totalPersonel = await userContext.Where(e =>
        e.ShahrbinInstanceId != null && e.ShahrbinInstanceId == queryParameters.InstanceId).LongCountAsync();

        var totalUsers = await userContext.Where(u => u.ShahrbinInstanceId == null).LongCountAsync();

        //var now = DateTime.UtcNow.Date;
        var now = DateTime.UtcNow.Date;
        //???????????????????????????????? citizen or staff :  ???
        var lastDay = await userContext.Where(p => p.VerificationSent >= now.Subtract(new TimeSpan(1, 0, 0, 0)))
            .LongCountAsync();

        var lastWeek = await userContext.Where(p => p.VerificationSent >= now.Subtract(new TimeSpan(7, 0, 0, 0)))
            .LongCountAsync();

        var lastMonth = await userContext.Where(p => p.VerificationSent >= now.Subtract(new TimeSpan(30, 0, 0, 0)))
            .LongCountAsync();

        result.Add(new InfoSingleton(
            totalPersonel.ToString(),
            "کل پرسنل",
            null));

        result.Add(new InfoSingleton(
            totalUsers.ToString(),
            "کل شهروندان",
            null));

        result.Add(new InfoSingleton(
            lastDay.ToString(),
            "روز گذشته",
            null));

        result.Add(new InfoSingleton(
            lastWeek.ToString(),
            "هفته گذشته",
            null));

        result.Add(new InfoSingleton(
            lastMonth.ToString(),
            "ماه گذشته",
            null));

        return result;

    }

    public async Task<InfoModel> GetReportsStatistics(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var totalReports = await query.LongCountAsync();

        var now = DateTime.UtcNow;
        var lastDay = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(1, 0, 0, 0))).LongCountAsync();
        var lastWeek = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(7, 0, 0, 0))).LongCountAsync();
        var lastMonth = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(30, 0, 0, 0))).LongCountAsync();

        var result = new InfoModel();
        result.Add(new InfoSingleton(
            totalReports.ToString(),
            "کل درخواست ها",
            null));

        result.Add(new InfoSingleton(
            lastDay.ToString(),
            "روز گذشته",
            null));

        result.Add(new InfoSingleton(
            lastWeek.ToString(),
            "هفته گذشته",
            null));

        result.Add(new InfoSingleton(
            lastMonth.ToString(),
            "ماه گذشته",
            null));

        return result;
    }

    public async Task<InfoModel> GetTimeStatistics(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        //average durations
        var allDuration = await query
                .Where(p => p.Duration != null)
                .AverageAsync(p => p.Duration);

        var now = DateTime.UtcNow;

        var lastDayDuration = await query
               .Where(p => p.Duration != null && p.Sent >= now.Subtract(new TimeSpan(1, 0, 0, 0)))
               .AverageAsync(p => p.Duration);

        var lastWeekDuration = await query
            .Where(p => p.Duration != null && p.Sent >= now.Subtract(new TimeSpan(7, 0, 0, 0)))
            .AverageAsync(p => p.Duration);

        var lastMonthDuration = await query
            .Where(p => p.Duration != null && p.Sent >= now.Subtract(new TimeSpan(30, 0, 0, 0)))
            .AverageAsync(p => p.Duration);

        allDuration ??= 0;
        lastDayDuration ??= 0;
        lastWeekDuration ??= 0;
        lastMonthDuration ??= 0;

        var result = new InfoModel();
        result.Add(new InfoSingleton(
            new TimeSpan(0, 0, (int)allDuration).ToString(),
            "متوسط زمان انجام کل",
            null));

        result.Add(new InfoSingleton(
            new TimeSpan(0, 0, (int)lastDayDuration).ToString(),
            "متوسط زمان انجام روز گذشته",
            null));

        result.Add(new InfoSingleton(
            new TimeSpan(0, 0, (int)lastWeekDuration).ToString(),
            "متوسط زمان انجام هفته گذشته",
            null));

        result.Add(new InfoSingleton(
            new TimeSpan(0, 0, (int)lastMonthDuration).ToString(),
            "متوسط زمان انجام ماه گذشته",
            null));


        //average response duration

        var allResponseDuration = await query
                .Where(p => p.ResponseDuration != null)
                .AverageAsync(p => p.ResponseDuration);

        var lastDayResponseDuration = await query
               .Where(p => p.ResponseDuration != null && p.Sent >= now.Subtract(new TimeSpan(1, 0, 0, 0)))
               .AverageAsync(p => p.ResponseDuration);

        var lastWeekResponseDuration = await query
            .Where(p => p.ResponseDuration != null && p.Sent >= now.Subtract(new TimeSpan(7, 0, 0, 0)))
            .AverageAsync(p => p.ResponseDuration);

        var lastMonthResponseDuration = await query
            .Where(p => p.ResponseDuration != null && p.Sent >= now.Subtract(new TimeSpan(30, 0, 0, 0)))
            .AverageAsync(p => p.ResponseDuration);

        allResponseDuration ??= 0;
        lastDayResponseDuration ??= 0;
        lastWeekResponseDuration ??= 0;
        lastMonthResponseDuration ??= 0;

        result.Add(new InfoSingleton(
            new TimeSpan(0, 0, (int)allResponseDuration).ToString(),
            "متوسط زمان پاسخ کل",
            null));

        result.Add(new InfoSingleton(
            new TimeSpan(0, 0, (int)lastDayResponseDuration).ToString(),
            "متوسط زمان پاسخ روز گذشته",
            null));

        result.Add(new InfoSingleton(
            new TimeSpan(0, 0, (int)lastWeekResponseDuration).ToString(),
            "متوسط زمان پاسخ هفته گذشته",
            null));

        result.Add(new InfoSingleton(
            new TimeSpan(0, 0, (int)lastMonthResponseDuration).ToString(),
            "متوسط زمان پاسخ ماه گذشته",
            null));

        return result;
    }

    public async Task<InfoModel> GetSatisfactionStatistics(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Satisfaction>()
           .AsNoTracking();

        if (queryParameters.ReportFilters.Query is not null)
        {
            var phrase = queryParameters.ReportFilters.Query.Trim();
            if (phrase.Length >= 3)
            {
                query = query.Where(r =>
                        r.Report.TrackingNumber.Contains(phrase) ||
                        r.Report.Citizen.FirstName.Contains(phrase) ||
                        r.Report.Citizen.LastName.Contains(phrase) ||
                        r.Report.Citizen.PhoneNumber != null && r.Report.Citizen.PhoneNumber.Contains(phrase));
            }
        }

        List<string> userIds = new List<string> { queryParameters.UserId };
        bool returnAll = false;
        if (queryParameters.Roles.Contains(RoleNames.Manager))
        {
            userIds.AddRange(await GetUserIdsOfOrganizationalUnit(queryParameters.UserId));
            query = query.Where(r => r.Report.Address.RegionId != null);
        }
        else
        {
            var regionIds = unitOfWork.DbContext.Set<Actor>()
                .Where(a => a.Identifier == queryParameters.UserId)
                .SelectMany(a => a.Regions.Select(r => r.Id));
            query = query.Where(r => r.Report.Address.RegionId != null && regionIds.Contains(r.Report.Address.RegionId.Value));

            if (queryParameters.Roles.Contains(RoleNames.Operator))
            {
                returnAll = true;
                var categoryIds = unitOfWork.DbContext.Set<Category>()
                    .Where(c => c.Users.Select(u => u.Id).Contains(queryParameters.UserId))
                    .Select(c => c.Id);
                query = query.Where(r => categoryIds.Contains(r.Report.CategoryId));
            }
            else if (queryParameters.Roles.Contains(RoleNames.Mayor))
            {
                returnAll = true;
            }
            else if (queryParameters.Roles.Contains(RoleNames.Inspector))
            {
                returnAll = true;
            }
        }

        var reportIds = unitOfWork.DbContext.Set<TransitionLog>()
            .Where(tl => userIds.Contains(tl.ActorIdentifier))
            .Select(tl => tl.ReportId)
            .Distinct();
        var roleIds = unitOfWork.DbContext.Set<ApplicationRole>()
            .Where(r => queryParameters.Roles.Contains(r.Name!))
            .Select(r => r.Id);
        var actorIds = unitOfWork.DbContext.Set<Actor>()
            .Where(a => userIds.Contains(a.Identifier) || roleIds.Contains(a.Identifier))
            .Select(a => a.Id)
            .Distinct();

        if (queryParameters.ReportsToInclude is null || queryParameters.ReportsToInclude.Count() == 0)
        {
            query = query.Where(r => reportIds.Contains(r.Id) || returnAll ||
                                     r.Report.CurrentActorId != null && actorIds.Contains(r.Report.CurrentActorId.Value));
        }
        else
        {
            var reportsToInclude = queryParameters.ReportsToInclude;
            if (reportsToInclude.Contains(ReportsToInclude.Interacted) && reportsToInclude.Contains(ReportsToInclude.InCartable))
            {
                query = query.Where(r => reportIds.Contains(r.Id) || returnAll ||
                                    r.Report.CurrentActorId != null && actorIds.Contains(r.Report.CurrentActorId.Value));
            }
            else if (reportsToInclude.Contains(ReportsToInclude.Interacted))
            {
                query = query.Where(r => reportIds.Contains(r.Id));
            }
            else if (reportsToInclude.Contains(ReportsToInclude.InCartable))
            {
                query = query.Where(r => returnAll || r.Report.CurrentActorId != null && actorIds.Contains(r.Report.CurrentActorId.Value));
            }

        }


        if (queryParameters.ReportFilters.FromDate != null)
        {
            query = query.Where(s => s.DateTime >= queryParameters.ReportFilters.FromDate);
        }
        if (queryParameters.ReportFilters.ToDate != null)
        {
            query = query.Where(s => s.DateTime < queryParameters.ReportFilters.ToDate.Value.AddDays(1));
        }

        if (queryParameters.ReportFilters.Categories != null)
        {
            query = query.Where(r => queryParameters.ReportFilters.Categories.Contains(r.Report.CategoryId));
        }

        if (queryParameters.ReportFilters.Regions != null)
        {
            query = query.Where(r => r.Report.Address.RegionId != null &&
            queryParameters.ReportFilters.Regions.Contains(r.Report.Address.RegionId.Value));
        }

        if (queryParameters.ReportFilters.SatisfactionValues != null)
        {
            query = query.Where(r => queryParameters.ReportFilters.SatisfactionValues.Contains(0) && r.Report.Satisfaction == null ||
                                     r.Report.Satisfaction != null && queryParameters.ReportFilters.SatisfactionValues.Contains(r.Rating));
        }

        if (queryParameters.Geometry is not null && queryParameters.Geometry.Count > 0)
        {
            var geometryFactory = new GeometryFactory();
            var coordinates = queryParameters.Geometry.Select(g => new Coordinate(g.Longitude, g.Latitude)).ToList();
            var geometry = geometryFactory.CreatePolygon(coordinates.ToArray());
            if (!geometry.Shell.IsCCW)
            {
                coordinates.Reverse();
                geometry = geometryFactory.CreatePolygon(coordinates.ToArray());
            }
            geometry.SRID = 4326;
            query = query.Where(r => geometry.Contains(r.Report.Address.Location));
        }

        var groupedQuery = await query
            .Where(s => s.Report.ShahrbinInstanceId == queryParameters.InstanceId)
            .GroupBy(s => s.Rating)
            .Select(s => new { s.Key, Count = s.Count() })
            .ToListAsync();

        var total = groupedQuery.Sum(s => s.Count);
        var averageRating = (double)groupedQuery.Sum(s => s.Key * s.Count) / total;

        var result = new InfoModel();
        result.Add(new InfoSingleton(total.ToString(), "تعداد کل", ""));
        result.Add(new InfoSingleton(averageRating.ToString("0.00"), "متوسط امتیاز", ""));

        var ratingChart = new InfoChart("فراوانی امتیازهای خشنودی سنجی", "", false, false);
        var ratingSerie = new InfoSerie("تعداد", "");
        ratingChart.Add(ratingSerie);
        for (var i = 1; i <= 5; i++)
        {
            var count = groupedQuery.Where(r => r.Key == i).Select(r => r.Count).SingleOrDefault();
            ratingSerie.Add(new DataItem(i.ToString(), count.ToString(), GetPercent(count, total)));
        }

        result.Add(ratingChart);

        var objectionedCount = await unitOfWork.DbContext.Set<Report>()
            .Where(r => r.ShahrbinInstanceId == queryParameters.InstanceId && r.IsObjectioned)
            .LongCountAsync();
        result.Add(new InfoSingleton(objectionedCount.ToString(), "ارجاع به بازرسی", ""));

        return result;
    }

    public async Task<InfoModel> GetActiveCitizens(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var groupedQuery = await query
            .GroupBy(r => r.CitizenId)
            .Select(g => new { g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .Take(20)
            .ToListAsync();

        var citizenIds = groupedQuery.Select(g => g.Key).ToList();

        var citizens = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => citizenIds.Contains(u.Id))
            .Select(u => new { u.Id, u.PhoneNumber, u.FirstName, u.LastName })
            .ToListAsync();

        var result = new InfoModel();
        var chart = new InfoChart("شهروندترین ها", "", false, false);
        result.Add(chart);
        var serie = new InfoSerie("تعداد درخواست", "");
        chart.Add(serie);

        foreach (var citizen in citizens)
        {
            var count = groupedQuery.Where(g => g.Key == citizen.Id).First().Count;
            var title = citizen.FirstName + " " + citizen.LastName;
            if (title.IsNullOrWhiteSpace())
                title = citizen.PhoneNumber;
            else
                title = title + $"({citizen.PhoneNumber})";

            serie.Add(new DataItem(title!, count.ToString(), count.ToString()));
        }

        return result;
    }

    //Temporal
    public async Task<InfoModel> GetReportsTimePerCategory(GetInfoQueryParameters queryParameters)
    {
        int parentCategoryId;
        var result = new InfoModel();

        if (int.TryParse(queryParameters.Parameter, out int id))
        {
            parentCategoryId = id;
        }
        else
        {
            return result;
        }

        var infoChart = new InfoChart("زمان رسیدگی به تفکیک دسته بندی", "", false, false);

        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var groupedQuery = await query
            .Where(r => r.Duration != null)
            .GroupBy(r => r.CategoryId)
            .Select(p => new
            {
                Id = p.Key,
                Duration = p.Average(r => r.Duration),
                ResponseDuration = p.Average(r => r.ResponseDuration)
            })
            .ToListAsync();


        var categories = await unitOfWork.DbContext.Set<Category>()
            .AsNoTracking()
            .Where(c => c.ShahrbinInstanceId == queryParameters.InstanceId)
            .ToListAsync();

        categories.Structure();

        Category? parentNode = null;
        if (parentCategoryId < 1)
            parentNode = categories.Where(c => c.ParentId == null).FirstOrDefault();
        else
            parentNode = categories.Where(c => c.Id == parentCategoryId).FirstOrDefault();

        if (parentNode is null)
            return result;

        var doneSerie = new InfoSerie("متوسط زمان انجام", "");
        var responseSerie = new InfoSerie("متوسط زمان پاسخ", "");
        infoChart.Add(doneSerie);
        infoChart.Add(responseSerie);
        foreach (var category in parentNode.Categories)
        {
            var decendantIds = category.Decendants;

            var duration = groupedQuery
                .Where(g => decendantIds.Contains(g.Id))
                .Select(g => g.Duration)
                .Average();

            var responseDuration = groupedQuery
                .Where(g => decendantIds.Contains(g.Id))
                .Select(g => g.ResponseDuration)
                .Average();

            if (duration is null && responseDuration is null)
                continue;

            duration ??= 0;
            var durationTimeSpan = new TimeSpan(0, 0, (int)duration);

            responseDuration ??= 0;
            var responseDurationTimeSpan = new TimeSpan(0, 0, (int)responseDuration);

            doneSerie.Add(new DataItem(
                category.Title,
                durationTimeSpan.ToHoursValue(),
                durationTimeSpan.ToPersianString(),
                category.Categories.Any() ? category.Id.ToString() : null));

            responseSerie.Add(new DataItem(
                category.Title,
                responseDurationTimeSpan.ToHoursValue(),
                responseDurationTimeSpan.ToPersianString(),
                category.Categories.Any() ? category.Id.ToString() : null));
        }

        result.Add(infoChart.Sort(0));
        return result;
    }

    public async Task<InfoModel> GetReportsTimeByRegion(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        query = query.Include(r => r.Address);

        var cityId = await unitOfWork.DbContext.Set<ShahrbinInstance>()
            .AsNoTracking().Where(s => s.Id == queryParameters.InstanceId)
            .Select(s => s.CityId).SingleOrDefaultAsync();

        var bins = await unitOfWork.DbContext.Set<Region>()
            .AsNoTracking()
            .Where(r => r.CityId == cityId)
            .Select(e => new Bin<int>(e.Id, e.Name))
            .ToListAsync();

        //Expression<Func<Report, int>> filter = z => (int)z.Address.RegionId ;

        var result = await GetReportsTimeHistogram<Report, int>(
            "متوسط زمان رسیدگی به تفکیک منطقه",
            bins,
            query,
            z => z.Address.RegionId!.Value);

        return result;
    }

    public async Task<InfoModel> GetRepportsTimeByExecutive(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var executives = (await userRepository.GetUsersInRole(RoleNames.Executive))
            .Where(u => u.ShahrbinInstanceId == queryParameters.InstanceId)
            .ToList();

        var bins = executives.Select(e => new Bin<string>(e.Id, e.Title)).ToList();

        var result = await GetReportsTimeHistogram<Report, string>(
            "متوسط زمان رسیدگی به تفکیک واحد اجرایی",
            bins,
            query,
            z => z.ExecutiveId!);

        if (result == null)
            result = new InfoModel();

        return result;
    }


    //Histograms
    public async Task<InfoModel> GetRequestsPerOperator(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);
        var operatorReports = await query
    .Where(r => r.ShahrbinInstanceId == queryParameters.InstanceId)
    .Join(
        unitOfWork.DbContext.Set<ApplicationUser>(),
        report => report.RegistrantId,  // فرض شده که Report یک فیلد OperatorId دارد
        user => user.Id,
        (report, user) => new { report.RegistrantId, user.Title , user.FirstName, user.LastName, report.Id }
    )
    .GroupBy(x => x.RegistrantId)
    .Select(g => new
    {
        Title = g.FirstOrDefault().Title + " - " + g.FirstOrDefault().FirstName + "  " + g.FirstOrDefault().LastName,  // Title از اولین رکورد در گروه گرفته می‌شود
        Count = g.Count()
    })
    .ToListAsync();

        var total = operatorReports.Sum(r => r.Count);

        var result = new InfoModel();
        var infoChart = new InfoChart("تعداد درخواست ثبت شده توسط هر اپراتور", "", false, false);
        var reportCountSerie = new InfoSerie("تعداد", "");
        infoChart.Add(reportCountSerie);

        foreach (var item in operatorReports)
        {
            reportCountSerie.Add(new DataItem(
                item.Title ?? "بدون عنوان",  // اگر Title خالی بود، "بدون عنوان" نمایش داده می‌شود
                item.Count.ToString(),
                GetPercent(item.Count, total)
            ));
        }

        result.Add(infoChart.Sort(0));
        return result;
    }

    public async Task<InfoModel> GetRequestsPerRegistrantType(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var operatorIds = (await userRepository.GetUsersInRole(RoleNames.Operator))
            .Where(u => u.ShahrbinInstanceId == queryParameters.InstanceId)
            .Select(u => u.Id)
            .ToList();

        var hist = await query
            .GroupBy(r => r.RegistrantId)
            .Select(r => new { r.Key, Count = r.Count() })
            .ToListAsync();
        var total = hist.Sum(h => h.Count);
        var citizenCount = hist.Where(h => h.Key == null).Sum(h => h.Count);
        var result = new InfoModel();

        var infoChart = new InfoChart("تعداد درخواست های ثبت شده توسط هر نوع ثبت کننده", "", false, false);
        var reportCountSerie = new InfoSerie("تعداد", "");
        infoChart.Add(reportCountSerie);
        reportCountSerie.Add(new DataItem(
            "اپراتور",
            (total - citizenCount).ToString(),
            GetPercent(total - citizenCount, total)));
        reportCountSerie.Add(new DataItem(
            "شهروند",
            citizenCount.ToString(),
            GetPercent(citizenCount, total)));

        result.Add(infoChart.Sort(0));
        return result;
    }


    public async Task<InfoModel> GetSatisrfactionsPerOperator(GetInfoQueryParameters queryParameters)
    {
        var operatorIds = (await userRepository.GetUsersInRole(RoleNames.Operator))
            .Where(u => u.ShahrbinInstanceId == queryParameters.InstanceId)
            .Select(u => new {Id = u.Id, Title = u.Title})
            .ToList();

        var query = unitOfWork.DbContext.Set<Satisfaction>().AsNoTracking();

        if (queryParameters.ReportFilters.FromDate != null)
        {
            query = query.Where(s => s.DateTime >= queryParameters.ReportFilters.FromDate);
        }

        if (queryParameters.ReportFilters.ToDate != null)
        {
            query = query.Where(s => s.DateTime < queryParameters.ReportFilters.ToDate.Value.AddDays(1));
        }

        var hist = await query
            .GroupBy(s => s.ActorId)
            .Select(g => new { OpId = g.Key, Count = g.LongCount()})
            .ToListAsync();

        var total = hist.Sum(h => h.Count);
        
        var result = new InfoModel();
        var infoChart = new InfoChart("تعداد خشنودی سنجی بر اساس هر اپراتور", "", false, false);
        var CountSerie = new InfoSerie("تعداد", "");

        foreach (var item in operatorIds)
        {
            var relatedHist = hist.Where(e => e.OpId == item.Id).SingleOrDefault();
            if (relatedHist == null)
                relatedHist = new { OpId = item.Id, Count = (long)0 };

            CountSerie.Add(new DataItem(
                item.Title,
                relatedHist.Count.ToString(),
                GetPercent(relatedHist.Count, total)));
        }

        infoChart.Add(CountSerie);
        result.Add(infoChart.Sort(0));
        return result;
    }


    public async Task<InfoModel> GetSatisrfactionsAveragePerOperator(GetInfoQueryParameters queryParameters)
    {
        var operatorIds = (await userRepository.GetUsersInRole(RoleNames.Operator))
            .Where(u => u.ShahrbinInstanceId == queryParameters.InstanceId)
            .Select(u => new { Id = u.Id, Title = u.Title })
            .ToList();

        var query = unitOfWork.DbContext.Set<Satisfaction>().AsNoTracking();

        if (queryParameters.ReportFilters.FromDate != null)
        {
            query = query.Where(s => s.DateTime >= queryParameters.ReportFilters.FromDate);
        }

        if (queryParameters.ReportFilters.ToDate != null)
        {
            query = query.Where(s => s.DateTime < queryParameters.ReportFilters.ToDate.Value.AddDays(1));
        }

        var hist = await query
            .GroupBy(s => s.ActorId)
            .Select(g => new { OpId = g.Key, Average = g.Average(e => e.Rating) })
            .ToListAsync();


        var result = new InfoModel();
        var infoChart = new InfoChart("میانگین خشنودی سنجی بر اساس هر اپراتور", "", false, false);
        var averageSerie = new InfoSerie("میانگین", "");

        foreach (var item in operatorIds)
        {
            var relatedHist = hist.Where(e => e.OpId == item.Id).SingleOrDefault();
            if (relatedHist == null)
                relatedHist = new { OpId = item.Id, Average = (double)0 };

            averageSerie.Add(new DataItem(
                item.Title,
                relatedHist.Average.ToString("0.00"),
                relatedHist.Average.ToString("0.00")));
        }

        infoChart.Add(averageSerie);
        result.Add(infoChart.Sort(0));
        return result;
    }

    //Locations
    public async Task<InfoModel> GetLocations(GetInfoQueryParameters queryParameters)
    {
        var result = new InfoModel();
        var locationsQuery = unitOfWork.DbContext.Set<Report>()
            .Where(r => r.ShahrbinInstanceId == queryParameters.InstanceId)
            .Where(r => r.Address.Location != null);

        locationsQuery = await addRestrictions(locationsQuery, queryParameters);
        

        List<LocationItem> locations;
        locations = await locationsQuery
            .Select(r => new LocationItem(r.Id, r.Address.Location!.Y, r.Address.Location!.X))
            .ToListAsync();

        if (locations is null)
            locations = new List<LocationItem>();

        result.Add(new LocationInfo(locations));

        return result;
    }

    public async Task<InfoModel> GetCitizenReportLocations(int instanceId, List<string> roles)
    {
        var result = new InfoModel();
        var locationsQuery = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId)
            .Where(r => r.Address.Location != null)
            .Where(r => (r.Category.Role.Name != null) && roles.Contains(r.Category.Role.Name))
            .OrderByDescending(r => r.LastStatusDateTime)
            .Take(100);


        List<LocationItem> locations;
        locations = await locationsQuery
            .Select(r => new LocationItem(r.Id, r.Address.Location!.Y, r.Address.Location!.X))
            .ToListAsync();

        if (locations is null)
            locations = new List<LocationItem>();

        result.Add(new LocationInfo(locations));

        return result;
    }

    //Reports
    public async Task<PagedList<T>> GetReports<T>(
        GetInfoQueryParameters queryParameters,
        Expression<Func<Report, T>> selector,
        PagingInfo pagingInfo)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking();

        query = await addRestrictions(query, queryParameters);

        var query2 = query
            .AsNoTracking()
            .OrderBy(r => r.Sent)
            .Select(selector);

        var reports = await PagedList<T>.ToPagedList(
            query2,
            pagingInfo.PageNumber,
            pagingInfo.PageSize);

        return reports;
    }

    public async Task<Result<MemoryStream>> GetExcel(GetInfoQueryParameters queryParameters)
    {
        var query = unitOfWork.DbContext.Set<Report>().AsNoTracking();
        query = await addRestrictions(query, queryParameters);

        query = query
            .Include(p => p.Citizen)
            .Include(p => p.Registrant)
            .Include(p => p.Executive)
            .Include(p => p.Contractor)
            .Include(p => p.Inspector)
            .Include(p => p.Category)
            .Include(p => p.Category)
            .ThenInclude(p => p.Parent)
            .Include(p => p.Address)
            .Include(p => p.Medias)
            .Include(p => p.LikedBy)
            .Include(p => p.LastReason)
            .Include(p => p.TransitionLogs)
            .OrderByDescending(p => p.Sent);

        var result = await query.Take(1000).ToListAsync();
        int i = 1, j = 1;
        int headerRows = 1;
        int rowNum;

        //Creating the workbook
        var t = Task.Run(() =>
        {
            var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Sheet1");
            ws.RightToLeft = true;

            //Header titles
            rowNum = 1;
            ws.Cell(rowNum, j++).Value = "ردیف";
            ws.Cell(rowNum, j++).Value = "تلفن همراه";
            ws.Cell(rowNum, j++).Value = "نام";
            ws.Cell(rowNum, j++).Value = "نام خانوادگی";
            ws.Cell(rowNum, j++).Value = "نام ثبت کننده";
            ws.Cell(rowNum, j++).Value = "نام خانوادگی ثبت کننده";
            ws.Cell(rowNum, j++).Value = "واحد اجرایی";
            ws.Cell(rowNum, j++).Value = "گروه موضوعی";
            ws.Cell(rowNum, j++).Value = "زیرگروه موضوعی";
            ws.Cell(rowNum, j++).Value = "آدرس";
            ws.Cell(rowNum, j++).Value = "شماره رهگیری";
            ws.Cell(rowNum, j++).Value = "تاریخ درخواست";
            ws.Cell(rowNum, j++).Value = "تاریخ اتمام";
            ws.Cell(rowNum, j++).Value = "آخرین وضعیت";
            ws.Cell(rowNum, j++).Value = "نتیجه";
            ws.Cell(rowNum, j++).Value = "بازخورد شهروند";
            ws.Cell(rowNum, j++).Value = "اعتراض شهروند";
            ws.Cell(rowNum, j++).Value = "متن درخواست";
            ws.Cell(rowNum, j++).Value = "تاریخچه درخواست";


            rowNum = headerRows;
            i = 0;
            foreach (var report in result)
            {
                rowNum++;
                i++;
                j = 1;

                ws.Cell(rowNum, j++).Value = i;
                ws.Cell(rowNum, j++).Value = report.Citizen.PhoneNumber;
                ws.Cell(rowNum, j++).Value = report.Citizen.FirstName;
                ws.Cell(rowNum, j++).Value = report.Citizen.LastName;
                ws.Cell(rowNum, j++).Value = report.Registrant?.FirstName;
                ws.Cell(rowNum, j++).Value = report.Registrant?.LastName;
                ws.Cell(rowNum, j++).Value = $"{report.Executive?.Title} ({report.Executive?.FirstName} {report.Executive?.LastName})";
                ws.Cell(rowNum, j++).Value = report.Category.Parent?.Title;
                ws.Cell(rowNum, j++).Value = report.Category.Title;
                ws.Cell(rowNum, j++).Value = report.Address.Detail;
                ws.Cell(rowNum, j++).Value = report.TrackingNumber;
                ws.Cell(rowNum, j++).Value = report.Sent.GregorianToPersian();
                ws.Cell(rowNum, j++).Value = (report.Finished != null) ? report.Finished.Value.GregorianToPersian() : "";
                ws.Cell(rowNum, j++).Value = report.LastStatus;
                ws.Cell(rowNum, j++).Value = (report.LastReason != null) ? report.LastReason.Title : "";
                ws.Cell(rowNum, j++).Value = report.IsFeedbacked ? "بله" : "خیر";
                ws.Cell(rowNum, j++).Value = report.IsObjectioned ? "بله" : "خیر";
                ws.Cell(rowNum, j++).Value = report.Comments;
                var transitions = "";
                foreach (var tl in report.TransitionLogs)
                {
                    transitions += tl.DateTime.GregorianToPersian();
                    transitions += ":";
                    transitions += tl.Message;
                    transitions += "-";
                    transitions += tl.Comment;
                    transitions += "\r\n";
                }
                ws.Cell(rowNum, j++).Value = transitions;

            }
            ws.Columns().AdjustToContents();
            return wb;
        });

        var wb = await t;
        var stream = new MemoryStream();
        wb.SaveAs(stream);

        return stream;
    }

    public IQueryable<Report> AddFilters(IQueryable<Report> query, ReportFilters reportFilters)
    {
        if (reportFilters.Query is not null)
        {
            var phrase = reportFilters.Query.Trim();
            if (phrase.Length >= 3)
            {
                query = query.Where(r =>
                        r.TrackingNumber.Contains(phrase) ||
                        r.Citizen.FirstName.Contains(phrase) ||
                        r.Citizen.LastName.Contains(phrase) ||
                        r.Citizen.PhoneNumber != null && r.Citizen.PhoneNumber.Contains(phrase));
            }
        }

        if (reportFilters.FromDate != null)
        {
            query = query.Where(r => r.Sent >= reportFilters.FromDate);
        }

        if (reportFilters.ToDate != null)
        {
            query = query.Where(r => r.Sent < reportFilters.ToDate.Value.AddDays(1));
        }

        if (reportFilters.Categories != null)
        {
            query = query.Where(r => reportFilters.Categories.Contains(r.CategoryId));
        }

        if (reportFilters.Priorities != null)
        {
            var priorities = reportFilters.Priorities.Select(p => (Priority)p).ToList();
            query = query.Where(r => priorities.Contains(r.Priority));
        }

        if (reportFilters.States != null)
        {
            var states = reportFilters.States.Select(s => (ReportState)s).ToList();
            query = query.Where(r => states.Contains(r.ReportState));
        }

        if (reportFilters.Regions != null)
        {
            query = query.Where(r => r.Address.RegionId != null && reportFilters.Regions.Contains(r.Address.RegionId.Value));
        }

        if (reportFilters.SatisfactionValues != null)
        {
            query = query.Where(r => reportFilters.SatisfactionValues.Contains(0) && r.Satisfaction == null ||
                                     r.Satisfaction != null && reportFilters.SatisfactionValues.Contains(r.Satisfaction.Rating));
        }

        if(reportFilters.Execitives != null)
        {
            query = query.Where(r => r.ExecutiveId != null && reportFilters.Execitives.Contains(r.ExecutiveId));
        }

        return query;
    }

    public IQueryable<Satisfaction> AddSatisrfactionsFilters(IQueryable<Satisfaction> query, GetInfoQueryParameters queryParameters)
    {
        //if (queryParameters.ReportFilters.Query is not null)
        //{
        //    var phrase = queryParameters.ReportFilters.Query.Trim();
        //    if (phrase.Length >= 3)
        //    {
        //        query = query.Where(r =>
        //                r.Report.TrackingNumber.Contains(phrase) ||
        //                r.Report.Citizen.FirstName.Contains(phrase) ||
        //                r.Report.Citizen.LastName.Contains(phrase) ||
        //                r.Report.Citizen.PhoneNumber != null && r.Report.Citizen.PhoneNumber.Contains(phrase));
        //    }
        //}

        //List<string> userIds = new List<string> { queryParameters.UserId };
        //bool returnAll = false;
        //if (queryParameters.Roles.Contains(RoleNames.Manager))
        //{
        //    userIds.AddRange(await GetUserIdsOfOrganizationalUnit(queryParameters.UserId));
        //    query = query.Where(r => r.Report.Address.RegionId != null);
        //}
        //else
        //{
        //    var regionIds = unitOfWork.DbContext.Set<Actor>()
        //        .Where(a => a.Identifier == queryParameters.UserId)
        //        .SelectMany(a => a.Regions.Select(r => r.Id));
        //    query = query.Where(r => r.Report.Address.RegionId != null && regionIds.Contains(r.Report.Address.RegionId.Value));

        //    if (queryParameters.Roles.Contains(RoleNames.Operator))
        //    {
        //        returnAll = true;
        //        var categoryIds = unitOfWork.DbContext.Set<Category>()
        //            .Where(c => c.Users.Select(u => u.Id).Contains(queryParameters.UserId))
        //            .Select(c => c.Id);
        //        query = query.Where(r => categoryIds.Contains(r.Report.CategoryId));
        //    }
        //    else if (queryParameters.Roles.Contains(RoleNames.Mayor))
        //    {
        //        returnAll = true;
        //    }
        //    else if (queryParameters.Roles.Contains(RoleNames.Inspector))
        //    {
        //        returnAll = true;
        //    }
        //}

        //var reportIds = unitOfWork.DbContext.Set<TransitionLog>()
        //    .Where(tl => userIds.Contains(tl.ActorIdentifier))
        //    .Select(tl => tl.ReportId)
        //    .Distinct();
        //var roleIds = unitOfWork.DbContext.Set<ApplicationRole>()
        //    .Where(r => queryParameters.Roles.Contains(r.Name!))
        //    .Select(r => r.Id);
        //var actorIds = unitOfWork.DbContext.Set<Actor>()
        //    .Where(a => userIds.Contains(a.Identifier) || roleIds.Contains(a.Identifier))
        //    .Select(a => a.Id)
        //    .Distinct();

        //if (queryParameters.ReportsToInclude is null || queryParameters.ReportsToInclude.Count() == 0)
        //{
        //    query = query.Where(r => reportIds.Contains(r.Id) || returnAll ||
        //                             r.Report.CurrentActorId != null && actorIds.Contains(r.Report.CurrentActorId.Value));
        //}
        //else
        //{
        //    var reportsToInclude = queryParameters.ReportsToInclude;
        //    if (reportsToInclude.Contains(ReportsToInclude.Interacted) && reportsToInclude.Contains(ReportsToInclude.InCartable))
        //    {
        //        query = query.Where(r => reportIds.Contains(r.Id) || returnAll ||
        //                            r.Report.CurrentActorId != null && actorIds.Contains(r.Report.CurrentActorId.Value));
        //    }
        //    else if (reportsToInclude.Contains(ReportsToInclude.Interacted))
        //    {
        //        query = query.Where(r => reportIds.Contains(r.Id));
        //    }
        //    else if (reportsToInclude.Contains(ReportsToInclude.InCartable))
        //    {
        //        query = query.Where(r => returnAll || r.Report.CurrentActorId != null && actorIds.Contains(r.Report.CurrentActorId.Value));
        //    }

        //}


        if (queryParameters.ReportFilters.FromDate != null)
        {
            query = query.Where(s => s.DateTime >= queryParameters.ReportFilters.FromDate);
        }
        if (queryParameters.ReportFilters.ToDate != null)
        {
            query = query.Where(s => s.DateTime < queryParameters.ReportFilters.ToDate.Value.AddDays(1));
        }

        if (queryParameters.ReportFilters.Categories != null)
        {
            query = query.Where(r => queryParameters.ReportFilters.Categories.Contains(r.Report.CategoryId));
        }

        if (queryParameters.ReportFilters.Regions != null)
        {
            query = query.Where(r => r.Report.Address.RegionId != null &&
            queryParameters.ReportFilters.Regions.Contains(r.Report.Address.RegionId.Value));
        }

        if (queryParameters.ReportFilters.SatisfactionValues != null)
        {
            query = query.Where(r => queryParameters.ReportFilters.SatisfactionValues.Contains(0) && r.Report.Satisfaction == null ||
                                     r.Report.Satisfaction != null && queryParameters.ReportFilters.SatisfactionValues.Contains(r.Rating));
        }

        if (queryParameters.Geometry is not null && queryParameters.Geometry.Count > 0)
        {
            var geometryFactory = new GeometryFactory();
            var coordinates = queryParameters.Geometry.Select(g => new Coordinate(g.Longitude, g.Latitude)).ToList();
            var geometry = geometryFactory.CreatePolygon(coordinates.ToArray());
            if (!geometry.Shell.IsCCW)
            {
                coordinates.Reverse();
                geometry = geometryFactory.CreatePolygon(coordinates.ToArray());
            }
            geometry.SRID = 4326;
            query = query.Where(r => geometry.Contains(r.Report.Address.Location));
        }

        return query;
    }

    /*******************************************************/
    private async Task<InfoModel> GetReportsTimeHistogram<T, Key>(
        string title,
        List<Bin<Key>> bins,
        IQueryable<Report> query,
        Expression<Func<Report, Key>> groupBy)
    {
        var result = new InfoModel();
        var infoChart = new InfoChart(title, "", false, false);

        var groupedQuery = await query
            .Where(r => r.Duration != null)
            .GroupBy(groupBy)
            .Select(p => new
            {
                Id = p.Key,
                Duration = p.Average(r => r.Duration),
                ResponseDuration = p.Average(r => r.ResponseDuration)
            })
            .ToListAsync();
        var doneSerie = new InfoSerie("متوسط زمان انجام", "");
        var responseSerie = new InfoSerie("متوسط زمان پاسخ", "");
        infoChart.Add(doneSerie);
        infoChart.Add(responseSerie);
        foreach (var bin in bins)
        {
            var duration = groupedQuery
                .Where(g => EqualityComparer<Key>.Default.Equals(g.Id, bin.Id))
                .Select(g => g.Duration)
                .SingleOrDefault();

            var responseDuration = groupedQuery
                .Where(g => EqualityComparer<Key>.Default.Equals(g.Id, bin.Id))
                .Select(g => g.ResponseDuration)
                .SingleOrDefault();

            if (duration is null && responseDuration is null)
                continue;

            duration ??= 0;
            var durationTimeSpan = new TimeSpan(0, 0, (int)duration);

            responseDuration ??= 0;
            var responseDurationTimeSpan = new TimeSpan(0, 0, (int)responseDuration);

            doneSerie.Add(new DataItem(
                bin.Title,
                durationTimeSpan.ToHoursValue(),
                durationTimeSpan.ToPersianString()));

            responseSerie.Add(new DataItem(
                bin.Title,
                responseDurationTimeSpan.ToHoursValue(),
                responseDurationTimeSpan.ToPersianString()));
        }

        result.Add(infoChart.Sort(0));
        return result;
    }

    private record Bin<Key>(Key Id, string Title);

    private static List<Bin<T>> GetBins<T>() where T : Enum
    {
        var values = (T[])Enum.GetValues(typeof(T));
        var bins = new List<Bin<T>>();
        values.ToList().ForEach(bin => { bins.Add(new Bin<T>(bin, bin.GetDescription() ?? "")); });
        return bins;
    }

    private string GetPercent(long value, long total)
    {
        var percent = Math.Round((double)value / (total == 0 ? 1 : total) * 10000) / 100;
        return $"{percent}% ({value})";
    }

    private record ActorUserRegions(int ActorId, string UserId, List<int> RegionIds);

    public async Task<List<string>> GetUserIdsOfOrganizationalUnit(string userId)
    {
        var organizationalUnits = await unitOfWork.DbContext.Set<OrganizationalUnit>()
            .Include(o => o.OrganizationalUnits)
            .AsNoTracking()
            .ToListAsync();

        var organizationalUnit = organizationalUnits.Where(o => o.UserId == userId).FirstOrDefault();
        var userIds = new List<string>();
        if (organizationalUnit != null)
        {
            var queue = new Queue<OrganizationalUnit>();
            queue.Enqueue(organizationalUnit);

            while (queue.Count > 0)
            {
                var t = queue.Dequeue();
                if (t.Type == OrganizationalUnitType.Executive || t.Type == OrganizationalUnitType.Person)
                {
                    userIds.Add(t.UserId);
                }
                var childOus = organizationalUnits.Where(o => o.Id == t.Id).First().OrganizationalUnits;
                foreach (var child in childOus)
                {
                    queue.Enqueue(child);
                }
            }
        }

        return userIds;
    }


    private async Task<IQueryable<Report>> addRestrictions(IQueryable<Report> query, GetInfoQueryParameters queryParameters)
    {
        query = query.Where(r => !r.IsDeleted);
        bool returnAll = false;
        List<string> userIds = new List<string> { queryParameters.UserId };

        if (queryParameters.Roles.Contains(RoleNames.Manager))
        {
            userIds.AddRange(await GetUserIdsOfOrganizationalUnit(queryParameters.UserId));
            query = query.Where(r => r.Address.RegionId != null);
        }
        else
        {
            var regionIds = unitOfWork.DbContext.Set<Actor>()
                .Where(a => a.Identifier == queryParameters.UserId)
                .SelectMany(a => a.Regions.Select(r => r.Id));
            query = query.Where(r => r.Address.RegionId != null && regionIds.Contains(r.Address.RegionId.Value));

            if (queryParameters.Roles.Contains(RoleNames.Operator))
            {
                returnAll = true;
                var categoryIds = unitOfWork.DbContext.Set<Category>()
                    .Where(c => c.Users.Select(u => u.Id).Contains(queryParameters.UserId))
                    .Select(c => c.Id);
                query = query.Where(r => categoryIds.Contains(r.CategoryId));
            }
            else if (queryParameters.Roles.Contains(RoleNames.Mayor))
            {
                returnAll = true;
            }
            else if(queryParameters.Roles.Contains(RoleNames.Inspector))
            {
                returnAll = true;
            }
        }
         
        var reportIds = unitOfWork.DbContext.Set<TransitionLog>()
            .Where(tl => userIds.Contains(tl.ActorIdentifier))
            .Select(tl => tl.ReportId)
            .Distinct();
        var roleIds = unitOfWork.DbContext.Set<ApplicationRole>()
            .Where(r => queryParameters.Roles.Contains(r.Name!))
            .Select(r => r.Id);
        var actorIds = unitOfWork.DbContext.Set<Actor>()
            .Where(a => userIds.Contains(a.Identifier) || roleIds.Contains(a.Identifier))
            .Select(a => a.Id)
            .Distinct();

        if(queryParameters.ReportsToInclude is null || queryParameters.ReportsToInclude.Count() == 0)
        {
            query = query.Where(r => reportIds.Contains(r.Id) || returnAll ||
                                     r.CurrentActorId != null && actorIds.Contains(r.CurrentActorId.Value));
        }
        else
        {
            var reportsToInclude = queryParameters.ReportsToInclude;
            if (reportsToInclude.Contains(ReportsToInclude.Interacted) && reportsToInclude.Contains(ReportsToInclude.InCartable))
            {
                query = query.Where(r => reportIds.Contains(r.Id) || returnAll ||
                                    r.CurrentActorId != null && actorIds.Contains(r.CurrentActorId.Value));
            }
            else if(reportsToInclude.Contains(ReportsToInclude.Interacted))
            {
                query = query.Where(r => reportIds.Contains(r.Id));
            }
            else if (reportsToInclude.Contains(ReportsToInclude.InCartable))
            {
                query = query.Where(r => returnAll || r.CurrentActorId != null && actorIds.Contains(r.CurrentActorId.Value));
            }

        }
        


        if (queryParameters.Geometry is not null && queryParameters.Geometry.Count > 0)
        {
            var geometryFactory = new GeometryFactory();
            var coordinates = queryParameters.Geometry.Select(g => new Coordinate(g.Longitude, g.Latitude)).ToList();
            var geometry = geometryFactory.CreatePolygon(coordinates.ToArray());
            if (!geometry.Shell.IsCCW)
            {
                coordinates.Reverse();
                geometry = geometryFactory.CreatePolygon(coordinates.ToArray());
            }
            geometry.SRID = 4326;
            query = query.Where(r => geometry.Contains(r.Address.Location));
        }

        query = AddFilters(query, queryParameters.ReportFilters);

        return query;
    }

}
