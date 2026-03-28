using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonProfCa.Models.Interfaces;

public interface IArchivable
{
    public DateTimeOffset? ArchivedAt { get; set; }
}

public interface IUpdateable
{
    public DateTimeOffset? UpdatedAt { get; set; }
}

public interface ICreatable
{
    public DateTimeOffset CreatedAt { get; set; }
}

public abstract class BaseModel : IUpdateable, ICreatable, IArchivable
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [Column(TypeName = "timestamp with time zone")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset? UpdatedAt { get; set; }
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset? ArchivedAt { get; set; }
}

public class BaseModelOption : BaseModel
{
    [Required]
    [MaxLength(64)]
    public string Name { get; set; }

    [Required]
    [MaxLength(16)]
    public string Color { get; set; }
    
    [MaxLength(256)]
    public string? Icon { get; set; }
}
