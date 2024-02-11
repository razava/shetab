using Application.Reports.Common;
using Domain.Models.Relational;

namespace Application.Reports.Commands.CreateReportByOperator;

public sealed record CreateReportByOperatorCommand(
    int InstanceId,
    string OperatorId,
    string PhoneNumber,
    string FirstName,
    string LastName,
    int CategoryId,
    string Comments,
    AddressInfoRequest Address,
    List<Guid> Attachments,
    bool IsIdentityVisible = true,
    bool IsPublic = true) : IRequest<Result<GetReportByIdResponse>>;

