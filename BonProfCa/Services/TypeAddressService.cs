using Microsoft.EntityFrameworkCore;
using BonProfCa.Contexts;
using BonProfCa.Models;

namespace BonProfCa.Services;

public class TypeAddressService(MainContext context)
{
    /// <summary>
    /// Récupère toutes les types  adresses
    /// </summary>
    /// <returns>Liste des adresses</returns>
    public async Task<Response<List<TypeAddressDetails>>> GetAllAddresseTypesAsync()
    {
        try
        {
            var addressTypes = await context.TypeAddresses
                .AsNoTracking()
                .Where(a => a.ArchivedAt == null)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new TypeAddressDetails(a))
                .ToListAsync();

            return new Response<List<TypeAddressDetails>>
            {
                Status = 200,
                Message = "Adresses récupérées avec succès",
                Data = addressTypes,
                Count = addressTypes.Count
            };
        }
        catch (Exception ex)
        {
            return new Response<List<TypeAddressDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des adresses: {ex.Message}",
                Data = null
            };
        }
    }
}
