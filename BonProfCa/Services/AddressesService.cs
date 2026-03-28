using Microsoft.EntityFrameworkCore;
using BonProfCa.Models;
using BonProfCa.Utilities;
using System.Security.Claims;
using BonProfCa.Contexts;

namespace BonProfCa.Services;


public class AddressesService(MainContext context)
{
    public async Task<Response<List<AddressDetails>>> GetAddressesByUserIdAsync(ClaimsPrincipal principal)
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(principal, context);
            if (user is null)
            {
                return new Response<List<AddressDetails>>
                {
                    Status = 404,
                    Message = $"L'utilisateur n'existe pas",
                };
            }
            var profile = await context.Users.FirstOrDefaultAsync(p => p.Id == user.Id);
            if (profile is null)
            {
                return new Response<List<AddressDetails>>
                {
                    Status = 404,
                    Message = $"L'utilisateur n'existe pas",
                };
            }

            var addresses = await context.Addresses
                .AsNoTracking()
                .Where(a => a.UserId == profile.Id)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AddressDetails(a))
                .ToListAsync();

            return new Response<List<AddressDetails>>
            {
                Status = 200,
                Message = "Adresses de l'utilisateur récupérées avec succès",
                Data = addresses,
                Count = addresses.Count
            };
        }
        catch (Exception ex)
        {
            return new Response<List<AddressDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des adresses de l'utilisateur: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Response<AddressDetails>> CreateAddressAsync(AddressCreate addressDto, ClaimsPrincipal User)
    {
        try
        {
            // Vérifier que l'utilisateur existe
            var user = CheckUser.GetUserFromClaim(User, context);
            if (user is null)
            {
                return new Response<AddressDetails>
                {
                    Status = 404,
                    Message = "Utilisateur non trouvé",
                    Data = null
                };
            }
            var profile = await context.Users.FirstOrDefaultAsync(p => p.Id == user.Id);
            if (profile is null)
            {
                return new Response<AddressDetails>
                {
                    Status = 404,
                    Message = "Utilisateur non trouvé",
                    Data = null
                };
            }
            var addressesCount = await context.Addresses.CountAsync(a => a.UserId == profile.Id && a.ArchivedAt == null);

            if (addressesCount >= 2)
            {
                return new Response<AddressDetails>
                {
                    Status = 401,
                    Message = "Le nombre d'addresses autorisé est depassé",
                    Data = null
                };
            }

            addressDto.UserId = profile.Id;
            var address = new Address(addressDto);

            context.Addresses.Add(address);
            await context.SaveChangesAsync();

            return new Response<AddressDetails>
            {
                Status = 201,
                Message = "Adresse créée avec succès",
                Data = new AddressDetails(address)
            };
        }
        catch (Exception ex)
        {
            return new Response<AddressDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la création de l'adresse: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Response<AddressDetails>> UpdateAddressAsync(AddressUpdate addressDto, ClaimsPrincipal User)
    {
        try
        {
            var address = await context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressDto.Id && a.ArchivedAt == null);

            if (address == null)
            {
                return new Response<AddressDetails>
                {
                    Status = 404,
                    Message = "Adresse non trouvée",
                    Data = null
                };
            }

            // Vérifier que l'utilisateur existe
            var user = CheckUser.GetUserFromClaim(User, context);
            if (user is null)
            {
                return new Response<AddressDetails>
                {
                    Status = 404,
                    Message = "Utilisateur non trouvé",
                    Data = null
                };
            }
            address.UpdateAddress(addressDto);
            await context.SaveChangesAsync();
            return new Response<AddressDetails>
            {
                Status = 200,
                Message = "Adresse mise à jour avec succès",
                Data = new AddressDetails(address)
            };
        }
        catch (Exception ex)
        {
            return new Response<AddressDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la mise à jour de l'adresse: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Response<object>> DeleteAddressAsync(Guid id, ClaimsPrincipal principal)
    {
        try
        {
            // Vérifier que l'utilisateur existe
            var user = CheckUser.GetUserFromClaim(principal, context);
            if (user is null)
            {
                return new Response<object>
                {
                    Status = 404,
                    Message = "Utilisateur non trouvé",
                };
            }
            var profile = await context.Users.FirstOrDefaultAsync(p => p.Id == user.Id);
            if (profile is null)
            {
                return new Response<object>
                {
                    Status = 404,
                    Message = "Utilisateur non trouvé",
                };
            }
            var address = await context.Addresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == profile.Id);

            if (address == null)
            {
                return new Response<object>
                {
                    Status = 404,
                    Message = "Adresse non trouvée",
                    Data = null
                };
            }

            address.ArchivedAt = DateTimeOffset.UtcNow;
            address.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<object>
            {
                Status = 200,
                Message = "Adresse supprimée avec succès",
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new Response<object>
            {
                Status = 500,
                Message = $"Erreur lors de la suppression de l'adresse: {ex.Message}",
                Data = null
            };
        }
    }
}
