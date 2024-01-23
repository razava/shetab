using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public record GetUsersQuery(PagingInfo PagingInfo, int? InstanceId = null) : IRequest<Result<PagedList<ApplicationUser>>>;