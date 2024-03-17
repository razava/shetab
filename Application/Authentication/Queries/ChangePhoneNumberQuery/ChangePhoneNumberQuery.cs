using Application.Authentication.Common;
using Application.Common.Interfaces.Security;

namespace Application.Authentication.Queries.ChangePhoneNumberQuery;

public sealed record ChangePhoneNumberQuery(
    string Username,
    string NewPhoneNumber,
    CaptchaValidateModel? CaptchaValidateModel = null) 
    : IRequest<Result<ChangePhoneNumberQueryResponse>>;
