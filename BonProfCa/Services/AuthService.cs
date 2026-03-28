using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Utilities;
using BonProfCa.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BonProfCa.Services;

public class AuthService
{
    private readonly MainContext _context;
    private readonly UserManager<UserApp> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly MailService _mailService;
    private readonly MinioService _minioService;

    public AuthService(
        MainContext context,
        UserManager<UserApp> userManager,
        IWebHostEnvironment env,
        MailService mailService,
        MinioService minioService
    )
    {
        this._context = context;
        this._userManager = userManager;
        this._env = env;
        this._mailService = mailService;
        this._minioService = minioService;
    }

    public async Task<Response<UserDetails>> Register(UserCreate newUserDTO)
    {
        var transaction = _context.Database.BeginTransaction();
        try
        {
            // Vérifier le consentement
            if (!newUserDTO.DataProcessingConsent || !newUserDTO.PrivacyPolicyConsent)
            {
                return new Response<UserDetails>
                {
                    Status = 400,
                    Message =
                        "\"Le consentement est obligatoire pour acceder aux fonctionnalités de cette application\"",
                };
            }

            bool isEmailAlreadyUsed = await IsEmailAlreadyUsedAsync(newUserDTO.Email);
            // Vérifier si l'adresse e-mail est déjà utilisée
            if (isEmailAlreadyUsed)
            {
                // Si l'adresse e-mail est déjà utilisée, mettre à jour la réponse et sauter vers l'étiquette UserAlreadyExisted
                return new Response<UserDetails>
                {
                    Status = 400,
                    Message = "\"L'email est déjà utilisé\"",
                };
            }
            // Créer un nouvel utilisateur en utilisant les données du modèle et la base de données contextuelle
            UserApp? newUser = new UserApp(newUserDTO);
            newUser.CreatedAt = DateTime.Now;

            // Obtenir la date actuelle
            DateTimeOffset date = DateTimeOffset.UtcNow;

            // Tenter de créer un nouvel utilisateur avec le gestionnaire d'utilisateurs
            IdentityResult result = await _userManager.CreateAsync(newUser, newUserDTO.Password);

            // Tenter d'ajouter l'utilisateur aux rôles spécifiés dans le modèle
            IdentityResult roleResult = await _userManager.AddToRolesAsync(
                user: newUser,
                roles: newUserDTO.RoleId == HardCode.ROLE_TEACHER ? ["Teacher"] : ["Student"]
            );

            newUser = await _context
                .Users.Where(u => u.Id == newUser.Id)
                .Include(u => u.Status)
                .Include(x => x.Gender)
                .FirstOrDefaultAsync();

            if (newUser is null)
            {
                await transaction.RollbackAsync();
                return new Response<UserDetails>
                {
                    Message = "Création échouée",
                    Status = 404,
                    Data = null,
                };
            }

            // Vérifier si la création de l'utilisateur a échoué
            if (!result.Succeeded)
            {
                // Si la création a échoué, ajouter les erreurs au modèle d'état pour retourner une réponse BadRequest
                var errors = Enumerable.Empty<string>();
                foreach (var error in result.Errors)
                {
                    errors.Append(error.Description);
                }

                // Retourner une réponse BadRequest avec le modèle d'état contenant les erreurs
                return new Response<UserDetails>
                {
                    Message = "Création échouée",
                    Status = 401,
                    Data = null,
                };
            }

            // Si tout s'est bien déroulé, enregistrer les changements dans le contexte de base de données
            await _context.SaveChangesAsync();

            // creer les profiles
            await CreateProfile(newUser, newUserDTO);
            await transaction.CommitAsync();

            try
            {
                var confirmationLink = await GenerateAccountConfirmationLink(newUser);
                await _mailService.SendConfirmAccount(newUser, confirmationLink ?? "");

                // Retourne une réponse avec le statut déterminé, l'identifiant de l'utilisateur, le message de réponse et le statut complet
                return new Response<UserDetails>
                {
                    Message = "Profil créé",
                    Status = 201,
                    Data = new UserDetails(newUser, null),
                };
            }
            catch (Exception e)
            {
                // En cas d'exception, afficher la trace et retourner une réponse avec le statut approprié
                Console.WriteLine(e);
                return new Response<UserDetails>
                {
                    Status = 200,
                    Message = "Le compte est créé mais  pas d'email de validation!!!",
                };
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new Response<UserDetails>
            {
                Message = "Création échouée",
                Status = 401,
                Data = null,
            };
        }
    }

    private async Task CreateProfile(UserApp newUser, UserCreate userCreate)
    {
        try
        {
            if (userCreate.RoleId == HardCode.ROLE_TEACHER && userCreate.Teacher is not null)
            {
                Teacher newTeacher = new Teacher(userCreate.Teacher, newUser.Id);
                await _context.Teachers.AddAsync(newTeacher);
                await _context.SaveChangesAsync();
            }
            else
            {
                Student newStudent = new Student { Id = newUser.Id, UserId = newUser.Id };
                await _context.Students.AddAsync(newStudent);
                await _context.SaveChangesAsync();
            }
        }
        catch
        {
            throw;
        }
    }

    public async Task<Response<string?>> EmailConfirmation(string userId, string confirmationToken)
    {
        UserApp user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new Response<string?> { Message = "Validation échouée", Status = 400 };
        }

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, confirmationToken);

