using BonProfCa.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonProfCa.Models;

public class TeacherWalletTransaction : BaseModel
{
    [Required]
    public decimal Amount { get; set; }
    [Required]
    [ForeignKey(nameof(Type))]
    public Guid TypeId { get; set; }
    public TypeTeacherTransaction? Type { get; set; }
    public Guid? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }
}
