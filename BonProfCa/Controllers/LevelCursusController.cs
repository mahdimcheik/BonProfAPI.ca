using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Models;
using BonProfCa.Services;

namespace BonProfCa.Controllers;

/// <summary>
/// Contrôleur pour la gestion des niveaux de cursus
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class LevelCursusController(LevelCursusService levelCursusService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<Response<List<LevelCursusDetails>>>> GetAllLevelCursus()
    {
        var response = await levelCursusService.GetAllLevelCursusAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Response<LevelCursusDetails>>> GetLevelCursusById(
        [FromRoute] Guid id)
    {
        var response = await levelCursusService.GetLevelCursusByIdAsync(id);

        return StatusCode(response.Status, response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<LevelCursusDetails>>> CreateLevelCursus(
        [FromBody] LevelCursusCreate levelDto)
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

        var response = await levelCursusService.CreateLevelCursusAsync(levelDto);

        return StatusCode(response.Status, response);
    }

    [HttpPut]
    public async Task<ActionResult<Response<LevelCursusDetails>>> UpdateLevelCursus( [FromBody] LevelCursusUpdate levelDto)
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

        var response = await levelCursusService.UpdateLevelCursusAsync( levelDto);

        return StatusCode(response.Status, response);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Response<object>>> DeleteLevelCursus(
        [FromRoute] Guid id)
    {
        var response = await levelCursusService.DeleteLevelCursusAsync(id);

        return StatusCode(response.Status, response);
    }
}
