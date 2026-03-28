using System.Net.NetworkInformation;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BonProfCa.Services;

public class FormationsService(MainContext context)
{

    public async Task<Response<List<FormationDetails>>> GetAllFormationsAsync()
    {
        try
        {
            var formations = await context
                .Formations.AsNoTracking()
                .Where(f => f.ArchivedAt == null)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FormationDetails(f))
                .ToListAsync();

            return new Response<List<FormationDetails>>
            {
                Status = 200,
                Message = "Formations récupérées avec succès",
                Data = formations,
                Count = formations.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<FormationDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des formations: {ex.Message}",
                Data = null,
            };
        }
    }


    public async Task<Response<FormationDetails>> GetFormationByIdAsync(Guid id)
    {
        try
        {
            var formation = await context
                .Formations.AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id && f.ArchivedAt == null);

            if (formation == null)
            {
                return new Response<FormationDetails>
                {
                    Status = 404,
                    Message = "Formation non trouvée",
                    Data = null,
                };
            }

            return new Response<FormationDetails>
            {
                Status = 200,
                Message = "Formation récupérée avec succès",
                Data = new FormationDetails(formation),
            };
        }
        catch (Exception ex)
        {
            return new Response<FormationDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération de la formation: {ex.Message}",
                Data = null,
            };
        }
    }

 
    public async Task<Response<List<FormationDetails>>> GetFormationsByUserIdAsync(Guid userId)
    {
        try
        {
            var formations = await context
                .Formations.AsNoTracking()
                .Where(f => f.UserId == userId && f.ArchivedAt == null)
                .OrderByDescending(f => f.DateFrom)
                .Select(f => new FormationDetails(f))
                .ToListAsync();

            return new Response<List<FormationDetails>>
            {
                Status = 200,
                Message = "Formations de l'utilisateur récupérées avec succès",
                Data = formations,
                Count = formations.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<FormationDetails>>
            {
                Status = 500,
                Message =
                    $"Erreur lors de la récupération des formations de l'utilisateur: {ex.Message}",
                Data = null,
            };
        }
    }


    public async Task<Response<FormationDetails>> CreateFormationAsync(
        FormationCreate formationDto,
        ClaimsPrincipal User
    )
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(User, context);
            if (user is null)
            {
                return new Response<FormationDetails>
                {
                    Status = 404,
                    Message = "Utilisateur non trouvé",
                    Data = null,
                };
            }

            // Validation des dates
            if (formationDto.DateTo.HasValue && formationDto.DateTo <= formationDto.DateFrom)
            {
                return new Response<FormationDetails>
                {
                    Status = 400,
                    Message = "La date de fin doit être postérieure à la date de début",
                    Data = null,
                };
            }

            var count = await context.Formations.Where(f => f.UserId == user.Id && f.ArchivedAt == null).CountAsync();
            if (count >= 5)
            {
                return new Response<FormationDetails>
                {
                    Status = 403,
                    Message = "Le nombre maximal de formations autorisé par utilisateur est de 5",
                    Data = null,
                };
            }

            var formation = new Formation(formationDto, user.Id);

            context.Formations.Add(formation);
            await context.SaveChangesAsync();

            return new Response<FormationDetails>
            {
                Status = 201,
                Message = "Formation créée avec succès",
                Data = new FormationDetails(formation),
            };
        }
        catch (Exception ex)
        {
            return new Response<FormationDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la création de la formation: {ex.Message}",
                Data = null,
            };
        }
    }

    public async Task<Response<FormationDetails>> UpdateFormationAsync(FormationUpdate formationDto, ClaimsPrincipal User)
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(User, context);
            if (user is  null)
            {
                return new Response<FormationDetails>
                {
                    Status = 404,
                    Message = "Utilisateur non trouvé",
                    Data = null,
                };
            }
            var formation = await context.Formations.FirstOrDefaultAsync(f =>
                f.Id == formationDto.Id && f.UserId == user.Id && f.ArchivedAt == null
            );

            if (formation == null)
            {
                return new Response<FormationDetails>
                {
                    Status = 404,
                    Message = "Formation non trouvée",
                    Data = null,
                };
            }
            // Validation des dates
            if (formationDto.DateTo.HasValue && formationDto.DateTo <= formationDto.DateFrom)
            {
                return new Response<FormationDetails>
                {
                    Status = 400,
                    Message = "La date de fin doit être postérieure à la date de début",
                    Data = null,
                };
            }

            formation.UpdateFormation(formationDto);

            await context.SaveChangesAsync();

            return new Response<FormationDetails>
            {
                Status = 200,
                Message = "Formation mise à jour avec succès",
                Data = new FormationDetails(formation),
            };
        }
        catch (Exception ex)
        {
            return new Response<FormationDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la mise à jour de la formation: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Archive une formation (suppression logique)
    /// </summary>
    /// <param name="id">Identifiant de la formation</param>
    /// <returns>Résultat de l'opération</returns>
    public async Task<Response<bool>> DeleteFormationAsync(Guid id)
    {
        try
        {
            var formation = await context.Formations.FirstOrDefaultAsync(f =>
                f.Id == id && f.ArchivedAt == null
            );

            if (formation == null)
            {
                return new Response<bool>
                {
                    Status = 404,
                    Message = "Formation non trouvée",
                    Data = false,
                };
            }

            formation.ArchivedAt = DateTimeOffset.UtcNow;
            formation.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<bool>
            {
                Status = 200,
                Message = "Formation supprimée avec succès",
                Data = true,
            };
        }
        catch (Exception ex)
        {
            return new Response<bool>
            {
                Status = 500,
                Message = $"Erreur lors de la suppression de la formation: {ex.Message}",
                Data = true,
            };
        }
    }
}
