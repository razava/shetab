using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Commands.CreateContractor;

public record CreateContractorCommand(
    string ExecutiveId,
    List<string> UserRoles,
    string PhoneNumber,
    string FirstName = "",
    string LastName = "",
    string Title = "",
    string Organization = ""):IRequest<Result<ApplicationUser>>;
