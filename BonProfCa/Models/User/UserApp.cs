using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using BonProfCa.Models;
using Microsoft.AspNetCore.Identity;
using BonProfCa.Models;
using BonProfCa.Models.Interfaces;
using BonProfCa.Utilities;

namespace BonProfCa.Models;

public class UserApp : IdentityUser<Guid>, IArchivable, IUpdateable, ICreatable
{
    [Required]
    [MaxLength(64)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public required DateTimeOffset DateOfBirth { get; set; }

    [MaxLength(500)]
    public string? ImgUrl { get; set; }
    
    public string? Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public required bool DataProcessingConsent { get; set; } = false;

    [Required]
    public required bool PrivacyPolicyConsent { get; set; } = false;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

    // Status account
    [Required]
    [ForeignKey(nameof(Status))]
    public Guid StatusId { get; set; }
    public StatusAccount? Status { get; set; }
    


    //gender
    [Required]
    [ForeignKey(nameof(Gender))]
    public Guid GenderId { get; set; }
    public Gender? Gender { get; set; }

    // notifications
    public ICollection<Notification> Notifications { get; set; }

    // navigation to teacher / student
    public Teacher? Teacher { get; set; }
    public Student? Student { get; set; }
    // roles
    public ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } = new List<IdentityUserRole<Guid>>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Formation> Formations { get; set; } = new List<Formation>();
    public ICollection<Language> Languages { get; set; } = new List<Language>();

    [SetsRequiredMembers]
    public UserApp() { }

    [SetsRequiredMembers]
    public UserApp(UserCreate newUser)
    {
        UserName = newUser.Email;
        Email = newUser.Email;
        FirstName = newUser.FirstName;
        Title = newUser.Title;
        Description =  newUser.Description;
        LastName = newUser.LastName;
        DateOfBirth = newUser.DateOfBirth;
        GenderId  = newUser.GenderId;
        DataProcessingConsent = true;
        PrivacyPolicyConsent = true;
        StatusId = HardCode.ACCOUNT_PENDING;
    }
}
