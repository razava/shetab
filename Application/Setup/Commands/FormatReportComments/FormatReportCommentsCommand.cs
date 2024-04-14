namespace Application.Setup.Commands.FormatReportComments;

public record FormatReportCommentsCommand(int instanceId) : IRequest<Result<bool>>;
