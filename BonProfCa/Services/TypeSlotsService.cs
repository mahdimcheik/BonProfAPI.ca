using Microsoft.EntityFrameworkCore;
using BonProfCa.Contexts;
using BonProfCa.Models;

namespace BonProfCa.Services;

/// <summary>
/// Service pour la gestion des types de cr�neaux
/// </summary>
public class TypeSlotsService(MainContext context)
{
    /// <summary>
    /// R�cup�re tous les types de cr�neaux
    /// </summary>
    /// <returns>Liste des types de cr�neaux</returns>
    public async Task<Response<List<TypeSlotDetails>>> GetAllTypeSlotsAsync()
    {
        try
        {
            var typeSlots = await context
                .TypeSlots.AsNoTracking()
                .Where(t => t.ArchivedAt == null)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TypeSlotDetails(t))
                .ToListAsync();

            return new Response<List<TypeSlotDetails>>
            {
                Status = 200,
                Message = "Types de cr�neaux r�cup�r�s avec succ�s",
                Data = typeSlots,
                Count = typeSlots.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<TypeSlotDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration des types de cr�neaux: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// R�cup�re un type de cr�neau par son identifiant
    /// </summary>
    /// <param name="id">Identifiant du type de cr�neau</param>
    /// <returns>Type de cr�neau trouv�</returns>
    public async Task<Response<TypeSlotDetails>> GetTypeSlotByIdAsync(Guid id)
    {
        try
        {
            var typeSlot = await context
                .TypeSlots.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.ArchivedAt == null);

            if (typeSlot == null)
            {
                return new Response<TypeSlotDetails>
                {
                    Status = 404,
                    Message = "Type de cr�neau non trouv�",
                    Data = null,
                };
            }

            return new Response<TypeSlotDetails>
            {
                Status = 200,
                Message = "Type de cr�neau r�cup�r� avec succ�s",
                Data = new TypeSlotDetails(typeSlot),
            };
        }
        catch (Exception ex)
        {
            return new Response<TypeSlotDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration du type de cr�neau: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Cr�e un nouveau type de cr�neau
    /// </summary>
    /// <param name="typeSlotDto">Donn�es du type de cr�neau � cr�er</param>
    /// <returns>Type de cr�neau cr��</returns>
    public async Task<Response<TypeSlotDetails>> CreateTypeSlotAsync(TypeSlotCreate typeSlotDto)
    {
        try
        {
            // V�rifier si un type de cr�neau avec le m�me nom existe d�j�
            var existingTypeSlot = await context.TypeSlots.AnyAsync(t =>
                t.Name.ToLower() == typeSlotDto.Name.ToLower() && t.ArchivedAt == null
            );

            if (existingTypeSlot)
            {
                return new Response<TypeSlotDetails>
                {
                    Status = 400,
                    Message = "Un type de cr�neau avec ce nom existe d�j�",
                    Data = null,
                };
            }

            var typeSlot = new TypeSlot
            {
                Id = Guid.NewGuid(),
                Name = typeSlotDto.Name,
                Color = typeSlotDto.Color,
                Icon = typeSlotDto.Icon,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            context.TypeSlots.Add(typeSlot);
            await context.SaveChangesAsync();

            return new Response<TypeSlotDetails>
            {
                Status = 201,
                Message = "Type de cr�neau cr�� avec succ�s",
                Data = new TypeSlotDetails(typeSlot),
            };
        }
        catch (Exception ex)
        {
            return new Response<TypeSlotDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la cr�ation du type de cr�neau: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Met � jour un type de cr�neau existant
    /// </summary>
    /// <param name="id">Identifiant du type de cr�neau</param>
    /// <param name="typeSlotDto">Nouvelles donn�es du type de cr�neau</param>
    /// <returns>Type de cr�neau mis � jour</returns>
    public async Task<Response<TypeSlotDetails>> UpdateTypeSlotAsync(
        TypeSlotUpdate typeSlotDto
    )
    {
        try
        {
            var typeSlot = await context.TypeSlots.FirstOrDefaultAsync(t =>
                t.Id == typeSlotDto.Id && t.ArchivedAt == null
            );

            if (typeSlot == null)
            {
                return new Response<TypeSlotDetails>
                {
                    Status = 404,
                    Message = "Type de cr�neau non trouv�",
                    Data = null,
                };
            }

            // V�rifier si le nom n'existe pas d�j� pour un autre type de cr�neau
            var existingTypeSlot = await context.TypeSlots.AnyAsync(t =>
                t.Name.ToLower() == typeSlotDto.Name.ToLower()
                && t.Id != typeSlotDto.Id
                && t.ArchivedAt == null
            );

            if (existingTypeSlot)
            {
                return new Response<TypeSlotDetails>
                {
                    Status = 400,
                    Message = "Un autre type de cr�neau avec ce nom existe d�j�",
                    Data = null,
                };
            }

            typeSlotDto.UpdateTypeSlot(typeSlot);

            await context.SaveChangesAsync();

            return new Response<TypeSlotDetails>
            {
                Status = 200,
                Message = "Type de cr�neau mis � jour avec succ�s",
                Data = new TypeSlotDetails(typeSlot),
            };
        }
        catch (Exception ex)
        {
            return new Response<TypeSlotDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la mise � jour du type de cr�neau: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Archive un type de cr�neau (suppression logique)
    /// </summary>
    /// <param name="id">Identifiant du type de cr�neau</param>
    /// <returns>R�sultat de l'op�ration</returns>
    public async Task<Response<bool>> DeleteTypeSlotAsync(Guid id)
    {
        try
        {
            var typeSlot = await context.TypeSlots.FirstOrDefaultAsync(t =>
                t.Id == id && t.ArchivedAt == null
            );

            if (typeSlot == null)
            {
                return new Response<bool>
                {
                    Status = 404,
                    Message = "Type de cr�neau non trouv�",
                    Data = false,
                };
            }

            typeSlot.ArchivedAt = DateTimeOffset.UtcNow;
            typeSlot.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<bool>
            {
                Status = 200,
                Message = "Type de cr�neau supprim� avec succ�s",
                Data = true,
            };
        }
        catch (Exception ex)
        {
            return new Response<bool>
            {
                Status = 500,
                Message = $"Erreur lors de la suppression du type de cr�neau: {ex.Message}",
                Data = false,
            };
        }
    }
}
