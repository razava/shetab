namespace Api.Contracts;

public record CreateFeedbackDto(string token, int rating);


public record UpdateCommentDto(Guid ReportId/*??*/, string Comment, bool? IsSeen, bool? IsVerified);


