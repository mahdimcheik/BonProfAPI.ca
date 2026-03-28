using BonProfCa.Models;
using BonProfCa.Models;
using BonProfCa.Models;
using BonProfCa.Models.Interfaces;
using BonProfCa.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class UserDetails
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }
    
    public required DateTimeOffset DateOfBirth { get; set; }
    
    public required string FirstName { get; set; } 

    public required string LastName { get; set; } 
    
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ImgUrl { get; set; }


    public StatusAccountDetails? Status { get; set; }
    public GenderDetails? Gender { get; set; }
    
    [Required]
    public required ICollection<RoleDetails>? Roles { get; set; }
    public  required ICollection<AddressDetails> Addresses { get; set; } 
    public  required ICollection<FormationDetails> Formations { get; set; }
    public required ICollection<LanguageDetails> Languages { get; set; } 

    public TeacherDetails? Teacher { get; set; }
    public StudentDetails? Student { get; set; }

    public UserDetails()
    {
    }
    [SetsRequiredMembers]
    public UserDetails(UserApp user, List<RoleDetails>? roles)
    {
        Id = user.Id;
        Email = user.Email ?? user.UserName ?? "";
        FirstName = user.FirstName;
        LastName = user.LastName;
        Title = user.Title;
        Description = user.Description;
        DateOfBirth = user.DateOfBirth;
        ImgUrl = user.ImgUrl;
        
        Roles = roles;
        Status = user.Status is not null ?  new StatusAccountDetails(user.Status) : null;
        Gender = user.Gender is not null ?  new GenderDetails(user.Gender) : null;
        Teacher = user.Teacher is not null ? new TeacherDetails(user.Teacher) : null;
        Student = user.Student is not null ? new StudentDetails(user.Student) : null;

        Formations = user.Formations?.Select(f => new FormationDetails(f)).ToList() ?? [];
        Addresses = user.Addresses?.Select(f => new AddressDetails(f)).ToList() ?? [];
        Languages = user.Languages?.Select(f => new LanguageDetails(f)).ToList() ?? [];
    }
    [SetsRequiredMembers]
    public UserDetails(UserApp user, bool forStudent)
    {
        Id = user.Id;
        Email = user.Email ?? user.UserName ?? "";
        FirstName = user.FirstName;
        LastName = user.LastName;
        Title = user.Title;
        Description = user.Description;
        DateOfBirth = user.DateOfBirth;
        ImgUrl = user.ImgUrl;
        
        Status = user.Status is not null ?  new StatusAccountDetails(user.Status) : null;
        Gender = user.Gender is not null ?  new GenderDetails(user.Gender) : null;
        Student = user.Student is not null ? new StudentDetails(user.Student) : null;
    }
}

public class UserMinimalDetails
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }
    
    public required string FirstName { get; set; } 

    public required string LastName { get; set; } 

    public string? ImgUrl { get; set; }


    public UserMinimalDetails()
    {
    }

}


public class UserLogin
{
    /// <summary>
    /// Adresse email de l'utilisateur (format email valide requis)
    /// </summary>
    /// <example>utilisateur@exemple.com</example>
    [Required(ErrorMessage = "L'email est requis")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    public required string Email { get; set; }

    /// <summary>
    /// Mot de passe (minimum 8 caractères avec majuscules, minuscules, chiffres)
    /// </summary>
    /// <example>MonMotDePasse123!</example>
    [Required(ErrorMessage = "Le mot de passe est requis")]
    [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères")]
    public required string Password { get; set; }
}

public class ConfirmAccount
{
    public required string UserId { get; set; }
    public required string ConfirmationToken { get; set; }
}

public class UserCreate
{
    [Required]
    [MaxLength(64)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(200, ErrorMessage = "Le titre ne peut pas d�passer 200 caract�res")]
    public string? Title { get; set; }

    [StringLength(1000, ErrorMessage = "La description ne peut pas d�passer 1000 caract�res")]
    public string? Description { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }   

    [Required]
    public required bool DataProcessingConsent { get; set; } = false;
    [Required]
    public required bool PrivacyPolicyConsent { get; set; } = false;
    [Required]
    public Guid RoleId { get; set; } = HardCode.ROLE_STUDENT;

    [Required]
    public required DateTimeOffset DateOfBirth { get; set; }
    [Required]
    public Guid GenderId { get; set; }

    public TeacherCreate? Teacher { get; set; }
    public StudentCreate? Student { get; set; }
}

public class PasswordReset
{
    [Required]
    public required string ResetToken { get; set; } = string.Empty;

    [Required]
    public required string Email { get; set; } = string.Empty;

    [Required]
    public required Guid Id { get; set; }
}

public class ForgotPassword
{
    [Required(ErrorMessage = "Email required")]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
}

public class ChangePassword
{
    [Required]
    public required string OldPassword { get; set; }

    [Required]
    public required string NewPassword { get; set; }

    [Required]
    public required string NewPasswordConfirmation { get; set; }
}

public class PasswordRecovery
{
    [Required(ErrorMessage = "UserId required")]
    public required string UserId { get; set; }

    [Required(ErrorMessage = "ConfirmationToken required")]
    public required string ResetToken { get; set; }

    [Required(ErrorMessage = "Password required")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "PasswordConfirmation required")]
    public required string PasswordConfirmation { get; set; }
}

public class Login
{
    [Required]
    public required string Token { get; set; } = null!;

    [Required]
    public required string RefreshToken { get; set; } = null!;

    [Required]
    public required UserDetails User { get; set; } = null!;
}

public class UserUpdate
{
    [Required]
    [MaxLength(64)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(200, ErrorMessage = "Le titre ne peut pas d�passer 200 caractères")]
    public string? Title { get; set; }

    [StringLength(1000, ErrorMessage = "La description ne peut pas d�passer 1000 caractères")]
    public string? Description { get; set; }

    [Required]
    public required DateTimeOffset DateOfBirth { get; set; }
    [Required]
    public Guid GenderId { get; set; }
    public List<Guid> LanguagesIds { get; set; }
    public TeacherUpdate? Teacher { get; set; }
    public StudentUpdate? Student { get; set; }

    public void UpdateUser(UserApp user, List<Language> languages)
    {
        user.FirstName = FirstName;
        user.LastName = LastName;
        user.Title = Title;
        user.Description = Description;
        user.DateOfBirth =  DateOfBirth;
        user.GenderId = GenderId;
        user.Languages.Clear();
        user.Languages = languages.Where(l => LanguagesIds.Any(lid => l.Id == lid )).ToList();
        if (user.Teacher is not null && Teacher is not null) 
        {
            Teacher.UpdateTeacher(user.Teacher);
        }

        if (user.Student is not null && Student is not null)
        {
            Student.UpdateStudent(user.Student);
        }
    }
}

public class UserInfosWithtoken
{
    [Required]
    public required string Token { get; set; }

    [Required]
    public required UserDetails User { get; set; }
}