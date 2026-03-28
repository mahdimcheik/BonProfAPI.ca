using Microsoft.EntityFrameworkCore;
using BonProfCa.Contexts;
using BonProfCa.Models;

namespace BonProfCa.Services;

public class StatusReservationService(MainContext context)
{
    public async Task<Response<List<StatusReservationDetails>>> GetAllStatusReservationsAsync()
    {
        try
        {
            var statuses = await context
                .StatusReservations.AsNoTracking()
                .Where(s => s.ArchivedAt == null)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new StatusReservationDetails(s))
                .ToListAsync();

            return new Response<List<StatusReservationDetails>>
            {
                Status = 200,
                Message = "Statuts de réservation récupérés avec succès",
                Data = statuses,
                Count = statuses.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<StatusReservationDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des statuts de réservation: {ex.Message}",
                Data = null,
            };
        }
    }
}
