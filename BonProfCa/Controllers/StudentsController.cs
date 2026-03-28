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
public class StudentsController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentsController(StudentService studentService)
    {
        _studentService = studentService;
    }


    [HttpGet("my-profile")]
    [Authorize]
    public async Task<ActionResult<Response<UserDetails>>> GetMyProfile()
    {
        var response = await _studentService.GetTStudentProfileAsync( User);
        return StatusCode(response.Status, response);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<Response<UserDetails>>> GetStudentByUserId(
        [FromRoute] Guid userId)
    {
        var response = await _studentService.GetStudentByUserIdAsync(userId);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpPut("update-profile")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<Response<UserDetails>>> UpdateStudent(
        [FromBody] UserUpdate student)
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

        var response = await _studentService.UpdateStudentProfileAsync(student, User);

        return StatusCode(response.Status, response);
    }
}
