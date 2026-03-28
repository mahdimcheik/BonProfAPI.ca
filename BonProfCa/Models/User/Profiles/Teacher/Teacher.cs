using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using BonProfCa.Models.Interfaces;

namespace BonProfCa.Models;

public class Teacher : BaseModel
{
    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public required UserApp? User { get; set; }
    public string? LinkedIn { get; set; }
    public string? FaceBook { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    [Required]
    public required decimal PriceIndicative { get; set; }
    [Required]
    public bool IsProfessionnal { get; set; } = false;
    public  long?  Siret { get; set; }

    public ICollection<Cursus> Cursuses { get; set; } = new List<Cursus>();
    public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
    public ICollection<Slot> Slots { get; set; } = new List<Slot>();

    [SetsRequiredMembers]
    public Teacher()
    {
    }
    
    [SetsRequiredMembers]
    public Teacher(TeacherCreate teacherCreate,Guid userId)
    {
        Id = userId;
        UserId = userId;
        IsProfessionnal = teacherCreate.IsProfessionnal;
        Siret = teacherCreate.Siret;
    }
}
