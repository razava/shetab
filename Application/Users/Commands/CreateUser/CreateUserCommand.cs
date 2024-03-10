using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    int InstanceId,
    string Username,
    string Password,
    List<string>? Roles,
    string FirstName = "",
    string LastName = "",
    string Title = "") :IRequest<Result<ApplicationUser>>;