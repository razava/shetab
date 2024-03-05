using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Info.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using SharedKernel.ExtensionMethods;
using System.Linq.Expressions;

namespace Infrastructure.Info;

public class InfoService(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IActorRepository actorRepository) : IInfoService
{
    //Status
    public async Task<InfoModel> GetReportsStatusPerCategory(int instanceId, string? parameter)
    {
        int parentCategoryId;
        var result = new InfoModel();

        if (int.TryParse(parameter, out int id))
        {
            parentCategoryId = id;
        }
        else
        {
            return result;
        }

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک دسته بندی", "", false, false);

        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId);

        var groupedQuery = await query
            .GroupBy(q => new { q.CategoryId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
        .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var categories = await unitOfWork.DbContext.Set<Category>()
            .AsNoTracking()
            .Where(c => c.ShahrbinInstanceId == instanceId)
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

        result.Add(infoChart.Sort());

        return result;
    }

    public async Task<InfoModel> GetReportsStatusPerExecutive(int instanceId)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک واحد اجرایی", "", false, false);

        //todo : should not be filtered by regions related to user?
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId);

        var groupedQuery = await query
            .GroupBy(q => new { q.ExecutiveId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
            .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var executiveIds = (await userRepository.GetUsersInRole(RoleNames.Executive))
            .Where(u => u.ShahrbinInstanceId == instanceId)
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

        result.Add(infoChart.Sort());

        return result;
    }

    public async Task<InfoModel> GetReportsStatusPerContractor(int instanceId)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک پیمانکار", "", false, false);

        //todo : should not be filtered by regions related to user?
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId);

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

        result.Add(infoChart.Sort());

