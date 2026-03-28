using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BonProfCa.Models;

/// <summary>
/// DTO pour la création d'un nouveau type de créneau
/// </summary>
public class TypeSlotCreate
{
    /// <summary>
    /// Nom du type de créneau
    /// </summary>
    /// <example>Cours individuel</example>
    [Required(ErrorMessage = "Le nom est requis")]
    [StringLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
    public required string Name { get; set; }

    /// <summary>
    /// Couleur associée au type de créneau (code hexadécimal)
    /// </summary>
    /// <example>#ff69b4</example>
    [Required(ErrorMessage = "La couleur est requise")]
    [StringLength(16, ErrorMessage = "La couleur ne peut pas dépasser 16 caractères")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "La couleur doit être au format hexadécimal valide (ex: #ff69b4)")]
    public required string Color { get; set; }

    /// <summary>
    /// Icône associée au type de créneau
    /// </summary>
    /// <example>fa-user</example>
    [StringLength(256, ErrorMessage = "L'icône ne peut pas dépasser 256 caractères")]
    public string? Icon { get; set; }
}

/// <summary>
/// DTO pour la mise à jour d'un type de créneau existant
/// </summary>
public class TypeSlotUpdate
{
    /// <summary>
    /// Identifiant unique du type de créneau
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    [Required]
    public Guid Id { get; set; }
    /// <summary>
    /// Nom du type de créneau
    /// </summary>
    /// <example>Cours individuel</example>
    [Required(ErrorMessage = "Le nom est requis")]
    [StringLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
    public required string Name { get; set; }

    /// <summary>
    /// Couleur associée au type de créneau (code hexadécimal)
    /// </summary>
    /// <example>#ff69b4</example>
    [Required(ErrorMessage = "La couleur est requise")]
    [StringLength(16, ErrorMessage = "La couleur ne peut pas dépasser 16 caractères")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "La couleur doit être au format hexadécimal valide (ex: #ff69b4)")]
    public required string Color { get; set; }

    /// <summary>
    /// Icône associée au type de créneau
    /// </summary>
    /// <example>fa-user</example>
    [StringLength(256, ErrorMessage = "L'icône ne peut pas dépasser 256 caractères")]
    public string? Icon { get; set; }

    public void UpdateTypeSlot(TypeSlot typeSlot)
    {
        typeSlot.Name = Name;
        typeSlot.Color = Color;
        typeSlot.Icon = Icon;
        typeSlot.UpdatedAt = DateTimeOffset.UtcNow;
    }
}

public class TypeSlotDetails
{
    /// <summary>
    /// Identifiant unique du type de créneau
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Nom du type de créneau
    /// </summary>
    /// <example>Cours individuel</example>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    /// Couleur associée au type de créneau (code hexadécimal)
    /// </summary>
    /// <example>#ff69b4</example>
    [Required]
    public required string Color { get; set; }

    /// <summary>
    /// Icône associée au type de créneau
    /// </summary>
    /// <example>fa-user</example>
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

    public TypeSlotDetails() { }

    [SetsRequiredMembers]
    public TypeSlotDetails(TypeSlot typeSlot)
    {
        Id = typeSlot.Id;
        Name = typeSlot.Name;
        Color = typeSlot.Color;
        Icon = typeSlot.Icon;
        CreatedAt = typeSlot.CreatedAt;
        UpdatedAt = typeSlot.UpdatedAt;
    }
}