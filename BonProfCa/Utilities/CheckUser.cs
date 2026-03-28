using Microsoft.AspNetCore.Identity;
using BonProfCa.Models;
using System.Security.Claims;
using BonProfCa.Contexts;

namespace BonProfCa.Utilities;

public class CheckUser
{
    public static UserApp? GetUserFromClaim(
        ClaimsPrincipal userClaim,
        MainContext context
    )
    {
        string? userEmail = userClaim
            .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)
            ?.Value;
        if (userEmail == null)
            return null;
        var user = context
            .Users
            .FirstOrDefault(x => x.Email == userEmail);

        if (user == null)
            return null;
        return user;
    }

    public static async Task<(UserApp? user, bool isNull)> CheckUserNullByEmail(
        string email,
        UserManager<UserApp> _userManager
    )
    {
        var user = await _userManager.FindByEmailAsync(email);

        return (user, user is null);
    }

    public static async Task<(UserApp? user, bool isNull)> CheckUserNullByUserId(
        string id,
        UserManager<UserApp> _userManager
    )
    {
        var user = await _userManager.FindByIdAsync(id);

        return (user, user is null);
    }


    public static async Task<List<RoleDetails>> GetRoles(UserApp user, MainContext context, UserManager<UserApp> userManager)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        var roles = context.Roles.ToList();

        var rolesDetailed = roles
            .Where(r => userRoles.Contains(r.Name ?? string.Empty))
            .Select(r => new RoleDetails(r))
            .ToList();
        return rolesDetailed;
    }

}

