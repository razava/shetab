using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Username,
    string Password,
    string FirstName = "",
    string LastName = "",
    string Title = "") :IRequest<Result<ApplicationUser>>;