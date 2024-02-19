using Application.Common.Interfaces.Persistence;
using Application.Reports.Queries.GetHistory;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Reports.Queries.GetReportById;

internal sealed class GetHistoryQueryHandler(
    IUnitOfWork unitOfWork,
    IActorRepository actorRepository,
    IUserRepository userRepository) : IRequestHandler<GetHistoryQuery, Result<List<HistoryResponse>>>
{
    public async Task<Result<List<HistoryResponse>>> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
    {
        //TODO: check whether user can access to content or not
        var context = unitOfWork.DbContext;
        var result = await context.Set<TransitionLog>().Where(tl => tl.ReportId == request.Id).ToListAsync(cancellationToken);
        
        var response = new List<HistoryResponse>();
        foreach (var transitionLog in result)
        {
            var historyResponse = transitionLog.Adapt<HistoryResponse>();
            response.Add(historyResponse);

            var actor = await actorRepository.GetSingleAsync(a => a.Identifier == transitionLog.ActorIdentifier, false);
            if (actor == null)
                continue; //todo: handle this situation

            var actorResponse = new ActorResponse();
            if (actor.Type == ActorType.Person)
            {
                var actorUser = await userRepository.GetSingleAsync(u => u.Id == actor.Identifier, false);
                if (actorUser == null) return ServerNotFoundErrors.Actor;
                actorResponse.FirstName = actorUser.FirstName;
                actorResponse.LastName = actorUser.LastName;
                actorResponse.Title = actorUser.Title;
                historyResponse.Actor = actorResponse;
            }
            if(actor.Type == ActorType.Role)
            {
                var acorRole = (await userRepository.GetRoleActors(new List<string> { actor.Identifier }))
                    .FirstOrDefault();
                if (acorRole == null) return ServerNotFoundErrors.Actor;
                actorResponse.Title = acorRole.Title;
                historyResponse.Actor = actorResponse;
            }
            if(actor.Type == ActorType.Auto)
            {
                actorResponse.Title = "ارجاع خودکار";
                historyResponse.Actor = actorResponse;
            }
        }

        return response;
    }
}