using Application.Reports.Common;

namespace Application.Reports.Commands.CreateReportByCitizen;

public sealed record CreateReportByCitizenCommand(
    int InstanceId,
    string CitizenId,
    string PhoneNumber,
    int CategoryId,
    string Comments,
    AddressInfoRequest Address,
    List<Guid> Attachments,
    bool IsIdentityVisible = true,
    bool IsPublic = true) : IRequest<Result<GetReportByIdResponse>>;

