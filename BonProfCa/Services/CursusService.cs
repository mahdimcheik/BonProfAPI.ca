using Microsoft.EntityFrameworkCore;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Utilities;
using System.Security.Claims;

namespace BonProfCa.Services;

/// <summary>
/// Service pour la gestion des cursus
/// </summary>
public class CursusService(MainContext context)
{
    /// <summary>
    /// Récupère tous les cursus
    /// </summary>
    /// <returns>Liste des cursus</returns>
    public async Task<Response<List<CursusDetails>>> GetAllCursusAsync()
    {
        try
        {
            var cursuses = await context.Cursuses
                .AsNoTracking()
                .Include(c => c.Level)
                .Include(c => c.Teacher)
                .Include(c => c.Categories)
                .Where(c => c.ArchivedAt == null)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CursusDetails(c))
                .ToListAsync();

            return new Response<List<CursusDetails>>
            {
                Status = 200,
                Message = "Cursus récupérés avec succès",
                Data = cursuses,
                Count = cursuses.Count
            };
        }
        catch (Exception ex)
        {
            return new Response<List<CursusDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des cursus: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Récupère un cursus par son identifiant
    /// </summary>
    /// <param name="id">Identifiant du cursus</param>
    /// <returns>Cursus trouvé</returns>
    public async Task<Response<CursusDetails>> GetCursusByIdAsync(Guid id)
    {
        try
        {
            var cursus = await context.Cursuses
                .AsNoTracking()
                .Include(c => c.Level)
                .Include(c => c.Teacher)
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.Id == id && c.ArchivedAt == null);

            if (cursus == null)
            {
                return new Response<CursusDetails>
                {
                    Status = 404,
                    Message = "Cursus non trouvé",
                    Data = null
                };
            }

            return new Response<CursusDetails>
            {
                Status = 200,
                Message = "Cursus récupéré avec succès",
                Data = new CursusDetails(cursus)
            };
        }
        catch (Exception ex)
        {
            return new Response<CursusDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération du cursus: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Récupère les cursus d'un enseignant
    /// </summary>
    /// <param name="teacherId">Identifiant de l'enseignant</param>
    /// <returns>Liste des cursus de l'enseignant</returns>
    public async Task<Response<List<CursusDetails>>> GetCursusByTeacherIdAsync(Guid teacherId)
    {
        try
        {
            var cursuses = await context.Cursuses
                .AsNoTracking()
                .Include(c => c.Level)
                .Include(c => c.Teacher)
                .Include(c => c.Categories)
                .Where(c => c.TeacherId == teacherId && c.ArchivedAt == null)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CursusDetails(c))
                .ToListAsync();

            return new Response<List<CursusDetails>>
            {
                Status = 200,
                Message = "Cursus de l'enseignant récupérés avec succès",
                Data = cursuses,
                Count = cursuses.Count
            };
        }
        catch (Exception ex)
        {
            return new Response<List<CursusDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des cursus de l'enseignant: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Récupère les cursus par niveau
    /// </summary>
    /// <param name="levelId">Identifiant du niveau</param>
    /// <returns>Liste des cursus du niveau</returns>
    public async Task<Response<List<CursusDetails>>> GetCursusByLevelIdAsync(Guid levelId)
    {
        try
        {
            var cursuses = await context.Cursuses
                .AsNoTracking()
                .Include(c => c.Level)
                .Include(c => c.Teacher)
                .Include(c => c.Categories)
                .Where(c => c.LevelId == levelId && c.ArchivedAt == null)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CursusDetails(c))
                .ToListAsync();

            return new Response<List<CursusDetails>>
            {
                Status = 200,
                Message = "Cursus du niveau récupérés avec succès",
                Data = cursuses,
                Count = cursuses.Count
            };
        }
        catch (Exception ex)
        {
            return new Response<List<CursusDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des cursus du niveau: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Crée un nouveau cursus
    /// </summary>
    /// <param name="cursusDto">Données du cursus à créer</param>
    /// <returns>Cursus créé</returns>
    public async Task<Response<CursusDetails>> CreateCursusAsync(CursusCreate cursusDto, ClaimsPrincipal User)
    {
        try
        {
            // Vérifier que le niveau existe
            var levelExists = await context.LevelCursuses.AnyAsync(l => l.Id == cursusDto.LevelId && l.ArchivedAt == null);
            if (!levelExists)
            {
                return new Response<CursusDetails>
                {
                    Status = 404,
                    Message = "Niveau non trouvé",
                    Data = null
                };
            }

            // Vérifier que l'enseignant existe
            var teacher = CheckUser.GetUserFromClaim(User, context);
            if (teacher is null)
            {
                return new Response<CursusDetails>
                {
                    Status = 404,
                    Message = "Enseignant non trouvé",
                    Data = null
                };
            }

            // Vérifier que les catégories existent
            var categories = new List<CategoryCursus>();
            if (cursusDto.CategoryIds.Any())
            {
                categories = await context.CategoryCursuses
                    .Where(c => cursusDto.CategoryIds.Contains(c.Id) && c.ArchivedAt == null)
                    .ToListAsync();

                if (categories.Count != cursusDto.CategoryIds.Count)
                {
                    return new Response<CursusDetails>
                    {
                        Status = 404,
                        Message = "Une ou plusieurs catégories non trouvées",
                        Data = null
                    };
                }
            }

            var cursus = new Cursus(cursusDto,teacher.Id, categories);

            context.Cursuses.Add(cursus);
            await context.SaveChangesAsync();

            // Recharger avec les relations pour la réponse
            var createdCursus = await context.Cursuses
                .Include(c => c.Level)
                .Include(c => c.Teacher)
                .Include(c => c.Categories)
                .FirstAsync(c => c.Id == cursus.Id);

            return new Response<CursusDetails>
            {
                Status = 201,
                Message = "Cursus créé avec succès",
                Data = new CursusDetails(createdCursus)
            };
        }
        catch (Exception ex)
        {
            return new Response<CursusDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la création du cursus: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Met à jour un cursus existant
    /// </summary>
    /// <param name="id">Identifiant du cursus</param>
    /// <param name="cursusDto">Nouvelles données du cursus</param>
    /// <returns>Cursus mis à jour</returns>
    public async Task<Response<CursusDetails>> UpdateCursusAsync(CursusUpdate cursusDto, ClaimsPrincipal User)
    {
        try
        {
            var cursus = await context.Cursuses
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.Id == cursusDto.Id && c.ArchivedAt == null);

            if (cursus == null)
            {
                return new Response<CursusDetails>
                {
                    Status = 404,
                    Message = "Cursus non trouvé",
                    Data = null
                };
            }

            // Vérifier que le niveau existe
            var levelExists = await context.LevelCursuses.AnyAsync(l => l.Id == cursusDto.LevelId && l.ArchivedAt == null);
            if (!levelExists)
            {
                return new Response<CursusDetails>
                {
                    Status = 404,
                    Message = "Niveau non trouvé",
                    Data = null
                };
            }

            // Vérifier que l'enseignant existe
            var teacher = CheckUser.GetUserFromClaim(User, context);
            if (teacher is null || teacher.Id != cursus.TeacherId)
            {
                return new Response<CursusDetails>
                {
                    Status = 404,
                    Message = "Enseignant non trouvé, ou pas le propriétaire",
                    Data = null
                };
            }

            // Vérifier que le nom n'existe pas déjà pour un autre cursus
            var existingCursus = await context.Cursuses
                .AnyAsync(c => c.Name.ToLower() == cursusDto.Name.ToLower() && c.Id != cursusDto.Id && c.ArchivedAt == null);

            if (existingCursus)
            {
                return new Response<CursusDetails>
                {
                    Status = 400,
                    Message = "Un autre cursus avec ce nom existe déjà",
                    Data = null
                };
            }

            // Vérifier que les catégories existent
            var categories = new List<CategoryCursus>();
            if (cursusDto.CategoryIds.Any())
            {
                categories = await context.CategoryCursuses
                    .Where(c => cursusDto.CategoryIds.Contains(c.Id) && c.ArchivedAt == null)
                    .ToListAsync();

                if (categories.Count != cursusDto.CategoryIds.Count)
                {
                    return new Response<CursusDetails>
                    {
                        Status = 404,
                        Message = "Une ou plusieurs catégories non trouvées",
                        Data = null
                    };
                }
            }

            //cursus.Name = cursusDto.Name;
            //cursus.Color = cursusDto.Color;
            //cursus.Icon = cursusDto.Icon;
            //cursus.Description = cursusDto.Description;
            //cursus.LevelId = cursusDto.LevelId;
            //cursus.TeacherId = cursusDto.TeacherId;
            //cursus.Categories = categories;
            //cursus.UpdatedAt = DateTimeOffset.UtcNow;

            cursusDto.UpdateCursus(cursus, categories);

            await context.SaveChangesAsync();

            // Recharger avec les relations pour la réponse
            var updatedCursus = await context.Cursuses
                .Include(c => c.Level)
                .Include(c => c.Teacher)
                .Include(c => c.Categories)
                .FirstAsync(c => c.Id == cursusDto.Id);

            return new Response<CursusDetails>
            {
                Status = 200,
                Message = "Cursus mis à jour avec succès",
                Data = new CursusDetails(updatedCursus)
            };
        }
        catch (Exception ex)
        {
            return new Response<CursusDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la mise à jour du cursus: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Archive un cursus (suppression logique)
    /// </summary>
    /// <param name="id">Identifiant du cursus</param>
    /// <returns>Résultat de l'opération</returns>
    public async Task<Response<object>> DeleteCursusAsync(Guid id)
    {
        try
        {
            var cursus = await context.Cursuses
                .FirstOrDefaultAsync(c => c.Id == id && c.ArchivedAt == null);

            if (cursus == null)
            {
                return new Response<object>
                {
                    Status = 404,
                    Message = "Cursus non trouvé",
                    Data = null
                };
            }

            cursus.ArchivedAt = DateTimeOffset.UtcNow;
            cursus.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<object>
            {
                Status = 200,
                Message = "Cursus supprimé avec succès",
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new Response<object>
            {
                Status = 500,
                Message = $"Erreur lors de la suppression du cursus: {ex.Message}",
                Data = null
            };
        }
    }
}