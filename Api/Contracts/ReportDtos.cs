namespace Api.Contracts;

public record CreateFeedbackDto(
    string token,
    int rating);

public record UpdateCommentDto(
    Guid ReportId,   /*??*/
    string Comment,
    bool? IsSeen,
    bool? IsVerified);

public record PutSatisfactionDto(
    int Rating,
    string Comment);


public record MakeTransitionDto(
    int TransitionId,
    int ReasonId,
    List<Guid> Attachments,
    string Comment,
    List<int> ActorIds)
{
    public List<Guid> Attachments { get; init; } = Attachments ?? new List<Guid>();
    public string Comment { get; init; } = Comment ?? "";
}



