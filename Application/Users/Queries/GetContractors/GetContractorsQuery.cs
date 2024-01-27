using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetContractors;

public record GetContractorsQuery(
    string ExecutiveId,
    PagingInfo PagingInfo) : IRequest<Result<PagedList<ApplicationUser>>>;