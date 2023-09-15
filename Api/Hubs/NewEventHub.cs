using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public class NewEventHub : Hub<INewEventClient>
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}

public interface INewEventClient
{
    Task Update();
}
