namespace Application.Setup.Commands.SpecifyReplyComments;

public record SpecifiyReplyCommand(int instanceId) : IRequest<Result<bool>>;