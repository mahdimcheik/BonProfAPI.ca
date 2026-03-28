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
public class TypeSlotsController(TypeSlotsService typeSlotsService) : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType(typeof(Response<List<TypeSlotDetails>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<List<TypeSlotDetails>>>> GetAllTypeSlots()
    {
        var response = await typeSlotsService.GetAllTypeSlotsAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Response<TypeSlotDetails>>> GetTypeSlotById(
        [FromRoute] Guid id)
    {
        var response = await typeSlotsService.GetTypeSlotByIdAsync(id);

        return StatusCode(response.Status, response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<TypeSlotDetails>>> CreateTypeSlot(
        [FromBody] TypeSlotCreate typeSlotDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                Status = 400,
                Message = "Donn�es de validation invalides",
                Data = ModelState
            });
        }

        var response = await typeSlotsService.CreateTypeSlotAsync(typeSlotDto);

        return StatusCode(response.Status, response);
    }

    [HttpPut]
    public async Task<ActionResult<Response<TypeSlotDetails>>> UpdateTypeSlot(
        [FromBody] TypeSlotUpdate typeSlotDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                Status = 400,
                Message = "Donn�es de validation invalides",
                Data = ModelState
            });
        }

        var response = await typeSlotsService.UpdateTypeSlotAsync( typeSlotDto);

        return StatusCode(response.Status, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Response<bool>>> DeleteTypeSlot(
        [FromRoute] Guid id)
    {
        var response = await typeSlotsService.DeleteTypeSlotAsync(id);

        return StatusCode(response.Status, response);
    }
}
