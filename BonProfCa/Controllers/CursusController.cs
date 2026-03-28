using BonProfCa.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Services;

namespace BonProfCa.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class CursusController(CursusService cursusService, MainContext context) : ControllerBase
{

    [HttpGet("all")]
    public async Task<ActionResult<Response<List<CursusDetails>>>> GetAllCursus()
    {
        var response = await cursusService.GetAllCursusAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Response<CursusDetails>>> GetCursusById(
        [FromRoute] Guid id)
    {
        var response = await cursusService.GetCursusByIdAsync(id);

        return StatusCode(response.Status, response);
    }

    [HttpGet("teacher/{id:guid}")]
    public async Task<ActionResult<Response<List<CursusDetails>>>> GetCursusByTeacherId(
        [FromRoute] Guid id)
    {
        var response = await cursusService.GetCursusByTeacherIdAsync(id);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("level/{id:guid}")]
    public async Task<ActionResult<Response<List<CursusDetails>>>> GetCursusByLevelId(
        [FromRoute] Guid id)
    {
        var response = await cursusService.GetCursusByLevelIdAsync(id);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpPost("create")]
    public async Task<ActionResult<Response<CursusDetails>>> CreateCursus(
        [FromBody] CursusCreate cursusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                Status = 400,
                Message = "Données de validation invalides",
                Data = ModelState
            });
        }

        var response = await cursusService.CreateCursusAsync(cursusDto, User);

        return StatusCode(response.Status, response);
    }

    [HttpPut]
    public async Task<ActionResult<Response<CursusDetails>>> UpdateCursus(
        [FromBody] CursusUpdate cursusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                Status = 400,
                Message = "Données de validation invalides",
                Data = ModelState
            });
        }

        var response = await cursusService.UpdateCursusAsync(cursusDto, User);

        return StatusCode(response.Status, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Response<object>>> DeleteCursus(
        [FromRoute] Guid id)
    {
        var response = await cursusService.DeleteCursusAsync(id);

        return StatusCode(response.Status, response);
    }   
}
