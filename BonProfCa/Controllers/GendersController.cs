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
public class GendersController(GendersService gendersService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<Response<List<GenderDetails>>>> GetAllGenders()
    {
        var response = await gendersService.GetAllGendersAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }
}
