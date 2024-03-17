namespace Application.Authentication.Common;

public record ChangePhoneNumberQueryResponse(
    string PhoneNumber,
    string Token,
    string NewPhoneNumber,
    string NewToken);