namespace Application.Setup.Commands.AddGoldenUser;

public record AddGoldenUserCommand(int InstanceId, string UserName, string Password) : IRequest<Result<bool>>;
