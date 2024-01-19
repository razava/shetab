using Application.Common.Interfaces.Persistence;
using Application.Info.Common;
using DocumentFormat.OpenXml.InkML;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Application.Info.Queries.GetInfoQuery;

internal class GetInfoQueryHandler : IRequestHandler<GetInfoQuery, InfoModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetInfoQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<InfoModel> Handle(GetInfoQuery request, CancellationToken cancellationToken)
    {
        var code = request.Code % 100000;
        InfoModel result = new InfoModel();

        //todo : .............. query shoud be pri processed for relate to current user *********

        switch (code)
        {
            case 1:
                result = await GetUsersStatistics(request.InstanceId);
                break;
            case 2:
                result = await GetReportsStatistics(request.InstanceId);
                break;
            case 3:
                result = await GetTimeStatistics(request.InstanceId);
                break;
            case 102:
                result = await GetReportsStatusByCategory(request.InstanceId);
                break;
            default:
                break;
        }



        return result;
    }



    private async Task<InfoModel> GetUsersStatistics(int instanceId)
    {
        var result = new InfoModel();
        var userContext = _unitOfWork.DbContext.Set<ApplicationUser>().AsNoTracking();

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
     

    private async Task<InfoModel> GetReportsStatistics(int instanceId)
    {
        var result = new InfoModel();
        var query = _unitOfWork.DbContext.Set<Report>().AsNoTracking().Where(r => r.ShahrbinInstanceId == instanceId);

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

    private async Task<InfoModel> GetTimeStatistics(int instanceId)
    {
        var result = new InfoModel();
        var query = _unitOfWork.DbContext.Set<Report>().AsNoTracking().Where(e => e.ShahrbinInstanceId == instanceId);

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


    private async Task<InfoModel> GetReportsStatusByCategory(int instanceId)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک دسته بندی", "", false, false);

        var query = _unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId);

        var groupedQuery = await query
            .GroupBy(q => new { q.CategoryId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
            .Select(q => new { Key = q.Key, Count = q.LongCount() })
            .ToListAsync();

        var categories = await _unitOfWork.DbContext.Set<Category>()
            .AsNoTracking()
            .Where(c => c.ShahrbinInstanceId == instanceId && c.ParentId == null)
            .Include(c => c.Categories)
            .ThenInclude(x => x.Categories)
            .Select(e => e.Categories)
            .SingleOrDefaultAsync();

        //var temp = new List<long>();

        foreach (var category in categories)
        {
            var seri = new InfoSerie(category.Title, "");
            var catDescendant = category.Siblings;

            var finished = groupedQuery.Where(g => catDescendant.Contains(g.Key.CategoryId) && 
            (g.Key.ReportState == ReportState.Finished || g.Key.ReportState == ReportState.Accepted))
                .Sum(g => g.Count);

            var live = groupedQuery.Where(g => catDescendant.Contains(g.Key.CategoryId) && 
            (g.Key.ReportState == ReportState.Live || g.Key.ReportState == ReportState.Review))
                .Sum(g => g.Count);

            var needAcceptance = groupedQuery.Where(g => catDescendant.Contains(g.Key.CategoryId) &&
            g.Key.ReportState == ReportState.NeedAcceptance)
                .Sum(g => g.Count);

            var total = finished + live + needAcceptance;

            if(total == 0)
                continue;

            var feedbacked = groupedQuery.Where(g => catDescendant.Contains(g.Key.CategoryId) &&
            g.Key.IsFeedbacked == true)
                .Sum(g => g.Count);

            var objectioned = groupedQuery.Where(g => catDescendant.Contains(g.Key.CategoryId) &&
            g.Key.IsObjectioned == true)
                .Sum(g => g.Count);

            seri.Add(new DataItem(
                "کل",
                total.ToString(),
                total.ToString()));

            seri.Add(new DataItem(
                "در حال رسیدگی",
                live.ToString(),
                GetPercent(live, total)));

            seri.Add(new DataItem(
                "رسیدگی شده",
                finished.ToString(),
                GetPercent(finished, total)));

            seri.Add(new DataItem(
                "در انتظار تایید",
                needAcceptance.ToString(),
                GetPercent(needAcceptance, total)));

            seri.Add(new DataItem(
                "بازخورد شهروند",
                feedbacked.ToString(),
                GetPercent(feedbacked, total)));

            seri.Add(new DataItem(
                "اعتراض شهروند",
                objectioned.ToString(),
                GetPercent(objectioned, total)));

            infoChart.Add(seri);

            //temp.Add(finished);
        }

        //infoChart.Series = infoChart.Series.OrderByDescending(s => long.Parse(s.Values[0].Value)).ToList();

        result.Add(infoChart.Sort());

        return result;
    }



    private string GetPercent(long value, long total)
    {
        var percent = Math.Round(((double)value / (total == 0 ? 1 : total)) * 10000) / 100;
        return $"{percent}% ({value})";
    }

}
