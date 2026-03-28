using BonProfCa.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonProfCa.Models;

public class TeacherPayout : BaseModel
{
    [Required]
    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset PaidAt { get; set; }
    [Required]
    [ForeignKey(nameof(Status))]
    public Guid StatusId { get; set; }
    public StatusTransaction? Status { get; set; }
}
