using BonProfCa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Models.Developpement;
using BonProfCa.Services;
using BonProfCa.Utilities;

namespace BonProfCa.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[Authorize]
[ApiController]
public class AuthController : ControllerBase
{
    #region Attributes

    private readonly MainContext _context;
    private readonly UserManager<UserApp> _userManager;
    private readonly AuthService authService;

    public AuthController(
        MainContext context,
        UserManager<UserApp> userManager,
        AuthService authService
    )
    {
        this._context = context;
        this._userManager = userManager;
        this.authService = authService;
    }

    #endregion

    #region Register update upload
    [AllowAnonymous]
    [EnableCors]
    [HttpPost("register")]
    public async Task<ActionResult<Response<UserDetails>>> Register(
        [FromBody] UserCreate model
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new Response<object> { Status = 404, Message = "Problème de validation" }
            );
        }

        var response = await authService.Register(model);

        if (response.Status == 200 || response.Status == 201)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    #endregion

    #region POST Login
    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    public async Task<ActionResult<Response<Login>>> Login(
        [FromBody] UserLogin model
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(
                new Response<object>
                {
                    Message = "Connexion échouée",
                    Status = 401,
                    Data = ModelState,
                }
            );
        var result = await authService.Login(model, HttpContext.Response);

        if (result.Status == 200 || result.Status == 201)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    #endregion

    #region Confirm account
    [AllowAnonymous]
    [Route("email-confirmation")]
    [HttpGet]
    public async Task<ActionResult<Response<string>>> EmailConfirmation(
        [FromQuery] string userId,
        [FromQuery] string confirmationToken
    )
    {
        var result = await authService.EmailConfirmation(userId, confirmationToken);

        if (result.Status == 200 || result.Status == 201)
        {
            return Redirect(result.Message);
        }
        return BadRequest(result);
    }

    #endregion

    #region CurrentUser informations
    [HttpGet("my-informations")]
    public async Task<ActionResult<Response<UserInfosWithtoken>>> GetMyInformations()
    {
        var user = CheckUser.GetUserFromClaim(HttpContext.User, _context);

        if (user == null)
            return BadRequest(
                new Response<UserInfosWithtoken>
                {
                    Message = "Vous n'êtes pas connecté",
                    Status = 401,
                }
            );

        var userRoles = await _userManager.GetRolesAsync(user);
        var roles = _context.Roles.ToList();

        var rolesDetailed = roles
            .Where(r => userRoles.Contains(r.Name ?? string.Empty))
            .Select(r => new RoleDetails(r))
            .ToList();

        return Ok(
            new Response<UserInfosWithtoken>
            {
                Message = "Demande acceptée",
                Status = 200,
                Data = new UserInfosWithtoken
                {
                    Token = await authService.GenerateAccessTokenAsync(user),
                    User = new UserDetails(user, rolesDetailed),
                },
            }
        );
    }
    #endregion

    #region POST AskForPasswordRecoveryMail
    [AllowAnonymous]
    [Route("forgot-password")]
    [HttpPost]
    public async Task<ActionResult<Response<PasswordReset?>>> ForgotPassword(
        [FromBody] ForgotPassword model
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new Response<PasswordReset?>
                {
                    Message = "Demande refusée",
                    Status = 400,
                }
            );
        }

        var result = await authService.ForgotPassword(model);

        if (result.Status == 200 || result.Status == 201)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    #endregion

    #region PasswordChange after recovery
    [AllowAnonymous]
    [Route("reset-password")]
    [HttpPost]
    public async Task<ActionResult<Response<string?>>> ChangePassword(
        [FromBody] PasswordRecovery model
    )
    {
        if (!ModelState.IsValid || model.Password != model.PasswordConfirmation)
        {
            return BadRequest(
                new Response<string?> { Message = "Demande refusée", Status = 400 }
            );
        }

        var result = await authService.ChangePassword(model);

        if (result.Status == 200 || result.Status == 201)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    #endregion

    #region refresh token
    [Route("refresh-token")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<Response<Login?>>> UpdateRefreshToken()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized(
                new Response<Login?>
                {
                    Message = "Refresh token non-existant",
                    Status = 401,
                }
            );
        }

        var result = await authService.UpdateRefreshToken(refreshToken, HttpContext);

        if (result.Status == 200 || result.Status == 201)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    #endregion

    #region logout
    [AllowAnonymous]
    [HttpGet("logout")]
    public async Task<ActionResult<Response<object?>>> Logout()
    {
        // Récupération de l'email/nom d'utilisateur actuel pour nettoyer les connexions
        var userEmail =
            HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
            ?? HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value
            ?? HttpContext.User?.Identity?.Name;

        Response.Cookies.Delete("refreshToken");

        return Ok(new Response<object> { Message = "Vous êtes déconnecté", Status = 200 });
    }
    #endregion

    #region avatar
    [HttpPost("upload-avatar")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [AllowAnonymous]
    public async Task<ActionResult<Response<FileUrl>?>> OnPostUploadAsync(IFormFile file)
    {
        var result = await authService.UploadAvatar(
            file,
            HttpContext.User,
            HttpContext.Request
        );

        if (result.Status == 200 || result.Status == 201)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    #endregion

    #region developpement
    [HttpPost("dev" )]
    public IActionResult Developpment(GenApi gen){
        return Ok();
    }
    #endregion
}
