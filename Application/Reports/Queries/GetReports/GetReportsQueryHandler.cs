using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Reports.Queries.GetPossibleTransitions;

internal sealed class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, List<Report>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProcessRepository _processRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetReportsQueryHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, IUserRepository userRepository, IProcessRepository processRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _userRepository = userRepository;
        _processRepository = processRepository;
    }

    public async Task<List<Report>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var actors = await _userRepository.GetActors(request.userId);
        var actorIds = actors.Select(a => a.Id).ToList();
        var reports = await _reportRepository.GetAsync(r => r.CurrentActors.Any(ca => actorIds.Contains(ca.Id)));

        return reports.ToList();
    }
}
