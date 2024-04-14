namespace Application.Setup.Commands.SpecifyReplyComments;

public record SpecifyReplyCommand(int instanceId) : IRequest<Result<bool>>;