using System.Security.Claims;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Models;
using BonProfCa.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BonProfCa.Services;

/// <summary>
/// Service pour la gestion des langues
/// </summary>
public class LanguagesService(MainContext context)
{
    /// <summary>
    /// Récupère toutes les langues
    /// </summary>
    /// <returns>Liste des langues</returns>
    public async Task<Response<List<LanguageDetails>>> GetAllLanguagesAsync()
    {
        try
        {
            var languages = await context.Languages.AsNoTracking().ToListAsync();

            return new Response<List<LanguageDetails>>
            {
                Status = 200,
                Message = "Langues récupérées avec succès",
                Data = languages.Select(x => new LanguageDetails(x)).ToList(),
            };
        }
        catch (Exception ex)
        {
            return new Response<List<LanguageDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des langues: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Récupère les langues d'un utilisateur
    /// </summary>
    /// <param name="teacherId">Identifiant de l'utilisateur</param>
    /// <returns>Liste des langues de l'utilisateur</returns>
    public async Task<Response<List<LanguageDetails>>> GetLanguagesByTeacherIdAsync(
        ClaimsPrincipal principal
    )
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(principal, context);
            if (user is null)
            {
                return new Response<List<LanguageDetails>>
                {
                    Status = 404,
                    Message = $"L'utilisateur n'existe pas",
                    Data = null,
                };
            }

            var languages = await context
                .Users.Where(p => p.Id == user.Id)
                .AsNoTracking()
                .Include(p => p.Languages)
                .SelectMany(p => p.Languages)
                .Select(l => new LanguageDetails(l))
                .ToListAsync();

            return new Response<List<LanguageDetails>>
            {
                Status = 200,
                Message = "Langues de l'utilisateur récupérées avec succès",
                Data = languages,
                Count = languages.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<LanguageDetails>>
            {
                Status = 500,
                Message =
                    $"Erreur lors de la récupération des langues de l'utilisateur: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Crée une nouvelle langue
    /// </summary>
    /// <param name="languageDto">Données de la langue à créer</param>
    /// <returns>Langue créée</returns>
    public async Task<Response<LanguageDetails>> CreateLanguageAsync(LanguageCreate languageDto)
    {
        try
        {
            // Vérifier que le nom n'existe pas déjà
            var existingLanguage = await context.Languages.AnyAsync(l =>
                l.Name.ToLower() == languageDto.Name.ToLower() && l.ArchivedAt == null
            );

            if (existingLanguage)
            {
                return new Response<LanguageDetails>
                {
                    Status = 400,
                    Message = "Une langue avec ce nom existe déjà",
                    Data = null,
                };
            }

            var language = new Language
            {
                Id = Guid.NewGuid(),
                Name = languageDto.Name,
                Color = languageDto.Color,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            context.Languages.Add(language);
            await context.SaveChangesAsync();

            return new Response<LanguageDetails>
            {
                Status = 201,
                Message = "Langue créée avec succès",
                Data = new LanguageDetails(language),
            };
        }
        catch (Exception ex)
        {
            return new Response<LanguageDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la création de la langue: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Met à jour une langue existante
    /// </summary>
    /// <param name="id">Identifiant de la langue</param>
    /// <param name="languageDto">Nouvelles données de la langue</param>
    /// <returns>Langue mise à jour</returns>
    public async Task<Response<LanguageDetails>> UpdateLanguageAsync(
        Guid id,
        LanguageUpdate languageDto
    )
    {
        try
        {
            var language = await context.Languages.FirstOrDefaultAsync(l =>
                l.Id == id && l.ArchivedAt == null
            );

            if (language == null)
            {
                return new Response<LanguageDetails>
                {
                    Status = 404,
                    Message = "Langue non trouvée",
                    Data = null,
                };
            }

            // Vérifier que le nom n'existe pas déjà pour une autre langue
            var existingLanguage = await context.Languages.AnyAsync(l =>
                l.Name.ToLower() == languageDto.Name.ToLower() && l.Id != id && l.ArchivedAt == null
            );

            if (existingLanguage)
            {
                return new Response<LanguageDetails>
                {
                    Status = 400,
                    Message = "Une autre langue avec ce nom existe déjà",
                    Data = null,
                };
            }

            language.Name = languageDto.Name;
            language.Color = languageDto.Color;
            language.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<LanguageDetails>
            {
                Status = 200,
                Message = "Langue mise à jour avec succès",
                Data = new LanguageDetails(language),
            };
        }
        catch (Exception ex)
        {
            return new Response<LanguageDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la mise à jour de la langue: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Archive une langue (suppression logique)
    /// </summary>
    /// <param name="id">Identifiant de la langue</param>
    /// <returns>Résultat de l'opération</returns>
    public async Task<Response<object>> DeleteLanguageAsync(Guid id)
    {
        try
        {
            var language = await context.Languages.FirstOrDefaultAsync(l =>
                l.Id == id && l.ArchivedAt == null
            );

            if (language == null)
            {
                return new Response<object>
                {
                    Status = 404,
                    Message = "Langue non trouvée",
                    Data = null,
                };
            }

            language.ArchivedAt = DateTimeOffset.UtcNow;
            language.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<object>
            {
                Status = 200,
                Message = "Langue supprimée avec succès",
                Data = null,
            };
        }
        catch (Exception ex)
        {
            return new Response<object>
            {
                Status = 500,
                Message = $"Erreur lors de la suppression de la langue: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Associe une langue à un utilisateur
    /// </summary>
    /// <param name="userLanguageDto">Données d'association utilisateur-langue</param>
    /// <returns>Résultat de l'opération</returns>
    public async Task<Response<List<LanguageDetails>>> UpdateLanguagesForUser(
        Guid[] languagesIds,
        ClaimsPrincipal principal
    )
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(principal, context);
            if (user is null)
            {
                return new Response<List<LanguageDetails>>
                {
                    Status = 404,
                    Message = $"L'utilisateur n'existe pas",
                    Data = null,
                };
            }
            var userWithLanguage = await context.Users.FirstOrDefaultAsync(p => p.Id == user!.Id);
            if (userWithLanguage is null)
            {
                return new Response<List<LanguageDetails>>
                {
                    Status = 404,
                    Message = $"L'utilisateur n'existe pas",
                    Data = null,
                };
            }

            var newLanguages = await context
                .Languages.Where(l => languagesIds.Contains(l.Id))
                .ToListAsync();

            userWithLanguage.Languages = newLanguages;
            await context.SaveChangesAsync();

            return new Response<List<LanguageDetails>>
            {
                Data = newLanguages.Select(l => new LanguageDetails(l)).ToList(),
                Message = "Ok",
                Status = 200,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<LanguageDetails>>
            {
                Status = 500,
                Message =
                    $"Erreur lors de l'association de la langue à l'utilisateur: {ex.Message}",
                Data = null,
            };
        }
    }
}
