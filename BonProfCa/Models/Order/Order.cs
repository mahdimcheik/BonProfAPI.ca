using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BonProfCa.Models.Interfaces;
using BonProfCa.Utilities;
using BonProfCa.Utilities;

namespace BonProfCa.Models;

[Table("Orders")]
public class Order : BaseModel
{
    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset OrderDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal ReductionPercentage { get; set; } = 0m;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ReductionAmount { get; set; } = 0m;

    [Required]
    [ForeignKey(nameof(Status))]
    public Guid StatusId { get; set; } = HardCode.STATUS_ORDER_PENDING;

    public StatusOrder? Status { get; set; }

    [Required]
    [ForeignKey(nameof(Student))]
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }

    [Required]
    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [ForeignKey(nameof(Payment))]
    public Guid? PaymentId { get; set; }
    public Payment? Payment { get; set; }

    public Order() { }
}
