using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.CreateContractor;

public record CreateContractorCommand(
    string ExecutiveId,
    string PhoneNumber,
    string Organization = "",
    string FirstName = "",
    string LastName = "",
    string Title = ""):IRequest<ApplicationUser>;
