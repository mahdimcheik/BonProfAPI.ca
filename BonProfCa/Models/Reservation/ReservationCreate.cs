using System.ComponentModel.DataAnnotations;
using BonProfCa.Models;
using BonProfCa.Utilities;

namespace BonProfCa.Models;

public class ReservationDetails(Reservation booking)
{
    [Required]
    public Guid Id => booking.Id;
    [Required]
    public string Title => booking.Title;
    [Required]
    public string Description => booking.Description;
    public StatusReservationDetails? Status =>
        booking.Status is not null ? new StatusReservationDetails(booking.Status) : null;
    public ProductDetails? Product =>
        booking.Product is not null ? new ProductDetails(booking.Product) : null;
    public StudentDetails Student =>
        booking.Student is not null ? new StudentDetails(booking.Student) : null;
    public SlotMinimalDetails Slot =>
    ( booking.Slot is not null) ? new SlotMinimalDetails(booking.Slot) : null;
}

public class ReservationCreate
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public Guid SlotId { get; set; }

    [Required]
    public Guid StudentId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public ReservationCreate() { }

    public ReservationCreate(Reservation reservation)
    {
        Title = reservation.Title;
        Description = reservation.Description;
        SlotId = reservation.SlotId;
        StudentId = reservation.StudentId;
        ProductId = reservation.ProductId;
    }
}

public class ReservationUpdate
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; }
    public string Description { get; set; }

    public void UpdateModel(Reservation reservation)
    {
        reservation.Title = Title;
        reservation.Description = Description;
    }
}

public class ReservationUpdateStatus
{
    [Required]
    public Guid ReservationId { get; set; }

    [Required]
    public StatusReservationCode StatusCode { get; set; }
}
