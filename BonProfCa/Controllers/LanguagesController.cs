using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Services;
using BonProfCa.Utilities;

namespace BonProfCa.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("[controller]")]
    [ApiController]
    [EnableCors]
    public class LanguagesController(LanguagesService languagesService, MainContext context) : ControllerBase
    {
        [HttpPost("all")]
        public async Task<ActionResult<Response<List<LanguageDetails>>>> GetAllLanguages()
        {
            var response = await languagesService.GetAllLanguagesAsync();

            if (response.Status == 200)
            {
                return Ok(response);
            }

            return StatusCode(response.Status, response);
        }

        [HttpGet("user")]
        public async Task<ActionResult<Response<List<LanguageDetails>>>> GetLanguagesByUserId()
        {
            var response = await languagesService.GetLanguagesByTeacherIdAsync(User);

            if (response.Status == 200)
            {
                return Ok(response);
            }

            return StatusCode(response.Status, response);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Response<LanguageDetails>>> CreateLanguage(
            [FromBody] LanguageCreate languageDto)
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

            var response = await languagesService.CreateLanguageAsync(languageDto);

            return StatusCode(response.Status, response);
        }
        
        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult<Response<LanguageDetails>>> UpdateLanguage(
            [FromRoute] Guid id,
            [FromBody] LanguageUpdate languageDto)
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

            var response = await languagesService.UpdateLanguageAsync(id, languageDto);

            return StatusCode(response.Status, response);
        }
        
        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult<Response<object>>> DeleteLanguage(
            [FromRoute] Guid id)
        {
            var response = await languagesService.DeleteLanguageAsync(id);

            return StatusCode(response.Status, response);
        }
        
        [HttpPost("teacher/update-languages")]
        public async Task<ActionResult<Response<List<LanguageDetails>>>> UpdateLanguagesForUser(
            [FromBody] Guid[] languagesIds)
        {
            var user = CheckUser.GetUserFromClaim(User, context);

            if (!ModelState.IsValid || user is null)
            {
                return BadRequest(new Response<object>
                {
                    Status = 400,
                    Message = "Données de validation invalides",
                    Data = ModelState
                });
            }
            var teacher = await context.Teachers.FindAsync(user.Id);
            if (teacher is null)
            {
                return BadRequest(new Response<object>
                {
                    Status = 400,
                    Message = "Données de validation invalides",
                    Data = ModelState
                });
            }

            var response = await languagesService.UpdateLanguagesForUser(languagesIds, User);

            return StatusCode(response.Status, response);
        }
    }
}
