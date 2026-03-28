using System.Security.Claims;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BonProfCa.Services;

public class ConversationService(MainContext context)
{
    public async Task<Response<bool>> SendMessageAsync(
        ConversationCreate dto
    )
    {
        try
        {
            var reservationExists = await context.Reservations.AnyAsync(r =>
                r.Id == dto.ReservationId
            );

            if (!reservationExists)
                return new Response<bool>
                {
                    Status = 404,
                    Message = "Réservation introuvable",
                    Data = false,
                };

            var conversation = new Conversation
            {
                Content = dto.Content,
                ReservationId = dto.ReservationId,
                SenderId = dto.SenderId,
            };

            context.Conversations.Add(conversation);
            await context.SaveChangesAsync();

            return new Response<bool>
            {
                Status = 200,
                Message = "Message envoyé avec succès",
                Data = true,
            };
        }
        catch (Exception ex)
        {
            return new Response<bool>
            {
                Status = 500,
                Message = $"Erreur: {ex.Message}",
                Data = false,
            };
        }
    }

    public async Task<Response<List<ConversationDetails>>> GetConversationByReservationIdAsync(
        Guid reservationId
    )
    {
        try
        {
            

            var reservationExists = await context.Reservations.AnyAsync(r => r.Id == reservationId);

            if (!reservationExists)
                return new Response<List<ConversationDetails>>
                {
                    Status = 404,
                    Message = "Réservation introuvable",
                };

            var conversations = await context
                .Conversations.Where(c => c.ReservationId == reservationId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new ConversationDetails
                {
                    Id = c.Id,
                    Content = c.Content,
                    ReservationId = c.ReservationId,
                    CreatedAt = c.CreatedAt,
                    SenderId = c.SenderId,
                })
                .ToListAsync();

            return new Response<List<ConversationDetails>>
            {
                Status = 200,
                Message = "Conversations récupérées avec succès",
                Data = conversations,
                Count = conversations.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<ConversationDetails>>
            {
                Status = 500,
                Message = $"Erreur: {ex.Message}",
            };
        }
    }
}
