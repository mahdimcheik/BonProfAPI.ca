using BonProfCa.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonProfCa.Models;

[Table("Experiences")]
public class Experience : BaseModel
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Company { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset DateFrom { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset? DateTo { get; set; }

    [Required]
    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    // Parameterless constructor for EF Core
    public Experience()
    {
    }
}
