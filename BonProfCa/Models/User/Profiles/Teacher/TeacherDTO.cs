using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BonProfCa.Models;

namespace BonProfCa.Models;

public class TeacherDetails
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
    public List<CursusDetails> Cursuses { get; set; }
    public string? LinkedIn { get; set; }
    public string? FaceBook { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    public decimal PriceIndicative { get; set; }
    public bool IsProfessionnal { get; set; } = false;
    public long? Siret { get; set; }
    public UserMinimalDetails? User { get; set; }

    public TeacherDetails() { }

    [SetsRequiredMembers]
    public TeacherDetails(Teacher teacher)
    {
        Id = teacher.Id;

        CreatedAt = teacher.CreatedAt;
        UpdatedAt = teacher.UpdatedAt;

        LinkedIn = teacher.LinkedIn;
        FaceBook = teacher.FaceBook;
        GitHub = teacher.GitHub;
        Twitter = teacher.Twitter;
        PriceIndicative = teacher.PriceIndicative;
        IsProfessionnal = teacher.IsProfessionnal;
        Siret = teacher.Siret;
        Cursuses = teacher.Cursuses.Select(c => new CursusDetails(c)).ToList();
        User =
            teacher.User != null
                ? new UserMinimalDetails
                {
                    Id = teacher.User.Id,
                    FirstName = teacher.User.FirstName,
                    LastName = teacher.User.LastName,
                    Email = teacher.User.Email ?? teacher.User.UserName ?? "",
                    
                }
                : null;
    }
}

/// <summary>
/// DTO pour la cr�ation d'un profil enseignant
/// </summary>
public class TeacherCreate {
    [Required]
    public bool IsProfessionnal { get; set; } = false;
    public long? Siret { get; set; }
}

public class TeacherUpdate
{
    public string? LinkedIn { get; set; }
    public string? FaceBook { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    public decimal PriceIndicative { get; set; }

    public void UpdateTeacher(Teacher teacher)
    {
        // teacher.Title = Title;
        // teacher.Description = Description;
        teacher.LinkedIn = LinkedIn;
        teacher.FaceBook = FaceBook;
        teacher.GitHub = GitHub;
        teacher.Twitter = Twitter;
        teacher.PriceIndicative = PriceIndicative;
    }
}
