using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Models;
using BonProfCa.Services;
using Microsoft.AspNetCore.Authorization;

namespace BonProfCa.Controllers;

/// <summary>
/// Contrôleur pour la gestion des formations
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class FormationsController(FormationsService formationsService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<Response<List<FormationDetails>>>> GetAllFormations()
    {
        var response = await formationsService.GetAllFormationsAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Response<FormationDetails>>> GetFormationById(
        [FromRoute] Guid id)
    {
        var response = await formationsService.GetFormationByIdAsync(id);

        return StatusCode(response.Status, response);
    }

    [HttpGet("teacher/{id:guid}")]
    public async Task<ActionResult<Response<List<FormationDetails>>>> GetFormationsByUserId(
        [FromRoute] Guid id)
    {
        var response = await formationsService.GetFormationsByUserIdAsync(id);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Response<FormationDetails>>> CreateFormation(
        [FromBody] FormationCreate formationDto)
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

        var response = await formationsService.CreateFormationAsync(formationDto, User);

        return StatusCode(response.Status, response);
    }
    
    [HttpPut]
    public async Task<ActionResult<Response<FormationDetails>>> UpdateFormation(
        [FromBody] FormationUpdate formationDto)
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

        var response = await formationsService.UpdateFormationAsync( formationDto, User);

        return StatusCode(response.Status, response);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Response<bool>>> DeleteFormation(
        [FromRoute] Guid id)
    {
        var response = await formationsService.DeleteFormationAsync(id);

        return StatusCode(response.Status, response);
    }
}
