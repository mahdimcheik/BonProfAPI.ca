using System.Security.Claims;
using System.Text.Json;
using BonProfCa.Contexts;
using BonProfCa.Models;
using BonProfCa.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BonProfCa.Services;

public class SlotsService(MainContext context, MinioService _minioService, SignalRNotificationsService signalRService)
{
    public async Task<Response<SlotDetails>> AddSlotByTeacherAsync(
        SlotCreate slotDto,
        ClaimsPrincipal userPrincipal
    )
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(userPrincipal, context);
            if (user == null)
            {
                return new Response<SlotDetails>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifié",
                    Data = null,
                };
            }

            var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (teacher == null)
            {
                return new Response<SlotDetails>
                {
                    Status = 403,
                    Message = "Vous devez être un enseignant pour cr�er des créneaux",
                    Data = null,
                };
            }

            // Validation des dates
            if (slotDto.DateFrom >= slotDto.DateTo)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "La date de fin doit être postérieure à la date de début",
                    Data = null,
                };
            }

            if (slotDto.DateFrom < DateTimeOffset.UtcNow)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Le créneau doit être dans le futur",
                    Data = null,
                };
            }

            var typeExists = await context.TypeSlots.AnyAsync(t =>
                t.Id == slotDto.TypeId && t.ArchivedAt == null
            );
            if (!typeExists)
            {
                return new Response<SlotDetails>
                {
                    Status = 404,
                    Message = "Type de créneau non trouvé",
                    Data = null,
                };
            }

            var hasOverlap = await context.Slots.AnyAsync(s =>
                s.TeacherId == teacher.Id
                && s.ArchivedAt == null
                && (
                    (slotDto.DateFrom >= s.DateFrom && slotDto.DateFrom < s.DateTo)
                    || (slotDto.DateTo > s.DateFrom && slotDto.DateTo <= s.DateTo)
                    || (slotDto.DateFrom <= s.DateFrom && slotDto.DateTo >= s.DateTo)
                )
            );

            if (hasOverlap)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Ce cr�neau chevauche un cr�neau existant",
                    Data = null,
                };
            }

            var slot = new Slot
            {
                Id = Guid.NewGuid(),
                DateFrom = slotDto.DateFrom,
                DateTo = slotDto.DateTo,
                TeacherId = teacher.Id,
                TypeId = slotDto.TypeId,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            context.Slots.Add(slot);
            await context.SaveChangesAsync();

            var createdSlot = await context
                .Slots.Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Type)
                .FirstAsync(s => s.Id == slot.Id);

            return new Response<SlotDetails>
            {
                Status = 201,
                Message = "Cr�neau créé avec succès",
                Data = new SlotDetails(createdSlot),
            };
        }
        catch (Exception ex)
        {
            return new Response<SlotDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la création du cr�neau: {ex.Message}",
                Data = null,
            };
        }
    }

    public async Task<Response<SlotDetails>> UpdateSlotByTeacherAsync(
        SlotUpdate slotDto,
        ClaimsPrincipal userPrincipal
    )
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(userPrincipal, context);
            if (user == null)
            {
                return new Response<SlotDetails>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifié",
                    Data = null,
                };
            }

            var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (teacher == null)
            {
                return new Response<SlotDetails>
                {
                    Status = 403,
                    Message = "Vous devez �tre un enseignant pour créer des créneaux",
                    Data = null,
                };
            }

            // Validation des dates
            if (slotDto.DateFrom >= slotDto.DateTo)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "La date de fin doit être postérieure à la date de début",
                    Data = null,
                };
            }

            if (slotDto.DateFrom < DateTimeOffset.UtcNow)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Le cr�neau doit être dans le futur",
                    Data = null,
                };
            }

            var typeExists = await context.TypeSlots.AnyAsync(t =>
                t.Id == slotDto.TypeId && t.ArchivedAt == null
            );
            if (!typeExists)
            {
                return new Response<SlotDetails>
                {
                    Status = 404,
                    Message = "Type de cr�neau non trouv�",
                    Data = null,
                };
            }

            var hasOverlap = await context.Slots.AnyAsync(s =>
                s.TeacherId == teacher.Id
                && s.Id != slotDto.Id
                && s.ArchivedAt == null
                && (
                    (slotDto.DateFrom >= s.DateFrom && slotDto.DateFrom < s.DateTo)
                    || (slotDto.DateTo > s.DateFrom && slotDto.DateTo <= s.DateTo)
                    || (slotDto.DateFrom <= s.DateFrom && slotDto.DateTo >= s.DateTo)
                )
            );

            if (hasOverlap)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Ce cr�neau chevauche un cr�neau existant",
                    Data = null,
                };
            }

            var slot = await context.Slots.FirstOrDefaultAsync(x => x.Id == slotDto.Id);

            if (slot is null)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Ce cr�neau n'existe pas/plus",
                    Data = null,
                };
            }
            slotDto.UpdateSlot(slot);
            await context.SaveChangesAsync();

            return new Response<SlotDetails>
            {
                Status = 201,
                Message = "Cr�neau cr�� avec succ�s",
                Data = new SlotDetails(slot),
            };
        }
        catch (Exception ex)
        {
            return new Response<SlotDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la cr�ation du cr�neau: {ex.Message}",
                Data = null,
            };
        }
    }

    public async Task<Response<bool>> RemoveSlotByTeacherAsync(
        Guid slotId,
        ClaimsPrincipal userPrincipal
    )
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(userPrincipal, context);
            if (user == null)
            {
                return new Response<bool>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifi�",
                    Data = false,
                };
            }

            var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (teacher == null)
            {
                return new Response<bool>
                {
                    Status = 403,
                    Message = "Vous devez �tre un enseignant pour supprimer des cr�neaux",
                    Data = false,
                };
            }

            var slot = await context
                .Slots.Include(s =>
                    s.Reservations.Where(r =>
                        r.ArchivedAt == null
                        && (
                            r.StatusId == HardCode.RESERVATION_ACCEPTED
                            || r.StatusId == HardCode.RESERVATION_PENDING
                        )
                    )
                )
                .FirstOrDefaultAsync(s => s.Id == slotId && s.ArchivedAt == null);

            if (slot == null)
            {
                return new Response<bool>
                {
                    Status = 404,
                    Message = "Cr�neau non trouv�",
                    Data = false,
                };
            }

            if (slot.TeacherId != teacher.Id)
            {
                return new Response<bool>
                {
                    Status = 403,
                    Message = "Vous n'�tes pas autoris� � supprimer ce cr�neau",
                    Data = false,
                };
            }

            if (slot.Reservations != null && slot.Reservations.Any())
            {
                return new Response<bool>
                {
                    Status = 400,
                    Message = "Impossible de supprimer un créneau réservé",
                    Data = false,
                };
            }

            slot.ArchivedAt = DateTimeOffset.UtcNow;
            slot.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<bool>
            {
                Status = 200,
                Message = "Créneau supprimé avec succès",
                Data = true,
            };
        }
        catch (Exception ex)
        {
            return new Response<bool>
            {
                Status = 500,
                Message = $"Erreur lors de la suppression du créneau: {ex.Message}",
                Data = false,
            };
        }
    }

    public async Task<Response<List<SlotDetails>>> GetSlotsByTeacherAndDatesAsync(
        ClaimsPrincipal userPrincipal,
        DateTimeOffset dateFrom,
        DateTimeOffset dateTo
    )
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(userPrincipal, context);
            if (user == null)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifié",
                    Data = null,
                };
            }

            var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (teacher == null)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 403,
                    Message = "Vous devez être un enseignant pour consulter des créneaux",
                    Data = null,
                };
            }

            // Validation des dates
            if (dateFrom >= dateTo)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 400,
                    Message = "La date de fin doit �tre post�rieure � la date de d�but",
                    Data = null,
                };
            }

            var slots = await context
                .Slots.AsNoTracking()
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Type)
                .Where(s =>
                    s.TeacherId == teacher.Id
                    && s.ArchivedAt == null
                    && s.DateFrom >= dateFrom
                    && s.DateTo <= dateTo
                )
                .Include(s =>
                    s.Reservations.Where(r =>
                        r.ArchivedAt == null
                        && (
                            r.StatusId == HardCode.RESERVATION_ACCEPTED
                            || r.StatusId == HardCode.RESERVATION_PENDING
                        )
                    )
                )
                    .ThenInclude(r => r.Student)
                        .ThenInclude(s => s.User)
                .Include(s =>
                    s.Reservations.Where(r =>
                        r.ArchivedAt == null
                        && (
                            r.StatusId == HardCode.RESERVATION_ACCEPTED
                            || r.StatusId == HardCode.RESERVATION_PENDING
                        )
                    )
                )
                    .ThenInclude(r => r.Status)
                .OrderBy(s => s.DateFrom)
                // .Select(s => new SlotDetails(s))
                .ToListAsync();

            try
            {
                var students = slots
                    .SelectMany(x => x.Reservations)
                    .Select(r => r.Student)
                    .Distinct()
                    .ToList();
                foreach (var student in students)
                {
                    if (
                        student?.User?.ImgUrl is not null
                        && !string.IsNullOrEmpty(student?.User?.ImgUrl)
                    )
                    {
                        student.User.ImgUrl =
                            await _minioService.GetFileUrlAsync(student.User.ImgUrl) ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                var toto = ex;
            }

            return new Response<List<SlotDetails>>
            {
                Status = 200,
                Message = "Créneaux récupérés avec succès",
                Data = slots.Select(s => new SlotDetails(s)).ToList(),
                Count = slots.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<SlotDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des créneaux: {ex.Message}",
                Data = null,
            };
        }
    }

    public async Task<Response<List<SlotDetails>>> GetSlotsByStudentWithTeacherAsync(
        Guid teacherId,
        DateTimeOffset dateFrom,
        DateTimeOffset dateTo,
        ClaimsPrincipal principals
    )
    {
        try
        {
            var student = CheckUser.GetUserFromClaim(principals, context);
            var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);

            if (teacher == null || student is null)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 404,
                    Message = "Enseignant non trouvé",
                    Data = null,
                };
            }

            if (dateFrom >= dateTo)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 400,
                    Message = "La date de fin doit être postérieure à la date de début",
                    Data = null,
                };
            }
            var minDateFrom = dateFrom > DateTime.Now ? dateFrom : DateTime.Now;
            var slots = await context
                .Slots.AsNoTracking()
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Type)
                .Include(s =>
                    s.Reservations.Where(r =>
                        r.ArchivedAt == null
                        && (
                            r.StatusId == HardCode.RESERVATION_ACCEPTED
                            || r.StatusId == HardCode.RESERVATION_PENDING
                        )
                    )
                )
                .Where(s =>
                    s.TeacherId == teacherId
                    && s.ArchivedAt == null
                    && s.DateFrom >= minDateFrom
                    && s.DateTo <= dateTo
                    && (s.Reservations == null || !s.Reservations.Any()) // Seulement les créneaux disponibles
                )
                .OrderBy(s => s.DateFrom)
                .Select(s => new SlotDetails(s))
                .ToListAsync();

            var studentReservations = await context
                .Slots.Include(s =>
                    s.Reservations.Where(r =>
                        r.ArchivedAt == null
                        && (
                            r.StatusId == HardCode.RESERVATION_ACCEPTED
                            || r.StatusId == HardCode.RESERVATION_PENDING
                        )
                    )
                )
                    .ThenInclude(r => r.Product)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Status)
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Type)
                .Where(s => s.Reservations.Any(r => r.StudentId == student.Id))
                .Select(s => new SlotDetails(s))
                .ToListAsync();

            slots.AddRange(studentReservations);

            return new Response<List<SlotDetails>>
            {
                Status = 200,
                Message = "Cr�neaux disponibles r�cup�r�s avec succ�s",
                Data = slots,
                Count = slots.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<SlotDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration des cr�neaux: {ex.Message}",
                Data = null,
            };
        }
    }

    public async Task<Response<List<SlotDetails>>> GetReservationByStudentAsync(
        DateTimeOffset dateFrom,
        DateTimeOffset dateTo,
        ClaimsPrincipal principals
    )
    {
        try
        {
            var student = CheckUser.GetUserFromClaim(principals, context);

            if (student is null)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 404,
                    Message = "Enseignant non trouv�",
                    Data = null,
                };
            }

            if (dateFrom >= dateTo)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 400,
                    Message = "La date de fin doit �tre post�rieure � la date de d�but",
                    Data = null,
                };
            }
            var slots = await context
                .Slots.Include(s =>
                    s.Reservations.Where(r =>
                        r.ArchivedAt == null
                        && (
                            r.StatusId == HardCode.RESERVATION_ACCEPTED
                            || r.StatusId == HardCode.RESERVATION_PENDING
                        )
                    )
                )
                .Where(s => s.Reservations != null)
                .Include(s =>
                    s.Reservations.Where(r =>
                        r.ArchivedAt == null
                        && (
                            r.StatusId == HardCode.RESERVATION_ACCEPTED
                            || r.StatusId == HardCode.RESERVATION_PENDING
                        )
                    )
                )
                    .ThenInclude(r => r.Status)
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Type)
                .Where(s => s.Reservations.Any(r => r.StudentId == student.Id))
                .Select(s => new SlotDetails(s))
                .ToListAsync();

            return new Response<List<SlotDetails>>
            {
                Status = 200,
                Message = "Créneaux disponibles récupérés avec succès",
                Data = slots,
                Count = slots.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<SlotDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des créneaux: {ex.Message}",
                Data = null,
            };
        }
    }

    public async Task<Response<List<ReservationDetails>>> GetReservationByStudentPaginated(
        CustomTableState query,
        ClaimsPrincipal principals
    )
    {
        try
        {
            var student = CheckUser.GetUserFromClaim(principals, context);

            if (student is null)
            {
                return new Response<List<ReservationDetails>>
                {
                    Status = 404,
                    Message = "Étudiant non trouvé",
                    Data = null,
                };
            }

            IQueryable<Reservation> queryable = context.Reservations.Where(r =>
                r.StudentId == student.Id && r.ArchivedAt == null
            );

            IQueryable<Reservation> reservations = queryable
                .Include(r => r.Slot)
                    .ThenInclude(s => s.Teacher)
                        .ThenInclude(t => t.User)
                .Include(r => r.Slot)
                    .ThenInclude(s => s.Type)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .Include(r => r.Status);

            if (query.Filters.TryGetValue("teacherName", out Filter teacherName))
            {
                reservations = reservations.Where(r =>
                    EF.Functions.ILike(
                        r.Slot.Teacher.User.FirstName.ToLower()
                            + " "
                            + r.Slot.Teacher.User.LastName.ToLower(),
                        $"%{teacherName.Value.ToString().Trim()}%"
                    )
                );
            }

            // global filter
            if (!string.IsNullOrEmpty(query.Search?.Trim()))
            {
                var trimmed = query.Search.Trim();
                reservations = reservations.Where(r =>
                    EF.Functions.ILike(r.Description, $"%{trimmed}%")
                    || EF.Functions.ILike(r.Title, $"%{trimmed}%")
                );
            }

            // count
            var count = await reservations.CountAsync();

            // pagination
            if (query.Rows == 0 || query.Rows < 0)
            {
                query.Rows = 10;
            }
            reservations = reservations
                .OrderByDescending(r => r.Slot.DateFrom)
                .Skip(query.First)
                .Take(query.Rows);

            return new Response<List<ReservationDetails>>
            {
                Status = 200,
                Message = "Réservations récupérées avec succès",
                Data = reservations.Select(r => new ReservationDetails(r)).ToList(),
                Count = count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<ReservationDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des réservations: {ex.Message}",
                Data = null,
            };
        }
    }

    public async Task<Response<List<ReservationDetails>>> GetReservationByTeacherPaginated(
        CustomTableState query,
        ClaimsPrincipal principals
    )
    {
        try
        {
            var teacher = CheckUser.GetUserFromClaim(principals, context);

            if (teacher is null)
            {
                return new Response<List<ReservationDetails>>
                {
                    Status = 404,
                    Message = "Étudiant non trouvé",
                    Data = null,
                };
            }

            IQueryable<Reservation> queryable = context.Reservations.Where(r =>
                r.Slot.TeacherId == teacher.Id && r.ArchivedAt == null
            );

            IQueryable<Reservation> reservations = queryable
                .Include(r => r.Slot)
                    .ThenInclude(s => s.Teacher)
                        .ThenInclude(t => t.User)
                .Include(r => r.Slot)
                    .ThenInclude(s => s.Type)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .Include(r => r.Status);

            if (query.Filters.TryGetValue("status", out Filter statuses))
            {
                var ids = JsonSerializer.Deserialize<Guid[]>(statuses.Value?.ToString() ?? "[]");
                if (ids != null && ids.Length > 0)
                {
                    reservations = reservations.Where(r => ids.Contains(r.StatusId));
                }
            }

            if (query.Filters.TryGetValue("studentName", out Filter studentName))
            {
                reservations = reservations.Where(r =>
                    EF.Functions.ILike(
                        r.Student.User.FirstName.ToLower()
                            + " "
                            + r.Student.User.LastName.ToLower(),
                        $"%{studentName.Value.ToString().Trim()}%"
                    )
                );
            }

            // global filter
            if (!string.IsNullOrEmpty(query.Search?.Trim()))
            {
                var trimmed = query.Search.Trim();
                reservations = reservations.Where(r =>
                    EF.Functions.ILike(r.Description, $"%{trimmed}%")
                    || EF.Functions.ILike(r.Title, $"%{trimmed}%")
                );
            }

            // count
            var count = await reservations.CountAsync();

            // pagination
            if (query.Rows == 0 || query.Rows < 0)
            {
                query.Rows = 10;
            }
            reservations = reservations.Skip(query.First).Take(query.Rows);

            return new Response<List<ReservationDetails>>
            {
                Status = 200,
                Message = "Réservations récupérées avec succès",
                Data = reservations.Select(r => new ReservationDetails(r)).ToList(),
                Count = count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<ReservationDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des réservations: {ex.Message}",
                Data = null,
            };
        }
    }

    // reservations
    public async Task<Response<ReservationDetails>> GetReservationById(Guid reservationId)
    {
        try
        {
            var reservation = await context
                .Reservations.Where(r => r.Id == reservationId)
                .Include(r => r.Slot)
                    .ThenInclude(s => s.Teacher)
                        .ThenInclude(t => t.User)
                .Include(r => r.Slot)
                    .ThenInclude(s => s.Type)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .Include(r => r.Status)
                .FirstOrDefaultAsync();

            return new Response<ReservationDetails>
            {
                Status = 200,
                Message = "Réservations récupérées avec succès",
                Data = new ReservationDetails(reservation),
            };
        }
        catch (Exception ex)
        {
            return new Response<ReservationDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la récupération des réservations: {ex.Message}",
                Data = null,
            };
        }
    }

    public async Task<Response<ReservationDetails>> BookSlotAsync(
        ReservationCreate reservationCreate,
        ClaimsPrincipal principals
    )
    {
        var student = CheckUser.GetUserFromClaim(principals, context);
        if (student is null)
        {
            return new Response<ReservationDetails>
            {
                Status = 403,
                Message = "Utilisateur non trouvé",
            };
        }
        var slot = await context
            .Slots.Include(s =>
                s.Reservations.Where(r =>
                    r.ArchivedAt == null
                    && (
                        r.StatusId == HardCode.RESERVATION_ACCEPTED
                        || r.StatusId == HardCode.RESERVATION_PENDING
                    )
                )
            )
            .Include(s => s.Teacher.User)
            .FirstOrDefaultAsync(s => s.Id == reservationCreate.SlotId && s.ArchivedAt == null);
        if (slot is null || (slot.Reservations != null && slot.Reservations.Any()))
        {
            return new Response<ReservationDetails>
            {
                Status = 404,
                Message = "Créneau non trouvé ou déjà réservé",
                Data = null,
            };
        }
        try
        {
            var orderId = await GetActiveOrderId(student.Id, slot.TeacherId);

            Reservation newReservation = new Reservation(reservationCreate, orderId);
            await context.Reservations.AddAsync(newReservation);

            await context.SaveChangesAsync();
            
            var notif = new Notification
            {
                Message = $"Nouvelle reservation en attente de validation",
                Name = nameof(NotificationTypeEnum.Reservation),
                Color = "#ffabcf",
                Icon = "pi pi-plus",
                IsSeen = false,
                UserId = slot.TeacherId
            };
            context.Notifications.Add(notif);
            await context.SaveChangesAsync();

            signalRService.SendMessageByUserEmail(slot.Teacher.User.Email, nameof(SignalRNotificationTypeEnum.Notification), new {Type = nameof(NotificationTypeEnum.Reservation), Message= "New reservation créé"});

            return new Response<ReservationDetails>
            {
                Status = 201,
                Message = "Réservation créé avec succès",
                Data = new ReservationDetails(newReservation),
            };
        }
        catch (Exception ex)
        {
            return new Response<ReservationDetails>
            {
                Status = 404,
                Message = ex.Message,
                Data = null,
            };
        }
    }

    public async Task UpdateReservationStatusAsync(ReservationUpdateStatus updateStatus)
    {
        var reservation = await context
            .Reservations.Include(r => r.Status)
            .Include(r => r.Student.User)
            .FirstOrDefaultAsync(r => r.Id == updateStatus.ReservationId);

        if (reservation is null)
        {
            throw new Exception("Réservation non trouvée");
        }

        var newStatus = await context.StatusReservations.FirstOrDefaultAsync(s =>
            s.Code == updateStatus.StatusCode
        );

        if (newStatus is null)
        {
            throw new Exception("Nouveau statut non trouvé");
        }

        reservation.StatusId = newStatus.Id;
        reservation.UpdatedAt = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync();
        
        var notif = new Notification
        {
            Message = $"Une reservation vient d'être valider",
            Name = nameof(NotificationTypeEnum.Reservation),
            Color = "#ffabcf",
            Icon = "pi pi-pencil",
            IsSeen = false,
            UserId = reservation.StudentId
        };
        context.Notifications.Add(notif);
        await context.SaveChangesAsync();

        signalRService.SendMessageByUserEmail(reservation.Student.User.Email, nameof(SignalRNotificationTypeEnum.Notification), new {Type = nameof(NotificationTypeEnum.Reservation), Message= "Reservation mise à jour"});
    }

    public async Task RemoveReservationByTeacherAsync(
        Guid reservationId,
        ClaimsPrincipal principals
    )
    {
        var teacher = CheckUser.GetUserFromClaim(principals, context);
        if (teacher is null)
        {
            throw new Exception("Utilisateur non trouvé");
        }
        var reservation = await context
            .Reservations.Where(r =>
                r.Id == reservationId
                && r.ArchivedAt == null
                && (
                    r.StatusId == HardCode.RESERVATION_PENDING
                    || r.StatusId == HardCode.RESERVATION_ACCEPTED
                )
            )
            .Include(r => r.Slot)
            .Include(r => r.Student.User)
            .Where(r => r.Slot.TeacherId == teacher.Id)
            .FirstOrDefaultAsync();

        if (reservation is null)
        {
            throw new Exception("Réservation non trouvée");
        }

        if (reservation.StatusId == HardCode.RESERVATION_ACCEPTED)
        {
            reservation.StatusId = HardCode.RESERVATION_CANCELLED;
            reservation.OrderId = null;
            reservation.ArchivedAt = DateTimeOffset.UtcNow;
            reservation.UpdatedAt = DateTimeOffset.UtcNow;
        }
        if (reservation.StatusId == HardCode.RESERVATION_PENDING)
        {
            reservation.StatusId = HardCode.RESERVATION_REJECTED;
            reservation.OrderId = null;
            reservation.ArchivedAt = DateTimeOffset.UtcNow;
            reservation.UpdatedAt = DateTimeOffset.UtcNow;
        }
        

        await context.SaveChangesAsync();
        
        var notif = new Notification
        {
            Message = $"Une reservation vient d'être rejetée",
            Name = nameof(NotificationTypeEnum.Reservation),
            Color = "#ffabcf",
            Icon = "pi pi-minus",
            IsSeen = false,
            UserId = reservation.StudentId
        };
        context.Notifications.Add(notif);
        await context.SaveChangesAsync();

        signalRService.SendMessageByUserEmail(reservation.Student.User.Email, nameof(SignalRNotificationTypeEnum.Notification), new {Type = nameof(NotificationTypeEnum.Reservation), Message= "Reservation rejetée"});
    }

    public async Task RemoveReservationByStudentAsync(
        Guid reservationId,
        ClaimsPrincipal principals
    )
    {
        var student = CheckUser.GetUserFromClaim(principals, context);
        if (student is null)
        {
            throw new Exception("Utilisateur non trouvé");
        }
        var reservation = await context
            .Reservations.Where(r =>
                r.Id == reservationId
                && r.ArchivedAt == null
                && r.StudentId == student.Id
                && (
                    r.StatusId == HardCode.RESERVATION_PENDING
                    || r.StatusId == HardCode.RESERVATION_ACCEPTED
                )
            )
            .Include(r => r.Slot)
            .ThenInclude(s => s.Teacher.User)
            .FirstOrDefaultAsync();

        if (reservation is null)
        {
            throw new Exception("Réservation non trouvée");
        }

        reservation.StatusId = HardCode.RESERVATION_PENDING;
        reservation.OrderId = null;
        reservation.ArchivedAt = DateTimeOffset.UtcNow;
        reservation.UpdatedAt = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync();
        
        var notif = new Notification
        {
            Message = $"Une reservation vient d'être Annulée",
            Name = nameof(NotificationTypeEnum.Reservation),
            Color = "#ffabcf",
            Icon = "pi pi-minus",
            IsSeen = false,
            UserId = reservation.Slot.TeacherId
        };
        context.Notifications.Add(notif);
        await context.SaveChangesAsync();

        signalRService.SendMessageByUserEmail(reservation.Slot.Teacher.User.Email, nameof(SignalRNotificationTypeEnum.Notification), new {Type = nameof(NotificationTypeEnum.Reservation), Message= "Reservation annulée"});
    }

    public async Task<Guid> GetActiveOrderId(Guid studentId, Guid teacherId)
    {
        try
        {
            var activeOrder = await context.Orders.FirstOrDefaultAsync(o =>
                o.StudentId == studentId
                && o.ArchivedAt == null
                && o.StatusId == HardCode.STATUS_ORDER_PENDING
                && o.TeacherId == teacherId
            );

            if (activeOrder is not null)
            {
                return activeOrder.Id;
            }

            var orderId = Guid.NewGuid();
            var newOrder = new Order
            {
                Id = orderId,
                OrderNumber = GenerateOrderNumberAsync(),
                StudentId = studentId,
                OrderDate = DateTime.UtcNow,
                TeacherId = teacherId,
                TotalAmount = 0m,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            context.Orders.Add(newOrder);
            await context.SaveChangesAsync();
            return orderId;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string GenerateOrderNumberAsync()
    {
        string datePart = DateTime.UtcNow.ToString("yyyyMMdd");

        //int count = await context.Orders.CountAsync(o =>
        //    o.CreatedAt.Value.Date == DateTimeOffset.UtcNow.Date
        //);
        //int nextNumber = count + 1;
        var id = Guid.NewGuid().ToString()[..8];

        return $"BP-{datePart}-{id:D8}";
    }
}