        if (result.Succeeded)
        {
            return new Response<string?>
            {
                Message = $"{EnvironmentVariables.API_FRONT_URL}/auth/email-confirmation-success",
                Status = 200,
            };
        }

        return new Response<string?> { Message = "Validation échouée", Status = 400 };
    }

    public async Task<Response<Login>> UpdateRefreshToken(
        string refreshToken,
        HttpContext httpContext
    )
    {
        var refreshTokenDB = await _context
            .RefreshTokens.Where(x =>
                x.Token == refreshToken && x.ExpirationDate > DateTimeOffset.UtcNow
            )
            .FirstOrDefaultAsync();

        if (refreshTokenDB is null)
        {
            return new Response<Login> { Message = "Token expiré ou non valide", Status = 401 };
        }

        var user = await _context
            .Users.Where(u => u.Id == refreshTokenDB.UserId)
            .Include(p => p.Gender)
            .Include(u => u.Teacher)
            .Include(u => u.Student)
            .FirstOrDefaultAsync();

        httpContext.Response.Headers.Append(key: "Access-Control-Allow-Credentials", value: "true");

        var userRoles = await _userManager.GetRolesAsync(refreshTokenDB.User);
        var roles = _context.Roles.ToList();

        if (user.ImgUrl is not null)
        {
            var imgUrl = await _minioService.GetFileUrlAsync(user.ImgUrl);
            user.ImgUrl = imgUrl;
        }

        var rolesDetailed = roles
            .Where(r => userRoles.Contains(r.Name ?? string.Empty))
            .Select(r => new RoleDetails(r))
            .ToList();

        return new Response<Login>
        {
            Message = "Autorisation renouvelée",
            Data = new Login
            {
                User = new UserDetails(user!, rolesDetailed),
                Token = await GenerateAccessTokenAsync(refreshTokenDB.User),
                RefreshToken = refreshToken,
            },
            Status = 200,
        };
    }

    public async Task<Response<PasswordReset>> ForgotPassword(ForgotPassword model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            try
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken = HttpUtility.UrlEncode(resetToken);

                var resetLink =
                    EnvironmentVariables.API_FRONT_URL
                    + "/auth/reset-password?userId="
                    + user.Id
                    + "&resetToken="
                    + resetToken;

                // Tentative d'envoi de l'e-mail pour la regénération du mot de passe

                //await mailService.ScheduleSendResetEmail(
                //    new Mail
                //    {
                //        MailSubject = "Mail de réinitialisation",
                //        MailTo = user.Email,
                //    },
                //    resetLink
                //);

                return new Response<PasswordReset>
                {
                    Message =
                        "Un email de réinitialisation vient d'être envoyé à cette adresse "
                        + user.Email,
                    Status = 200,
                    Data = new PasswordReset
                    {
                        ResetToken = resetToken,
                        Email = user.Email,
                        Id = user.Id,
                    },
                };
            }
            catch
            {
                return new Response<PasswordReset>
                {
                    Message = "Erreur de réinitialisation, réessayez plus tard ",
                    Status = 400,
                };
            }
        }

        return new Response<PasswordReset>
        {
            Message = "Erreur de réinitialisation, réessayez plus tard ",
            Status = 400,
        };
    }

    public async Task<Response<string?>> ChangePassword(PasswordRecovery model)
    {
        UserApp? user = await _userManager.FindByIdAsync(model.UserId);
        if (user is null)
        {
            return new Response<string?> { Message = "L'utilisateur n'existe pas", Status = 404 };
        }

        IdentityResult result = await _userManager.ResetPasswordAsync(
            user: user,
            token: model.ResetToken,
            newPassword: model.Password
        );

        var newRefreshToken = await RenewRefreshTokenAsync(user);

        if (result.Succeeded)
        {
            return new Response<string?>
            {
                Message = "Mot de passe vient d'être modifié",
                Status = 201,
            };
        }

        return new Response<string?>
        {
            Message = "Problème de validation, votre token est valid ?",
            Status = 404,
        };
    }

    public async Task<Response<Login>> Login(UserLogin model, HttpResponse response)
    {
        //var user = await userManager.FindByEmailAsync(model.Email);
        var user = await _context
            .Users.Where(u => u.UserName.ToLower() == model.Email)
            .Include(p => p.Gender)
            .Include(u => u.Teacher)
            .Include(u => u.Student)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new Response<Login> { Message = "L'utilisateur n'existe pas ", Status = 404 };
        }

        var result = await _userManager.CheckPasswordAsync(user: user, password: model.Password);
        if (!_userManager.CheckPasswordAsync(user: user, password: model.Password).Result)
        {
            return new Response<Login> { Message = "Connexion échouée", Status = 401 };
        }

        // à la connection, je crée ou je met à jour le refreshtoken
        var refreshToken = await CreateOrUpdateTokenAsync(user, forceReset: true);

        await _context.SaveChangesAsync();
        // to allow cookies sent from the front end
        response.Headers.Append(key: "Access-Control-Allow-Credentials", value: "true");
        var userRoles = await _userManager.GetRolesAsync(user);
        var roles = _context.Roles.ToList();

        var rolesDetailed = roles
            .Where(r => userRoles.Contains(r.Name ?? string.Empty))
            .Select(r => new RoleDetails(r))
            .ToList();

        response.Cookies.Append(
            "refreshToken",
            refreshToken.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(EnvironmentVariables.COOKIES_VALIDITY_DAYS),
            }
        );

        if (user.ImgUrl is not null)
        {
            var imgUrl = await _minioService.GetFileUrlAsync(user.ImgUrl);
            user.ImgUrl = imgUrl;
        }

        return new Response<Login>
        {
            Message = "Connexion réussite",
            Status = 200,
            Data = new Login
            {
                Token = await GenerateAccessTokenAsync(user),
                RefreshToken = refreshToken?.Token,
                User = new UserDetails(user, rolesDetailed),
            },
        };
    }

    private async Task<RefreshToken?> CreateOrUpdateTokenAsync(
        UserApp user,
        bool forceReset = false
    )
    {
        // à la connection, je crée ou je met à jour le refreshtoken
        var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.UserId == user.Id);

        if (refreshToken is null)
        {
            refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ExpirationDate = DateTimeOffset.UtcNow.AddDays(
                    EnvironmentVariables.COOKIES_VALIDITY_DAYS
                ),
            };
            _context.RefreshTokens.Add(refreshToken);
        }
        else if (forceReset)
        {
            refreshToken.UserId = user.Id;
            refreshToken.ExpirationDate = DateTimeOffset.UtcNow.AddDays(
                EnvironmentVariables.COOKIES_VALIDITY_DAYS
            );
        }

        await _context.SaveChangesAsync();

        return refreshToken;
    }

    private async Task<RefreshToken?> RenewRefreshTokenAsync(UserApp user)
    {
        var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.UserId == user.Id);

        if (refreshToken is null)
        {
            _context.RefreshTokens.Add(
                new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    Token = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    ExpirationDate = DateTimeOffset.UtcNow.AddDays(
                        EnvironmentVariables.COOKIES_VALIDITY_DAYS
                    ),
                }
            );
        }
        else
        {
            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.UserId = user.Id;
            refreshToken.ExpirationDate = DateTimeOffset.UtcNow.AddDays(
                EnvironmentVariables.COOKIES_VALIDITY_DAYS
            );
        }

        await _context.SaveChangesAsync();

        return refreshToken;
    }

    /// <summary>
    /// Génère un token d'accès JWT pour l'utilisateur
    /// </summary>
    /// <param name="user">Utilisateur pour lequel générer le token</param>
    /// <returns>Token JWT en string</returns>
    public async Task<string> GenerateAccessTokenAsync(UserApp user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(EnvironmentVariables.JWT_KEY)
        );
        var credentials = new SigningCredentials(
            key: securityKey,
            algorithm: SecurityAlgorithms.HmacSha256
        );

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(type: ClaimTypes.Email, value: user.Email),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(type: ClaimTypes.Role, value: userRole));
        }

        var token = new JwtSecurityToken(
            issuer: EnvironmentVariables.API_BACK_URL,
            audience: EnvironmentVariables.API_BACK_URL,
            claims: authClaims,
            expires: DateTime.Now.AddMinutes(EnvironmentVariables.TOKEN_VALIDITY_MINUTES),
            signingCredentials: credentials
        );

        _context.Entry(user).State = EntityState.Modified;

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string?> GenerateAccountConfirmationLink(UserApp user)
    {
        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        confirmationToken = HttpUtility.UrlEncode(confirmationToken);

        var confirmationLink =
            EnvironmentVariables.API_BACK_URL
            + "/auth/email-confirmation?userId="
            + user.Id
            + "&confirmationToken="
            + confirmationToken;

        return confirmationLink;
    }

    private async Task<bool> IsEmailAlreadyUsedAsync(string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        return existingUser != null;
    }

    public async Task<Response<FileUrl>> UploadAvatar(
        IFormFile file,
        ClaimsPrincipal UserPrincipal,
        HttpRequest request
    )
    {
        if (file == null)
        {
            return new Response<FileUrl> { Message = "Aucun fichier téléversé", Status = 400 };
        }
        var user = CheckUser.GetUserFromClaim(UserPrincipal, _context);
        if (user is null)
        {
            return new Response<FileUrl> { Status = 40, Message = "Demande refusée" };
        }

        //verifier si le type est image
        var allowedMimeTypes = new[]
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/bmp",
            "image/webp",
        };
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (
            !allowedMimeTypes.Contains(file.ContentType)
            || !allowedExtensions.Contains(fileExtension)
        )
        {
            return new Response<FileUrl>
            {
                Status = 40,
                Message = "le type du ficheir n'est pas autorisé'",
            };
        }

        // supprimer l' ancien fichier s' il existe
        try
        {
            await _minioService.RemoveFileAsync(user.ImgUrl);
        }
        catch { }
        // resize

        using var inputStream = file.OpenReadStream();
        using var image = await Image.LoadAsync(inputStream);

        image.Mutate(x =>
            x.Resize(new ResizeOptions { Size = new Size(800, 1200), Mode = ResizeMode.Max })
        );

        using var outputStream = new MemoryStream();
        await image.SaveAsWebpAsync(outputStream);
        outputStream.Seek(0, SeekOrigin.Begin);

        // minio
        var url = await _minioService.UploadFileAsync("avatars", file.FileName, file);
        user.ImgUrl = url.ObjectName;

        await _context.SaveChangesAsync();

        if (user.ImgUrl is not null)
        {
            var imgUrl = await _minioService.GetFileUrlAsync(user.ImgUrl);
            user.ImgUrl = imgUrl;
        }

        return new Response<FileUrl>
        {
            Message = "Avatar téléversé",
            Status = 200,
            Data = new FileUrl { Url = user.ImgUrl },
        };
    }
}
