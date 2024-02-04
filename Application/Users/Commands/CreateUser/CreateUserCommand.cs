using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    int InstanceId,
    string Username,
    string Password,
    string FirstName = "",
    string LastName = "",
    string Title = "") :IRequest<Result<ApplicationUser>>;