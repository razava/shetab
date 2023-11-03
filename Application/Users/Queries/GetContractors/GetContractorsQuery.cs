using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public record GetContractorsQuery(string ExecutiveId, PagingInfo PagingInfo):IRequest<PagedList<ApplicationUser>>;