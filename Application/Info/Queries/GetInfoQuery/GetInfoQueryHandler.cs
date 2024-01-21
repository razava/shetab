using Application.Common.Extentions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Info.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Info.Queries.GetInfoQuery;

internal class GetInfoQueryHandler : IRequestHandler<GetInfoQuery, InfoModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IActorRepository _actorRepository;

    public GetInfoQueryHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IActorRepository actorRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _actorRepository = actorRepository;
    }


    public async Task<InfoModel> Handle(GetInfoQuery request, CancellationToken cancellationToken)
    {
        var code = request.Code % 100000;
        InfoModel result = new InfoModel();

        //todo : .............. query shoud be pre processed for relate to current user *********

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
                result = await GetReportsStatusPerCategory(request.InstanceId);
                break;
            case 104:
                result = await GetReportsStatusPerRegion(request.InstanceId);
                break;
            case 203:
                result = await GetRepportsTimeByExecutive(request.InstanceId);
                break;
            case 204:
                result = await GetRepportsTimeByRegion(request.InstanceId);
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


    private async Task<InfoModel> GetReportsStatusPerCategory(int instanceId)
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
            var serie = new InfoSerie(category.Title, "");
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

            serie.Add(new DataItem(
                "کل",
                total.ToString(),
                total.ToString()));

            serie.Add(new DataItem(
                "در حال رسیدگی",
                live.ToString(),
                GetPercent(live, total)));

            serie.Add(new DataItem(
                "رسیدگی شده",
                finished.ToString(),
                GetPercent(finished, total)));

            serie.Add(new DataItem(
                "در انتظار تایید",
                needAcceptance.ToString(),
                GetPercent(needAcceptance, total)));

            serie.Add(new DataItem(
                "بازخورد شهروند",
                feedbacked.ToString(),
                GetPercent(feedbacked, total)));

            serie.Add(new DataItem(
                "اعتراض شهروند",
                objectioned.ToString(),
                GetPercent(objectioned, total)));

            infoChart.Add(serie);

            //temp.Add(finished);
        }

        //infoChart.Series = infoChart.Series.OrderByDescending(s => long.Parse(s.Values[0].Value)).ToList();

        result.Add(infoChart.Sort());

        return result;
    }


    private async Task<InfoModel> GetReportsStatusPerRegion(int instanceId)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک منطقه", "", false, false);

        //todo : should not be filtered by regions related to user?
        var query = _unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId);

        var groupedQuery = await query
            .GroupBy(q => new { q.Address.RegionId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
            .Select(q => new { Key = q.Key, Count = q.LongCount() })
            .ToListAsync();

        var cityId = await _unitOfWork.DbContext.Set<ShahrbinInstance>()
            .AsNoTracking().Where(s => s.Id == instanceId).
            Select(s => s.CityId).SingleOrDefaultAsync();

        var regions = await _unitOfWork.DbContext.Set<Region>()
            .AsNoTracking()
            .Where(r => r.CityId == cityId)
            .Select(e => new Bin<int>(e.Id, e.Name))
            .ToListAsync();

        foreach (var region in regions)
        {
            var serie = new InfoSerie(region.Title, "");

            var finished = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            (g.Key.ReportState == ReportState.Finished || g.Key.ReportState == ReportState.Accepted))
                .Sum(g => g.Count);

            var live = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            (g.Key.ReportState == ReportState.Live || g.Key.ReportState == ReportState.Review))
                .Sum(g => g.Count);

            var needAcceptance = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            g.Key.ReportState == ReportState.NeedAcceptance)
                .Sum(g => g.Count);

            var total = finished + live + needAcceptance;

            if (total == 0)
                continue;

            var feedbacked = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            g.Key.IsFeedbacked == true)
                .Sum(g => g.Count);

            var objectioned = groupedQuery.Where(g => g.Key.RegionId == region.Id &&
            g.Key.IsObjectioned == true)
                .Sum(g => g.Count);

            serie.Add(new DataItem(
                "کل",
                total.ToString(),
                total.ToString()));

            serie.Add(new DataItem(
                "در حال رسیدگی",
                live.ToString(),
                GetPercent(live, total)));

            serie.Add(new DataItem(
                "رسیدگی شده",
                finished.ToString(),
                GetPercent(finished, total)));

            serie.Add(new DataItem(
                "در انتظار تایید",
                needAcceptance.ToString(),
                GetPercent(needAcceptance, total)));

            serie.Add(new DataItem(
                "بازخورد شهروند",
                feedbacked.ToString(),
                GetPercent(feedbacked, total)));

            serie.Add(new DataItem(
                "اعتراض شهروند",
                objectioned.ToString(),
                GetPercent(objectioned, total)));

            infoChart.Add(serie);

        }

        result.Add(infoChart.Sort());

        return result;
    }



    private async Task<InfoModel> GetRepportsTimeByRegion(int instanceId)
    {
        var query = _unitOfWork.DbContext.Set<Report>()
        .AsNoTracking()
        .Where(r => r.ShahrbinInstanceId == instanceId)
        .Include(r => r.Address);

        var cityId = await _unitOfWork.DbContext.Set<ShahrbinInstance>()
            .AsNoTracking().Where(s => s.Id == instanceId).
            Select(s => s.CityId).SingleOrDefaultAsync();

        var bins = await _unitOfWork.DbContext.Set<Region>()
            .AsNoTracking()
            .Where(r => r.CityId == cityId)
            .Select(e => new Bin<int>(e.Id, e.Name))
            .ToListAsync();

        //Expression<Func<Report, int>> filter = z => (int)z.Address.RegionId ;

        var result = await GetReportsTimeHistogram<Report, int>(
            "متوسط زمان رسیدگی به تفکیک منطقه",
            bins,
            query,
            z => (int)z.Address.RegionId);

        return result;
    }



    private async Task<InfoModel> GetRepportsTimeByExecutive(int instanceId)
    {
        
        var query = _unitOfWork.DbContext.Set<Report>()
        .AsNoTracking()
        .Where(r => r.ShahrbinInstanceId == instanceId);

        var executives = (await _userRepository.GetUsersInRole(RoleNames.Executive)).Where(u => u.ShahrbinInstanceId == instanceId).ToList();
        var executivesIds = executives.Select(executives => executives.Id);
        var executiveActors = (await _actorRepository.GetAsync(a => executivesIds.Contains(a.Identifier), false)).ToList();

        var bins = new List<Bin<string>>();

        foreach (var actor in executiveActors)
        {
            var user = executives.Where(e => e.Id == actor.Identifier).Single();
            bins.Add(new Bin<string>(actor.Identifier, user.Title));
        }

        //Expression<Func<Report, int>> filter = z => (int)z.Address.RegionId ;
        
        var result = await GetReportsTimeHistogram<Report, string>(
            "متوسط زمان رسیدگی به تفکیک واحد اجرایی",
            bins,
            query,
            z => z.ExecutiveId);
        
        if(result == null)
            result = new InfoModel();

        return result;
    }




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

        foreach (var bin in bins)
        {
            var serie = new InfoSerie(bin.Title, "");

            var duration = groupedQuery
                .Where(g => EqualityComparer<Key>.Default.Equals(g.Id, bin.Id))
                .Select(g => g.Duration)
                .SingleOrDefault();

            var responseDuration = groupedQuery
                .Where(g => EqualityComparer<Key>.Default.Equals(g.Id, bin.Id))
                .Select(g => g.ResponseDuration)
                .SingleOrDefault();

            duration = duration ??= 0;
            var durationTimeSpan = new TimeSpan(0, 0, (int)duration);

            responseDuration = responseDuration ??= 0;
            var responseDurationTimeSpan = new TimeSpan(0, 0, (int)responseDuration);

            serie.Add(new DataItem(
                "متوسط زمان انجام",
                durationTimeSpan.ToHoursValue(),
                durationTimeSpan.ToPersianString()));

            serie.Add(new DataItem(
                "متوسط زمان پاسخ",
                responseDurationTimeSpan.ToHoursValue(),
                responseDurationTimeSpan.ToPersianString()));

            infoChart.Add(serie);
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

    //public record TimeStatus<Key>(Key Id, double? Duration, double? ResponseDuration);

    private string GetPercent(long value, long total)
    {
        var percent = Math.Round(((double)value / (total == 0 ? 1 : total)) * 10000) / 100;
        return $"{percent}% ({value})";
    }

}
