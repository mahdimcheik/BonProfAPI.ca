using System.Security.Claims;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Services;
using BonProfCa.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BonProfCa.Controllers;

[Authorize]
public class ChatHub : Hub
{
    private readonly ConnectionManager _connectionManager;
    private readonly MainContext _context;
    private readonly ConversationService _conversationService;

    public ChatHub(
        ConnectionManager connectionManager,
        MainContext context,
        ConversationService conversationService
    )
    {
        _connectionManager = connectionManager;
        _context = context;
        _conversationService = conversationService;
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            var connectionId = Context.ConnectionId;
            var userName =
                Context.User?.FindFirst("Email")?.Value
                ?? Context.User?.FindFirst("Name")?.Value
                ?? Context.User?.Identity?.Name
                ?? "Anonymous";

            _connectionManager.AddConnection(connectionId, userName);
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            // Log error if needed
            Console.WriteLine($"Error in OnConnectedAsync: {ex.Message}");
            throw;
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var connectionId = Context.ConnectionId;
            _connectionManager.RemoveConnection(connectionId);
        }
        catch (Exception ex)
        {
            // Log error if needed
            Console.WriteLine($"Error in OnDisconnectedAsync: {ex.Message}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    public Task<int> Ping()
    {
        return Task.FromResult(1);
    }

    public async Task SendMessageByUserEmail(string email, string type, object message)
    {
        try
        {
            var userConnections = _connectionManager
                .GetAllConnections()
                .Where(kvp => kvp.Value == email)
                .Select(kvp => kvp.Key)
                .ToList();

            if (userConnections.Count > 0)
            {
                // Send message to all connections for this user
                await Clients.Clients(userConnections).SendAsync(type, message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message to user {email}: {ex.Message}");
        }
    }

    public async Task SendMessageToAll(string type, object messageDTO)
    {
        await Clients.All.SendAsync(type, messageDTO);
    }

    public async Task SendMessageToGroup(string groupName, string type, object messageDTO)
    {
        await Clients.Groups(groupName).SendAsync(type, messageDTO);
    }

    // add or remove to group
    public Task AddToGroup(string roomName)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public Task RemoveFromGroup(string roomName)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }

    // chat
    public async Task<ConversationCreate?> SendChatByUserEmail(string email, ConversationCreate message)
    {
        try
        {
            var userConnections = _connectionManager
                .GetAllConnections()
                .Where(kvp => kvp.Value == email)
                .Select(kvp => kvp.Key)
                .ToList();
            var senderEmail = Context.User
                .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)
                ?.Value;

            if (senderEmail is null)
            {
                return null;
            }

            if (message is not null && message is ConversationCreate messageCreate)
            {
                await _conversationService.SendMessageAsync(messageCreate);
            }

            if (userConnections.Count > 0)
            {
                // Send message to all connections for this user
                await Clients
                    .Clients(userConnections)
                    .SendAsync(nameof(MessageTypeEnum.Chat), message);
            }
            
            userConnections = _connectionManager
                .GetAllConnections()
                .Where(kvp => kvp.Value == senderEmail)
                .Select(kvp => kvp.Key)
                .ToList();
            
            if (userConnections.Count > 0)
            {
                // Send confirmation to sender
                await Clients
                    .Clients(userConnections)
                    .SendAsync(nameof(MessageTypeEnum.Chat), message);
            }
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message to user {email}: {ex.Message}");
            return message;
        }
    }
}
