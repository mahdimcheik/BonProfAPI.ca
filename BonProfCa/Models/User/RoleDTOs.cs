using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BonProfCa.Models;

/// <summary>
/// DTO pour l'affichage détaillé d'un rôle
/// </summary>
public class RoleDetails
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
  
    public RoleDetails() { }


    [SetsRequiredMembers]   
    public RoleDetails(RoleApp role)
    {
        Id = role.Id;
        Name = role.Name ?? string.Empty;
        CreatedAt = role.CreatedAt;
        UpdatedAt = role.UpdatedAt;
        Color = role.Color;
    }
}
public class RoleAppCreate
{
    [Required(ErrorMessage = "Le nom du rôle est requis")]
    [StringLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
    public required string Name { get; set; }
    public required string Color { get; set; }
}

public class RoleAppUpdate
{  
    [Required(ErrorMessage = "Le nom du rôle est requis")]
    [StringLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
    public required string Name { get; set; }
    public required string Color { get; set; }

    public void UpdateRole(RoleApp role)
    {
        role.Name = Name;
        role.NormalizedName = Name.ToUpper();
        role.UpdatedAt = DateTimeOffset.UtcNow;
        role.Color = Color;
    }
}
