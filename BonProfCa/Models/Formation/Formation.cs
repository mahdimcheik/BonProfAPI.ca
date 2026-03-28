using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using BonProfCa.Models.Interfaces;

namespace BonProfCa.Models;

[Table("Formations")]
public class Formation : BaseModel
{
    [Required]
    [MaxLength(200)]
    public required string Title { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string Institute { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public required string Description { get; set; }
    
    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public required DateTimeOffset DateFrom { get; set; }
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset? DateTo { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public UserApp? User { get; set; }

    public Formation() { }

    [SetsRequiredMembers]
    public Formation(FormationCreate newFormation, Guid userId)
    {
        Title = newFormation.Title;
        Institute = newFormation.Institute;
        Description = newFormation.Description;
        DateFrom = newFormation.DateFrom;
        DateTo = newFormation.DateTo;
        UserId = userId;
    }

    public void UpdateFormation(FormationUpdate formationUpdate)
    {
        Title = formationUpdate.Title;
        Institute = formationUpdate.Institute;
        Description = formationUpdate.Description;
        DateFrom = formationUpdate.DateFrom;
        DateTo = formationUpdate.DateTo;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
