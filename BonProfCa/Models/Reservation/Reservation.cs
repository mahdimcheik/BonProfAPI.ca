using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using BonProfCa.Models;
using BonProfCa.Models.Interfaces;
using BonProfCa.Utilities;
using BonProfCa.Utilities;

namespace BonProfCa.Models;

[Table("Reservations")]
public class Reservation : BaseModel
{
    [Required]
    [StringLength(64)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(Slot))]
    public Guid SlotId { get; set; }
    public Slot? Slot { get; set; }

    [Required]
    [ForeignKey(nameof(Product))]
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    [ForeignKey(nameof(Status))]
    public Guid StatusId { get; set; } = HardCode.RESERVATION_PENDING;
    public StatusReservation? Status { get; set; }

    [ForeignKey(nameof(Order))]
    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }

    [Required]
    [ForeignKey(nameof(Student))]
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }

    public Reservation() { }

    [SetsRequiredMembers]
    public Reservation(ReservationCreate bookingCreate, Guid orderId)
    {
        Title = bookingCreate.Title;
        Description = bookingCreate.Description ?? "";
        SlotId = bookingCreate.SlotId;
        StudentId = bookingCreate.StudentId;
        ProductId = bookingCreate.ProductId;
        OrderId = orderId;
    }
}