        return result;
    }

    public async Task<InfoModel> GetReportsStatusPerRegion(int instanceId)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک منطقه", "", false, false);

        //todo : should not be filtered by regions related to user?
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId);

        var groupedQuery = await query
            .GroupBy(q => new { q.Address.RegionId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
            .Select(q => new { q.Key, Count = q.LongCount() })
            .ToListAsync();

        var cityId = await unitOfWork.DbContext.Set<ShahrbinInstance>()
            .AsNoTracking().Where(s => s.Id == instanceId).
            Select(s => s.CityId).SingleOrDefaultAsync();

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

        result.Add(infoChart.Sort());

        return result;
    }


    //Statistics
    public async Task<InfoModel> GetUsersStatistics(int instanceId)
    {
        var result = new InfoModel();
        var userContext = unitOfWork.DbContext.Set<ApplicationUser>().AsNoTracking();

        var totalPersonel = await userContext.Where(e =>
        e.ShahrbinInstanceId != null && e.ShahrbinInstanceId == instanceId).LongCountAsync();

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

    public async Task<InfoModel> GetReportsStatistics(int instanceId)
    {
        var result = new InfoModel();
        var query = unitOfWork.DbContext.Set<Report>().AsNoTracking().Where(r => r.ShahrbinInstanceId == instanceId);

        var totalReports = await query.LongCountAsync();

        var now = DateTime.UtcNow;
        var lastDay = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(1, 0, 0, 0))).LongCountAsync();
        var lastWeek = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(7, 0, 0, 0))).LongCountAsync();
        var lastMonth = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(30, 0, 0, 0))).LongCountAsync();

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

    public async Task<InfoModel> GetTimeStatistics(int instanceId)
    {
        var result = new InfoModel();
        var query = unitOfWork.DbContext.Set<Report>().AsNoTracking().Where(e => e.ShahrbinInstanceId == instanceId);

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

    public async Task<InfoModel> GetSatisfactionStatistics(int instanceId)
    {
        var result = new InfoModel();

        var groupedQuery = await unitOfWork.DbContext.Set<Satisfaction>()
            .Where(s => s.Report.ShahrbinInstanceId == instanceId)
            .GroupBy(s => s.Rating)
            .Select(s => new { s.Key, Count = s.Count() })
            .ToListAsync();

        var total = groupedQuery.Sum(s => s.Count);
        var averageRating = (double)groupedQuery.Sum(s => s.Key * s.Count) / total;
        result.Singletons.Add(new InfoSingleton(total.ToString(), "تعداد کل", ""));
        result.Singletons.Add(new InfoSingleton(averageRating.ToString("0.00"), "متوسط امتیاز", ""));

        var ratingChart = new InfoChart("فراوانی امتیازهای خشنودی سنجی", "", false, false);
        var ratingSerie = new InfoSerie("تعداد", "");
        ratingChart.Add(ratingSerie);
        for (var i = 1; i <= 5; i++)
        {
            var count = groupedQuery.Where(r => r.Key == i).Select(r => r.Count).SingleOrDefault();
            ratingSerie.Add(new DataItem(i.ToString(), count.ToString(), GetPercent(count, total)));
        }

        result.Charts.Add(ratingChart);

        var objectionedCount = await unitOfWork.DbContext.Set<Report>()
            .Where(r => r.ShahrbinInstanceId == instanceId && r.IsObjectioned)
            .LongCountAsync();
        result.Singletons.Add(new InfoSingleton(objectionedCount.ToString(), "ارجاع به بازرسی", ""));

        return result;
    }

    public async Task<InfoModel> GetActiveCitizens(int instanceId)
    {
        var groupedQuery = await unitOfWork.DbContext.Set<Report>()
            .Where(r => r.ShahrbinInstanceId == instanceId)
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
        result.Charts.Add(chart);
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
    public async Task<InfoModel> GetReportsTimePerCategory(int instanceId, string? parameter)
    {
        int parentCategoryId;
        var result = new InfoModel();

        if (int.TryParse(parameter, out int id))
        {
            parentCategoryId = id;
        }
        else
        {
            return result;
        }

        var infoChart = new InfoChart("زمان رسیدگی به تفکیک دسته بندی", "", false, false);

        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId);

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
            .Where(c => c.ShahrbinInstanceId == instanceId)
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
            var duration = groupedQuery
                .Where(g => g.Id == category.Id)
                .Select(g => g.Duration)
                .SingleOrDefault();

            var responseDuration = groupedQuery
                .Where(g => g.Id == category.Id)
                .Select(g => g.ResponseDuration)
                .SingleOrDefault();

            duration ??= 0;
            var durationTimeSpan = new TimeSpan(0, 0, (int)duration);

            responseDuration ??= 0;
            var responseDurationTimeSpan = new TimeSpan(0, 0, (int)responseDuration);

            doneSerie.Add(new DataItem(
                category.Title,
                durationTimeSpan.ToHoursValue(),
                durationTimeSpan.ToPersianString()));

            responseSerie.Add(new DataItem(
                category.Title,
                responseDurationTimeSpan.ToHoursValue(),
                responseDurationTimeSpan.ToPersianString()));
        }

        result.Add(infoChart.Sort());
        return result;
    }

    public async Task<InfoModel> GetReportsTimeByRegion(int instanceId)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId)
            .Include(r => r.Address);

        var cityId = await unitOfWork.DbContext.Set<ShahrbinInstance>()
            .AsNoTracking().Where(s => s.Id == instanceId)
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

    public async Task<InfoModel> GetRepportsTimeByExecutive(int instanceId)
    {

        var query = unitOfWork.DbContext.Set<Report>()
        .AsNoTracking()
        .Where(r => r.ShahrbinInstanceId == instanceId);

        var executives = (await userRepository.GetUsersInRole(RoleNames.Executive))
            .Where(u => u.ShahrbinInstanceId == instanceId)
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
    public async Task<InfoModel> GetRequestsPerOperator(int instanceId)
    {
        var operatorIds = (await userRepository.GetUsersInRole(RoleNames.Operator))
            .Where(u => u.ShahrbinInstanceId == instanceId)
            .Select(u => u.Id)
            .ToList();

        var hist = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => operatorIds.Contains(u.Id))
            .Select(u => new { u.Title, Count = u.RegisteredReports.Count() })
            .ToListAsync();
        var total = hist.Sum(h => h.Count);

        var result = new InfoModel();
        var infoChart = new InfoChart("تعداد درخواست ثبت شده توسط هر اپراتور", "", false, false);
        var reportCountSerie = new InfoSerie("تعداد", "");
        infoChart.Add(reportCountSerie);

        foreach (var item in hist)
        {
            reportCountSerie.Add(new DataItem(
                item.Title,
                item.Count.ToString(),
                GetPercent(item.Count, total))); ;
        }

        result.Add(infoChart.Sort());
        return result;
    }

    public async Task<InfoModel> GetRequestsPerRegistrantType(int instanceId)
    {
        var operatorIds = (await userRepository.GetUsersInRole(RoleNames.Operator))
            .Where(u => u.ShahrbinInstanceId == instanceId)
            .Select(u => u.Id)
            .ToList();

        var hist = await unitOfWork.DbContext.Set<Report>()
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

        result.Add(infoChart.Sort());
        return result;
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

        result.Add(infoChart.Sort());
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

    public async Task<InfoModel> GetLocations(int instanceId)
    {
        var result = new InfoModel();
        var locations = await unitOfWork.DbContext.Set<Report>()
            .Where(r => r.ShahrbinInstanceId == instanceId)
            .Where(r => r.Address.Location != null)
            .Select(r => new InfoLocation(r.Id, r.Address.Location!.Y, r.Address.Location!.X))
            .ToListAsync();

        result.Locations = locations;

        return result;
    }
}
