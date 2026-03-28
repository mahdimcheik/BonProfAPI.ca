using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models;

public class LanguageDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Color { get; set; }

    public LanguageDetails(Language language)
    {
        Id = language.Id;
        Name = language.Name;
        Color = language.Color;
    }
}

public class LanguageCreate
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Color { get; set; }
}
public class LanguageUpdate
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Color { get; set; }
}

/// <summary>
/// DTO pour associer/dissocier une langue à un utilisateur
/// </summary>
public class TeacherLanguageCreate
{
    /// <summary>
    /// Identifiant de l'utilisateur
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440001</example>
    [Required(ErrorMessage = "L'identifiant utilisateur est requis")]
    public Guid TeacherId { get; set; }

    /// <summary>
    /// Identifiant de la langue
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440002</example>
    [Required(ErrorMessage = "L'identifiant de la langue est requis")]
    public Guid LanguageId { get; set; }
}
