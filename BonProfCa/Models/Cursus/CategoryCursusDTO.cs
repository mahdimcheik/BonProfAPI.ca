using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models;

/// <summary>
/// DTO pour l'affichage des informations d'une catégorie de cursus
/// </summary>
public class CategoryCursusDetails
{
    /// <summary>
    /// Identifiant unique de la catégorie de cursus
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Nom de la catégorie de cursus
    /// </summary>
    /// <example>Front-end</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Couleur associée à la catégorie de cursus (code hexadécimal)
    /// </summary>
    /// <example>#ff69b4</example>
    [Required]
    public string Color { get; set; }

    /// <summary>
    /// Icône associée à la catégorie de cursus
    /// </summary>
    /// <example>fa-code</example>
    public string? Icon { get; set; }

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

    public CategoryCursusDetails() { }

    public CategoryCursusDetails(CategoryCursus categoryCursus)
    {
        Id = categoryCursus.Id;
        Name = categoryCursus.Name;
        Color = categoryCursus.Color;
        Icon = categoryCursus.Icon;
        CreatedAt = categoryCursus.CreatedAt;
        UpdatedAt = categoryCursus.UpdatedAt;
    }
}

/// <summary>
/// DTO pour la création d'une nouvelle catégorie de cursus
/// </summary>
public class CategoryCursusCreate
{
    /// <summary>
    /// Nom de la catégorie de cursus
    /// </summary>
    /// <example>Front-end</example>
    [Required(ErrorMessage = "Le nom est requis")]
    [StringLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
    public string Name { get; set; }

    /// <summary>
    /// Couleur associée à la catégorie de cursus (code hexadécimal)
    /// </summary>
    /// <example>#ff69b4</example>
    [Required(ErrorMessage = "La couleur est requise")]
    [StringLength(16, ErrorMessage = "La couleur ne peut pas dépasser 16 caractères")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "La couleur doit être au format hexadécimal valide (ex: #ff69b4)")]
    public string Color { get; set; }

    /// <summary>
    /// Icône associée à la catégorie de cursus
    /// </summary>
    /// <example>fa-code</example>
    [StringLength(256, ErrorMessage = "L'icône ne peut pas dépasser 256 caractères")]
    public string? Icon { get; set; }
}

/// <summary>
/// DTO pour la mise à jour d'une catégorie de cursus existante
/// </summary>
public class CategoryCursusUpdate
{
    /// <summary>
    /// Nom de la catégorie de cursus
    /// </summary>
    /// <example>Front-end</example>
    [Required(ErrorMessage = "Le nom est requis")]
    [StringLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
    public string Name { get; set; }

    /// <summary>
    /// Couleur associée à la catégorie de cursus (code hexadécimal)
    /// </summary>
    /// <example>#ff69b4</example>
    [Required(ErrorMessage = "La couleur est requise")]
    [StringLength(16, ErrorMessage = "La couleur ne peut pas dépasser 16 caractères")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "La couleur doit être au format hexadécimal valide (ex: #ff69b4)")]
    public string Color { get; set; }

    /// <summary>
    /// Icône associée à la catégorie de cursus
    /// </summary>
    /// <example>fa-code</example>
    [StringLength(256, ErrorMessage = "L'icône ne peut pas dépasser 256 caractères")]
    public string? Icon { get; set; }
}
