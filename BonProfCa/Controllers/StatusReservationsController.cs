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
public class StatusReservationsController(StatusReservationService statusReservationService) : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType(typeof(Response<List<StatusReservationDetails>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<List<StatusReservationDetails>>>> GetAllStatusReservations()
    {
        var response = await statusReservationService.GetAllStatusReservationsAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }
}
