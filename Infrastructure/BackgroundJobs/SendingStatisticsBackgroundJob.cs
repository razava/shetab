using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class SendingStatisticsBackgroundJob(
    IUnitOfWork unitOfWork,
    ICommunicationService communicationService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var waitedTasksCount = await getStatistics();
        foreach (var item in waitedTasksCount)
        {
            var message = $"{item.Title} محترم، {item.Count} درخواست در سامانه شهربین در کارتابل شما وجود دارد.";
            if (item.PhoneNumber != null && item.Count > 0)
                await communicationService.SendAsync(item.PhoneNumber, message);
        }

        await getStatistics();
    }

    private async Task<List<WaitedTasksCount>> getStatistics()
    {
        var context = unitOfWork.DbContext;

        var waitedTasks = await context.Set<Report>()
            .Where(r => r.CurrentActorId != null)
            .GroupBy(r => r.CurrentActorId)
            .Select(gr => new { Id = gr.Key, Identifier = gr.Select(a => a.CurrentActor!.Identifier).First(), Count = gr.Count() })
            .ToListAsync();

        var organizationalUnits = await context.Set<OrganizationalUnit>()
            .Include(o => o.OrganizationalUnits)
            .Include(o => o.User)
            .ToListAsync();

        var result = new List<WaitedTasksCount>();
        foreach(var ou in organizationalUnits)
        {
            var ous = getUserIdsOfOrganizationalUnit(organizationalUnits, ou.Id);
            var totalCount = waitedTasks.Where(wt => ous.Select(o => o.UserId).Contains(wt.Identifier)).Sum(wt => wt.Count);
            result.Add(new WaitedTasksCount(ou.User.PhoneNumber, ou.User.FirstName, ou.User.LastName, ou.User.Title, totalCount));
        }

        return result;
    }

    private record WaitedTasksCount(string? PhoneNumber, string FirstName, string LastName, string Title, int Count);
    private List<OrganizationalUnit> getUserIdsOfOrganizationalUnit(List<OrganizationalUnit> organizationalUnits, int id)
    {
        var organizationalUnit = organizationalUnits.Where(o => o.Id == id).FirstOrDefault();
        var result = new List<OrganizationalUnit>();
        if (organizationalUnit != null)
        {
            var queue = new Queue<OrganizationalUnit>();
            queue.Enqueue(organizationalUnit);

            while (queue.Count > 0)
            {
                var t = queue.Dequeue();
                if (t.Type == OrganizationalUnitType.Executive || t.Type == OrganizationalUnitType.Person)
                {
                    result.Add(t);
                }
                var childOus = organizationalUnits.Where(o => o.Id == t.Id).First().OrganizationalUnits;
                foreach (var child in childOus)
                {
                    queue.Enqueue(child);
                }
            }
        }

        return result;
    }
}
