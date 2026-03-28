using BonProfCa.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace BonProfCa.Services;

public class SignalRNotificationsService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ConnectionManager _connectionManager;

    public SignalRNotificationsService(
        IHubContext<ChatHub> hubContext,
        ConnectionManager connectionManager)
    {
        this._hubContext = hubContext;
        this._connectionManager = connectionManager;
    }

    public async Task SendMessageByUserEmail(string email, string type, object message)
    {
        try
        {
            var userConnections = _connectionManager.GetAllConnections()
                .Where(kvp => kvp.Value == email)
                .Select(kvp => kvp.Key)
                .ToList();

            if (userConnections.Count > 0)
            {
                await _hubContext.Clients.Clients(userConnections).SendAsync(type, message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message to user {email}: {ex.Message}");
        }
    }

    public async Task SendMessageToAll(string type, object messageDTO)
    {
        await _hubContext.Clients.All.SendAsync(type, messageDTO);
    }

    public async Task SendMessageToGroup(string groupName, string type, object messageDTO)
    {
        await _hubContext.Clients.Groups(groupName).SendAsync(type, messageDTO);
    }

    // add or remove to group
    public Task AddToGroup(string connectionId, string roomName)
    {
        return _hubContext.Groups.AddToGroupAsync(connectionId, roomName);
    }

    public Task RemoveFromGroup(string connectionId, string roomName)
    {
        return _hubContext.Groups.RemoveFromGroupAsync(connectionId, roomName);
    }
}