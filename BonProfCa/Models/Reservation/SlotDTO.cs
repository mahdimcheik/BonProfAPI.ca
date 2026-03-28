using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models;

public class SlotDetails
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public DateTimeOffset DateFrom { get; set; }

    [Required]
    public DateTimeOffset DateTo { get; set; }

    public TeacherDetails? Teacher { get; set; }

    [Required]
    public Guid TypeId { get; set; }

    public TypeSlotDetails? Type { get; set; }

    public ReservationDetails Reservation { get; set; }

    public SlotDetails() { }

    public SlotDetails(Slot slot)
    {
        Id = slot.Id;
        DateFrom = slot.DateFrom;
        DateTo = slot.DateTo;
        TypeId = slot.TypeId ?? Guid.Empty;

        if (slot.Reservations != null && slot.Reservations.Count > 0)
        {
            Reservation = new ReservationDetails(slot.Reservations.First());
        }

        if (slot.Teacher != null)
        {
            Teacher = new TeacherDetails(slot.Teacher);
        }

        if (slot.Type != null)
        {
            Type = new TypeSlotDetails(slot.Type);
        }
    }
}


public class SlotMinimalDetails
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public DateTimeOffset DateFrom { get; set; }
    [Required]
    public DateTimeOffset DateTo { get; set; }
    public TeacherDetails? Teacher { get; set; }
    [Required]
    public Guid TypeId { get; set; }
    public TypeSlotDetails? Type { get; set; }

    public SlotMinimalDetails() { }

    public SlotMinimalDetails(Slot slot)
    {
        Id = slot.Id;
        DateFrom = slot.DateFrom;
        DateTo = slot.DateTo;
        TypeId = slot.TypeId ?? Guid.Empty;

        if (slot.Teacher != null)
        {
            Teacher = new TeacherDetails(slot.Teacher);
        }

        if (slot.Type != null)
        {
            Type = new TypeSlotDetails(slot.Type);
        }
    }
}

public class SlotCreate
{
    [Required(ErrorMessage = "La date de début est requise")]
    public DateTimeOffset DateFrom { get; set; }
    [Required(ErrorMessage = "La date de fin est requise")]
    public DateTimeOffset DateTo { get; set; }
    [Required(ErrorMessage = "L'identifiant de l'enseignant est requis")]
    public Guid TeacherId { get; set; }
    [Required(ErrorMessage = "L'identifiant du type de créneau est requis")]
    public Guid TypeId { get; set; }
}

public class SlotUpdate
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "La date de début est requise")]
    public DateTimeOffset DateFrom { get; set; }

    [Required(ErrorMessage = "La date de fin est requise")]
    public DateTimeOffset DateTo { get; set; }

    [Required(ErrorMessage = "L'identifiant de l'enseignant est requis")]
    public Guid TeacherId { get; set; }

    [Required(ErrorMessage = "L'identifiant du type de créneau est requis")]
    public Guid TypeId { get; set; }

    public void UpdateSlot(Slot slot)
    {
        slot.DateFrom = DateFrom;
        slot.DateTo = DateTo;
        slot.TeacherId = TeacherId;
        slot.TypeId = TypeId;
        slot.UpdatedAt = DateTimeOffset.UtcNow;
    }
}
