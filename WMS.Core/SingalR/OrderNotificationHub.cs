using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WMS.Core.SingalR.Abstractions;

namespace WMS.Core.SingalR;

public class OrderNotificationHub : Hub<IOrderNotificationHub>
{
    private ILogger<OrderNotificationHub> _logger;

    public OrderNotificationHub(ILogger<OrderNotificationHub> logger)
    {
        _logger = logger;
    }

    public async Task AddToGroup(string groupName)
    {
        _logger.LogInformation($"AddToGroup Invoked - ContextId: {Context.ConnectionId}, Group: {groupName}");
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Connected! ConnectionId: {0}", Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Disconnected! ConnectionId: {0}", Context.ConnectionId);
    }
}