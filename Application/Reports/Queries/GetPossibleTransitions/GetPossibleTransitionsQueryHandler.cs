using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetPossibleTransitions;

internal sealed class GetPossibleTransitionsQueryHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    IUserRepository userRepository) 
    : IRequestHandler<GetPossibleTransitionsQuery, Result<List<PossibleTransitionResponse>>>
{
    public async Task<Result<List<PossibleTransitionResponse>>> Handle(GetPossibleTransitionsQuery request, CancellationToken cancellationToken)
    {
        var report = await reportRepository.GetByIDAsync(request.reportId);
        if (report == null)
            return NotFoundErrors.Report;
        //if (report.ShahrbinInstanceId != request.instanceId)
        //    return AccessDeniedErrors.General;

        var possibleTransitions = report.GetPossibleTransitions();
        //var pt = await unitOfWork.DbContext.Set<Report>()
        //    .Where(r => r.Id == request.reportId)
        //    .Select(r => r.Process.Transitions
        //        .Where(rpt => r.CurrentStageId == rpt.FromId)
        //        .Select(t => new
        //        {
        //            t.Id,
        //            t.To.Actors,
        //            t.To.DisplayName,
        //            t.ReasonList,
        //            t.CanSendMessageToCitizen,
        //            t.TransitionType
        //        }))
        //    .ToListAsync();

        var result = new List<PossibleTransitionResponse>();
        var regionId = report.Address.RegionId;
        var userId = request.userId;

        foreach (var transition in possibleTransitions)
        {
            var userActorIdentifiers = transition.To.Actors.Where(p => p.Type == ActorType.Person).Select(p => p.Identifier).ToList();
            var roleActorIdentifiers = transition.To.Actors.Where(p => p.Type == ActorType.Role).Select(p => p.Identifier).ToList();
            var userActors = await userRepository.GetUserActors(userActorIdentifiers);
            var roleActors = await userRepository.GetRoleActors(roleActorIdentifiers);

            var t = new PossibleTransitionResponse()
            {
                StageTitle = transition.To.DisplayName,
                ReasonList = transition.ReasonList.ToList(),
                TransitionId = transition.Id,
                CanSendMessageToCitizen = transition.CanSendMessageToCitizen
            };
            var actorList = new List<ActorWithName>();
            foreach (var actor in transition.To.Actors)
            {
                if (actor.Type == ActorType.Auto)
                    continue;
                //TODO: Is this true that if actor has not assigned any reagion should be included?
                if (regionId == null || actor.Regions.Count == 0 ||
                    actor.Regions.Select(p => p.Id).ToList().Contains(regionId.Value))
                {
                    actorList.Add(new ActorWithName()
                    {
                        Id = actor.Id,
                        Identifier = actor.Identifier,
                        Type = actor.Type,
                        FirstName = (actor.Type == ActorType.Role) ? roleActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault()!:
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.FirstName).FirstOrDefault()!,
                        LastName = (actor.Type == ActorType.Role) ? "" :
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.LastName).FirstOrDefault()!,
                        Title = (actor.Type == ActorType.Role) ? roleActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault()! :
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault()!,
                    });
                }
            }

            //string contractorIdentifier = null;

            //Add persons in each role actor
            List<ActorWithName> finalActors = new List<ActorWithName>();
            for (var i = 0; i < actorList.Count; i++)
            {
                if (actorList[i].Type == ActorType.Role)
                {
                    var role = (await userRepository.GetRoles()).Find(p => p.Id == actorList[i].Identifier);
                    if (role is null)
                        throw new ServerNotFoundException("خطایی رخ داد.", new NullActorRolesException());

                    if (role.Name == "Citizen")
                    {
                        finalActors.Add(actorList[i]);
                        continue;
                    }
                    if (role.Name == "Operator")
                    {
                        finalActors.Add(actorList[i]);
                        continue;
                    }

                    List<ApplicationUser> usersInRole = new();
                    if (role.Name == "Contractor")
                    {
                        ////contractorIdentifier = role.Id;
                        //usersInRole = await _context.Users.Where(p => p.Executeves.Any(q => q.Id == user.Id)).ToListAsync();
                        //var executive = (await userRepository.GetUsersInRole("Executive")).Where(p => p.Id == userId).SingleOrDefault();
                        var executive = await userRepository.GetSingleAsync(u => u.Id == userId, false, "Contractors");
                        if (executive != null)
                        {
                            usersInRole.AddRange(executive.Contractors.ToList());
                        }
                        else
                        {
                            usersInRole.AddRange((await userRepository.GetUsersInRole("Contractor")).ToList());
                        }
                    }
                    else
                    {
                        finalActors.Add(actorList[i]);
                        //usersInRole = (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync(role.Name);
                        usersInRole = await userRepository.GetUsersInRole(role.Name!);
                    }

                    var actorsForUsersInRole = (await userRepository.GetActors())
                        .Where(p => usersInRole.Select(a => a.Id).ToList().Contains(p.Identifier))
                        .ToList();

                    //actorList[i].Actors = new List<ActorDto>();
                    foreach (var actor in actorsForUsersInRole)
                    {
                        finalActors.Add(new ActorWithName()
                        {
                            Id = actor.Id,
                            Identifier = actor.Identifier,
                            Type = actor.Type,
                            FirstName = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.FirstName).FirstOrDefault()!,
                            LastName = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.LastName).FirstOrDefault()!,
                            Organization = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.Organization).FirstOrDefault()!,
                            PhoneNumber = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.PhoneNumber).FirstOrDefault()!,
                        });
                    }
                }
                else
                {
                    finalActors.Add(actorList[i]);
                }
            }

            ////FIX IT!
            ////Remove contractor role
            //if (contractorIdentifier != null)
            //{
            //    var con = actorList.Find(p => p.Identifier == contractorIdentifier);
            //    actorList.Remove(con);
            //}

            t.Actors = finalActors;
            result.Add(t);
        }

        return result;
    }
}
