using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Utilities;
using BonProfCa.Models;
using BonProfCa.Models.Files;
using BonProfCa.Models.Filters;
using BonProfCa.Services;

namespace BonProfCa.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class TeachersController : ControllerBase
{
    private readonly TeacherService _teacherProfileService;

    public TeachersController(TeacherService teacherProfileService)
    {
        _teacherProfileService = teacherProfileService;
    }
    [HttpPost("all")]
    public async Task<ActionResult<Response<List<UserDetails>>>> GetAllTeacherProfiles([FromBody]FilterTeacher filters)
    {
        var response = await _teacherProfileService.GetAllTeacherProfilesAsync(filters);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("my-profile")]
    [Authorize]
    public async Task<ActionResult<Response<UserDetails>>> GetMyTeacherProfile()
    {
        var response = await _teacherProfileService.GetTeacherFullProfileAsync( User);
        return StatusCode(response.Status, response);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<Response<UserDetails>>> GetTeacherProfileByUserId(
        [FromRoute] Guid userId)
    {
        var response = await _teacherProfileService.GetTeacherProfileByUserIdAsync(userId);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpPut("update-profile")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<Response<UserDetails>>> UpdateTeacherProfile(
        [FromBody] UserUpdate teacherUpdateDto)
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

        var response = await _teacherProfileService.UpdateTeacherProfileAsync(teacherUpdateDto, User);

        return StatusCode(response.Status, response);
    }

    [HttpGet("get-documents")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<Response<List<PrivacyDocumentDetails>>>> GetPrivacyDocuments()
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

        var response = await _teacherProfileService.GetDocumentsByTeacher( User);

        return StatusCode(response.Status, response);
    }

    [HttpPost("upload-document")]
    [Authorize(Roles = "Teacher")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<Response<PrivacyDocumentDetails>>> UploadDocument(
        [FromForm] UploadPrivacyDocument dto)
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

        var response = await _teacherProfileService.UploadPrivacyDocumentAsync(dto, User);

        return StatusCode(response.Status, response);
    }

    [HttpGet("document-types")]
    public async Task<ActionResult<Response<List<PrivacyDocumentTypeDetails>>>> GetPrivacyTypes(
    )
    {        
        var response = await _teacherProfileService.PrivacyDocumentTypesAsync();
        return StatusCode(response.Status, response);
    }
}
