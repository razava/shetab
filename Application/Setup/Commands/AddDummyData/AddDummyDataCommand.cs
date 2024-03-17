namespace Application.Setup.Commands.AddDummyDataCommand;

public record AddDummyDataCommand(int Count) : IRequest<Result<bool>>;
