using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models;

/// <summary>
/// DTO pour l'affichage des informations d'un cursus
/// </summary>
public class CursusDetails
{
    /// <summary>
    /// Identifiant unique du cursus
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Nom du cursus
    /// </summary>
    /// <example>Formation Développement Web Full Stack</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Couleur associée au cursus (code hexadécimal)
    /// </summary>
    /// <example>#3498db</example>
    [Required]
    public string Color { get; set; }

    /// <summary>
    /// Icône associée au cursus
    /// </summary>
    /// <example>fa-code</example>
    public string? Icon { get; set; }

    /// <summary>
    /// Description détaillée du cursus
    /// </summary>
    /// <example>Formation complète en développement web moderne avec React et .NET</example>
    public string? Description { get; set; }


    /// <summary>
    /// Identifiant du niveau du cursus
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440001</example>
    [Required]
    public Guid LevelId { get; set; }

    /// <summary>
    /// Niveau du cursus
    /// </summary>
    public LevelCursusDetails? Level { get; set; }

    /// <summary>
    /// Identifiant de l'enseignant du cursus
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440002</example>
    [Required]
    public Guid TeacherId { get; set; }

    /// <summary>
    /// Informations de l'enseignant
    /// </summary>
    public Teacher? Teacher { get; set; }

    /// <summary>
    /// Liste des catégories associées au cursus
    /// </summary>
    public List<CategoryCursusDetails> Categories { get; set; } = new();

    /// <summary>
    /// Date de création de l'enregistrement
    /// </summary>
    /// <example>2023-01-15T10:30:00Z</example>
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Date de dernière mise à jour
    /// </summary>
    /// <example>2023-01-20T14:45:00Z</example>
    public DateTimeOffset? UpdatedAt { get; set; }

    public CursusDetails() { }

    public CursusDetails(Cursus cursus)
    {
        Id = cursus.Id;
        Name = cursus.Name;
        Color = cursus.Color;
        Icon = cursus.Icon;
        Description = cursus.Description;
        LevelId = cursus.LevelId;
        Level = cursus.Level != null ? new LevelCursusDetails(cursus.Level) : null;
        TeacherId = cursus.TeacherId;
        Categories = cursus.Categories?.Select(c => new CategoryCursusDetails(c)).ToList() ?? new List<CategoryCursusDetails>();
        CreatedAt = cursus.CreatedAt;
        UpdatedAt = cursus.UpdatedAt;
    }
}

/// <summary>
/// DTO pour la création d'un nouveau cursus
/// </summary>
public class CursusCreate
{
    /// <summary>
    /// Nom du cursus
    /// </summary>
    /// <example>Formation Développement Web Full Stack</example>
    [Required(ErrorMessage = "Le nom est requis")]
    [StringLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
    public string Name { get; set; }

    /// <summary>
    /// Couleur associée au cursus (code hexadécimal)
    /// </summary>
    /// <example>#3498db</example>
    [Required(ErrorMessage = "La couleur est requise")]
    [StringLength(16, ErrorMessage = "La couleur ne peut pas dépasser 16 caractères")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "La couleur doit être au format hexadécimal valide (ex: #3498db)")]
    public string Color { get; set; }

    /// <summary>
    /// Icône associée au cursus
    /// </summary>
    /// <example>fa-code</example>
    [StringLength(256, ErrorMessage = "L'icône ne peut pas dépasser 256 caractères")]
    public string? Icon { get; set; }

    /// <summary>
    /// Description détaillée du cursus
    /// </summary>
    /// <example>Formation complète en développement web moderne avec React et .NET</example>
    [StringLength(512, ErrorMessage = "La description ne peut pas dépasser 512 caractères")]
    public string? Description { get; set; }

    /// <summary>
    /// Identifiant du niveau du cursus
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440001</example>
    [Required(ErrorMessage = "Le niveau est requis")]
    public Guid LevelId { get; set; }

    /// <summary>
    /// Liste des identifiants des catégories à associer au cursus
    /// </summary>
    public List<Guid> CategoryIds { get; set; } = new();
}

/// <summary>
/// DTO pour la mise à jour d'un cursus existant
/// </summary>
public class CursusUpdate
{
    /// <summary>
    /// Identifiant unique du cursus
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    [Required]
    public Guid Id { get; set; }
    /// <summary>
    /// Nom du cursus
    /// </summary>
    /// <example>Formation Développement Web Full Stack</example>
    [Required(ErrorMessage = "Le nom est requis")]
    [StringLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
    public string Name { get; set; }

    /// <summary>
    /// Couleur associée au cursus (code hexadécimal)
    /// </summary>
    /// <example>#3498db</example>
    [Required(ErrorMessage = "La couleur est requise")]
    [StringLength(16, ErrorMessage = "La couleur ne peut pas dépasser 16 caractères")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "La couleur doit être au format hexadécimal valide (ex: #3498db)")]
    public string Color { get; set; }

    /// <summary>
    /// Icône associée au cursus
    /// </summary>
    /// <example>fa-code</example>
    [StringLength(256, ErrorMessage = "L'icône ne peut pas dépasser 256 caractères")]
    public string? Icon { get; set; }

    /// <summary>
    /// Description détaillée du cursus
    /// </summary>
    /// <example>Formation complète en développement web moderne avec React et .NET</example>
    [StringLength(512, ErrorMessage = "La description ne peut pas dépasser 512 caractères")]
    public string? Description { get; set; }

    /// <summary>
    /// Identifiant du niveau du cursus
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440001</example>
    [Required(ErrorMessage = "Le niveau est requis")]
    public Guid LevelId { get; set; }


    /// <summary>
    /// Liste des identifiants des catégories à associer au cursus
    /// </summary>
    public List<Guid> CategoryIds { get; set; } = new();

    public void UpdateCursus(Cursus cursus, List<CategoryCursus> categories)
    {
        cursus.Name = Name;
        cursus.Color = Color;
        cursus.Icon = Icon;
        cursus.Description = Description;
        cursus.LevelId = LevelId;
        cursus.UpdatedAt = DateTimeOffset.UtcNow;
        categories = categories ?? [];
    }
}


