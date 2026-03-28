using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models;

/// <summary>
/// DTO pour l'affichage des informations d'une formation
/// </summary>
public class FormationDetails
{

    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Institute { get; set; }


    [Required]
    public DateTimeOffset DateFrom { get; set; }


    public DateTimeOffset? DateTo { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; }


    public DateTimeOffset? UpdatedAt { get; set; }

    public FormationDetails() { }

    public FormationDetails(Formation formation)
    {
        Id = formation.Id;
        Title = formation.Title;
        Description = formation.Description;
        Institute = formation.Institute;
        DateFrom = formation.DateFrom;
        DateTo = formation.DateTo;
        CreatedAt = formation.CreatedAt;
        UpdatedAt = formation.UpdatedAt;
    }
}

public class FormationCreate
{

    [Required(ErrorMessage = "Le titre est requis")]
    [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
    public string Title { get; set; }


    [Required(ErrorMessage = "La description est requise")]
    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
    public string Description { get; set; }
  
    [Required(ErrorMessage = "L'institution est requise")]
    [StringLength(200, ErrorMessage = "L'institution ne peut pas dépasser 200 caractères")]
    public string Institute { get; set; }

    [Required(ErrorMessage = "La date de début est requise")]
    public DateTimeOffset DateFrom { get; set; }

    public DateTimeOffset? DateTo { get; set; }
}

public class FormationUpdate
{
    [Required(ErrorMessage = "L'identifiant est requis")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Le titre est requis")]
    [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
    public string Title { get; set; }

    [Required(ErrorMessage = "La description est requise")]
    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
    public string Description { get; set; }

    [Required(ErrorMessage = "L'institution est requise")]
    [StringLength(200, ErrorMessage = "L'institution ne peut pas dépasser 200 caractères")]
    public string Institute { get; set; }


    [Required(ErrorMessage = "La date de début est requise")]
    public DateTimeOffset DateFrom { get; set; }


    public DateTimeOffset? DateTo { get; set; }

}
