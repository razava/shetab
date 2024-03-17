using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Setup.Commands.AddDummyDataCommand;

internal class AddDummyDataCommandHandler(
    IUserRepository userRepository,
    IReportRepository reportRepository,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository) : IRequestHandler<AddDummyDataCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddDummyDataCommand request, CancellationToken cancellationToken)
    {
        Random random = new Random();

        var citizen = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => u.UserName == "09133583714")
            .FirstOrDefaultAsync();
        var operatorUser = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => u.UserName == "abk-operator")
            .FirstOrDefaultAsync();
        if (citizen is null || operatorUser is null)
            throw new Exception();

        var regions = await unitOfWork.DbContext.Set<Region>()
            .Where(c => c.CityId == 1223)
            .ToListAsync();
        var addrDto = new AddressInfoRequest(
            regions[random.Next(1, regions.Count) - 1].Id,
            35, 54, "");

        var categoryIds = await unitOfWork.DbContext.Set<Category>()
            .Where(c => c.ShahrbinInstanceId == 1 && c.ProcessId != null)
            .Select(c => c.Id)
            .ToListAsync();

        for(int i = 0; i<request.Count; i++)
        {
            var category = await categoryRepository
                .GetByIDAsync(categoryIds[random.Next(1, categoryIds.Count) - 1]);
            if (category is null) continue;

            var report = Report.NewByCitizen(
            citizen.Id,
            citizen.PhoneNumber!,
            category,
            $"تست شماره {i} در {DateTime.UtcNow}",
            addrDto.GetAddress(),
            new List<Media>(),
            Visibility.EveryOne,
            true);

            if(random.Next(1, 10) > 2)
            {
                report.Accept(operatorUser.Id, null, null, null, null, null, null);
                var iterations = random.Next(0, 5);
                for (int j = 0; j < iterations; j++)
                {
                    if(report.CurrentActorId is null) continue;
                    var currentActor = await getActor(report.CurrentActorId);
                    if (currentActor is null) continue;
                    string currentUserId;
                    if (currentActor.Type == ActorType.Role)
                    {
                        var roleName = await unitOfWork.DbContext.Set<ApplicationRole>()
                            .Where(r => r.Id == currentActor.Identifier)
                            .FirstOrDefaultAsync();
                        var usersInRole = await userRepository.GetUsersInRole(roleName!.Name!);
                        usersInRole = usersInRole.Where(u => u.ShahrbinInstanceId == 1).ToList();
                        if(!usersInRole.Any()) break;
                        currentUserId = usersInRole[random.Next(1, usersInRole.Count)-1].Id;
                    }
                    else
                    {
                        currentUserId = currentActor.Identifier;
                    }
                    var possibleTransitions = report.GetPossibleTransitions();
                    if (possibleTransitions is null) continue;

                    var transition = possibleTransitions[random.Next(1, possibleTransitions.Count) - 1];
                    var reasonId = transition.ReasonList.ToList()[random.Next(1, transition.ReasonList.Count()) - 1].Id;
                    var toActor = transition.To.Actors.ToList()[random.Next(1, transition.To.Actors.Count)-1];

                    report.MakeTransition(
                        transition.Id,
                        reasonId,
                        new List<Media>(),
                        "",
                        ActorType.Person,
                        currentUserId,
                        toActor.Id,
                        await isExecutive(currentUserId),
                        await isContractor(currentUserId));
                }
            }
            reportRepository.Insert(report);
            try
            {
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                int hh = 80;
            }
        }
        
        return Result.Ok(true);
    }

    private async Task<Actor?> getActor(int? id)
    {
        return await unitOfWork.DbContext.Set<Actor>()
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();
    }

    private async Task<bool> isExecutive(string userId)
    {
        return (await userRepository.GetUserRoles(userId)).Contains(RoleNames.Executive);
    }

    private async Task<bool> isContractor(string userId)
    {
        return (await userRepository.GetUserRoles(userId)).Contains(RoleNames.Contractor);
    }
}
