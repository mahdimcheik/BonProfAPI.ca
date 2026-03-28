using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BonProfCa.Models.Interfaces;

namespace BonProfCa.Models;

[Table("Cursuses")]
public class Cursus : BaseModelOption
{
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [Required]
    [ForeignKey(nameof(Level))]
    public Guid LevelId { get; set; }
    public LevelCursus? Level { get; set; }
    
    [Required]
    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    
    public ICollection<CategoryCursus> Categories { get; set; } = new List<CategoryCursus>();

    public Cursus() { }

    public Cursus(CursusCreate newCursus, Guid teacherId, List<CategoryCursus> categories)
    {
        Name = newCursus.Name;
        Color = newCursus.Color;
        Icon = newCursus.Icon;
        Description = newCursus.Description;
        LevelId = newCursus.LevelId;
        TeacherId = teacherId;
        CreatedAt = DateTimeOffset.UtcNow;
        Categories = categories ?? [];
        UpdatedAt = null;
    }
}
