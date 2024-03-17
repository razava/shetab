namespace Application.Polls.Common;

public record PollChoiceRequest(string ShortTitle, string Text, int Order);
