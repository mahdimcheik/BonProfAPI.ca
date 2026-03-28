using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Models;
using BonProfCa.Services;

namespace BonProfCa.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationsController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<ActionResult<Response<List<NotificationDetails>>>> GetNotifications(
        [FromBody] FilterNotification filter)
    {
        var response = await _notificationService.GetNotificationsByUserAsync(filter, User);
        return StatusCode(response.Status, response);
    }

    [HttpPut("{id:guid}/toggle-seen")]
    public async Task<ActionResult<Response<NotificationDetails>>> ToggleSeen(
        [FromRoute] Guid id)
    {
        var response = await _notificationService.ToggleSeenAsync(id, User);
        return StatusCode(response.Status, response);
    }

    [HttpPut("all-toggle-seen")]
    public async Task<ActionResult<Response<NotificationDetails>>> AllToggleSeen(
    )
    {
        var response = await _notificationService.ToggleAllSeenAsync(User);
        return StatusCode(response.Status, response);
    }
}
