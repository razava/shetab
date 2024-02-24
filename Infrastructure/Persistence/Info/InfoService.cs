using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Info.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Info;

public class InfoService(IUnitOfWork unitOfWork) : IInfoService
{
    public async Task<InfoModel> GetReportsStatusPerCategory(int instanceId, int? parentCategoryId)
    {
        var result = new InfoModel();

        var infoChart = new InfoChart("وضعیت درخواست ها به تفکیک دسته بندی", "", false, false);

        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId);

        var groupedQuery = await query
            .GroupBy(q => new { q.CategoryId, q.ReportState, q.IsFeedbacked, q.IsObjectioned })
        .Select(q => new { Key = q.Key, Count = q.LongCount() })
            .ToListAsync();

        var categories = await unitOfWork.DbContext.Set<Category>()
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
            var catDescendant = category.Decendants;

            var finished = groupedQuery.Where(g => catDescendant.Contains(g.Key.CategoryId) &&
            (g.Key.ReportState == ReportState.Finished || g.Key.ReportState == ReportState.AcceptedByCitizen))
                .Sum(g => g.Count);

            var live = groupedQuery.Where(g => catDescendant.Contains(g.Key.CategoryId) &&
            (g.Key.ReportState == ReportState.Live || g.Key.ReportState == ReportState.Review))
                .Sum(g => g.Count);

            var needAcceptance = groupedQuery.Where(g => catDescendant.Contains(g.Key.CategoryId) &&
            g.Key.ReportState == ReportState.NeedAcceptance)
                .Sum(g => g.Count);

            var total = finished + live + needAcceptance;

            if (total == 0)
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

    private string GetPercent(long value, long total)
    {
        var percent = Math.Round(((double)value / (total == 0 ? 1 : total)) * 10000) / 100;
        return $"{percent}% ({value})";
    }
}
