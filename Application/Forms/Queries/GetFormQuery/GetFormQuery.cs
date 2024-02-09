using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Queries.GetFormQuery;

public record GetFormQuery(int InstanceId) 
    : IRequest<Result<List<Form>>>;
