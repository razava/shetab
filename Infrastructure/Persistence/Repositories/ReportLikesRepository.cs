using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ReportLikesRepository : IReportLikesRepository
{
    private readonly DbContext context;
    public ReportLikesRepository(
        IUnitOfWork unitOfWork)
    {       
        context = unitOfWork.DbContext;
    }

    public async Task<int> Count(Guid reportId)
    {
        var result = await context.Set<ReportLikes>().CountAsync(r => r.ReportId == reportId);
        return result;
    }

    public async Task<bool> Like(string userId, Guid reportId, bool? isLiked = null)
    {
        var alreadyLiked = await context.Set<ReportLikes>()
            .Where(rl => rl.LikedById == userId && rl.ReportId == reportId)
            .SingleOrDefaultAsync();

        var isAlreadyLiked = alreadyLiked is not null;

        var result = (isAlreadyLiked, isLiked) switch
        {
            (true, null) => false,
            (false, null) => true,
            (_, true) => true,
            (_, false) => false
        };

        if(result && !isAlreadyLiked)
        {
            context.Set<ReportLikes>().Add(new ReportLikes { ReportId = reportId, LikedById = userId });
            await context.Set<Report>()
                .Where(r => r.Id == reportId)
                .ExecuteUpdateAsync(r => r.SetProperty(e => e.Likes, e => e.Likes + 1));
            await context.SaveChangesAsync();
            return result;
        }
        if(!result && isAlreadyLiked)
        {
            context.Set<ReportLikes>().Remove(alreadyLiked!);
            await context.Set<Report>()
                .Where(r => r.Id == reportId)
                .ExecuteUpdateAsync(r => r.SetProperty(e => e.Likes, e => e.Likes - 1));
            await context.SaveChangesAsync();
            return result;
        }

        //Never get here
        return true;
    }
}
