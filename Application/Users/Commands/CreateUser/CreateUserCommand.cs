using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    int InstanceId,
    string Username,
    string Password,
    List<string>? Roles,
    List<int>? Regions,
    string FirstName = "",
    string LastName = "",
    string Title = "") :IRequest<Result<ApplicationUser>>;