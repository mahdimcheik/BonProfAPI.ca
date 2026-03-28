using BonProfCa.Models;
using BonProfCa.Models;
using BonProfCa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BonProfCa.Controllers;

/// <summary>
/// Contr�leur pour la gestion des cr�neaux
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class SlotsController(SlotsService slotsService, ConversationService conversationService) : ControllerBase
{
    [Authorize(Roles = "Teacher")]
    [HttpPost("teacher/add")]
    public async Task<ActionResult<Response<SlotDetails>>> AddSlotByTeacher(
        [FromBody] SlotCreate slotDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new Response<object>
                {
                    Status = 400,
                    Message = "Donn�es de validation invalides",
                    Data = ModelState,
                }
            );
        }

        var response = await slotsService.AddSlotByTeacherAsync(slotDto, User);

        return StatusCode(response.Status, response);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPut("teacher/update")]
    public async Task<ActionResult<Response<SlotDetails>>> UpdateSlotByTeacher(
        [FromBody] SlotUpdate slotDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new Response<object>
                {
                    Status = 400,
                    Message = "Donn�es de validation invalides",
                    Data = ModelState,
                }
            );
        }

        var response = await slotsService.UpdateSlotByTeacherAsync(slotDto, User);

        return StatusCode(response.Status, response);
    }

    [Authorize(Roles = "Teacher")]
    [HttpDelete("teacher/remove/{slotId:guid}")]
    public async Task<ActionResult<Response<bool>>> RemoveSlotByTeacher([FromRoute] Guid slotId)
    {
        var response = await slotsService.RemoveSlotByTeacherAsync(slotId, User);
        return StatusCode(response.Status, response);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost("teacher/my-slots")]
    public async Task<ActionResult<Response<List<SlotDetails>>>> GetSlotsByTeacher(
        [FromBody] PeriodTime periodTime
    )
    {
        var response = await slotsService.GetSlotsByTeacherAndDatesAsync(
            User,
            periodTime.DateFrom,
            periodTime.DateTo
        );

        return StatusCode(response.Status, response);
    }

    [AllowAnonymous]
    [HttpPost("teacher/{teacherId:guid}/available-slots")]
    public async Task<ActionResult<Response<List<SlotDetails>>>> GetAvailableSlotsByTeacher(
        [FromRoute] Guid teacherId,
        [FromBody] PeriodTime periodTime
    )
    {
        var response = await slotsService.GetSlotsByStudentWithTeacherAsync(
            teacherId,
            periodTime.DateFrom,
            periodTime.DateTo,
            User
        );

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("student")]
    public async Task<ActionResult<Response<List<SlotDetails>>>> GetReservationsByStudent(
        [FromBody] PeriodTime periodTime
    )
    {
        var response = await slotsService.GetReservationByStudentAsync(
            periodTime.DateFrom,
            periodTime.DateTo,
            User
        );

        return StatusCode(response.Status, response);
    }

    [Authorize(Roles = "Student")]
    [HttpPost("student/reservations")]
    public async Task<ActionResult<Response<List<ReservationDetails>>>> GetReservationsByStudentPaginated(
        [FromBody] CustomTableState query
    )
    {
        var response = await slotsService.GetReservationByStudentPaginated(query, User);

        return StatusCode(response.Status, response);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost("teacher/reservations")]
    public async Task<ActionResult<Response<List<ReservationDetails>>>> GetReservationsByTeacherPaginated(
    [FromBody] CustomTableState query
)
    {
        var response = await slotsService.GetReservationByTeacherPaginated(query, User);

        return StatusCode(response.Status, response);
    }

    [HttpPost("student/book")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<Response<List<SlotDetails>>>> BookSlotByStudent(
        [FromBody] ReservationCreate reservation
    )
    {
        var response = await slotsService.BookSlotAsync(reservation, User);
        return StatusCode(response.Status, response);
    }

    [HttpPost("teacher/confirm-reservation")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<Response<object>>> ConfirmBookingByTeacher(
        [FromBody] ReservationUpdateStatus updateStatus
    )
    {
        try
        {
            await slotsService.UpdateReservationStatusAsync(updateStatus);
            return StatusCode(
                200,
                new Response<object>
                {
                    Status = 200,
                    Message = "Booking status updated successfully",
                    Data = null,
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new Response<object>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                }
            );
        }
    }
    
    [HttpGet("reservation/{reservationId:guid}")]
    [Authorize]
    public async Task<ActionResult<Response<ReservationDetails>>> GetReservationById(Guid reservationId)
    {
        var response = await slotsService.GetReservationById(reservationId);
        return StatusCode(response.Status, response);
    }
    
    [HttpGet("conversation/{reservationId:Guid}")]
    [Authorize]
    public async Task<ActionResult<Response<List<ConversationDetails>>>> GetConversationByReservationId(Guid reservationId)
    {
        var response = await conversationService.GetConversationByReservationIdAsync(reservationId);
        return StatusCode(response.Status, response);
    }

    [HttpDelete("teacher/remove-reservation")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<Response<object>>> RemoveReservation(Guid reservationId)
    {
        try
        {
            await slotsService.RemoveReservationByTeacherAsync(reservationId, User);
            return StatusCode(
                200,
                new Response<object>
                {
                    Status = 200,
                    Message = "Reservation removed successfully",
                    Data = null,
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new Response<object>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                }
            );
        }
    }

    [HttpDelete("student/remove-reservation")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<Response<object>>> RemoveReservationByStudent(Guid reservationId)
    {
        try
        {
            await slotsService.RemoveReservationByStudentAsync(reservationId, User);
            return StatusCode(
                200,
                new Response<object>
                {
                    Status = 200,
                    Message = "Reservation removed successfully",
                    Data = null,
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new Response<object>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                }
            );
        }
    }
}
