using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models;

public class ConversationCreate
{
    [Required]
    [MaxLength(512, ErrorMessage = "longueur maximal ne dois pas depasser le 512 caractères.")]
    public string Content { get; set; } = string.Empty;

    [Required]
    public Guid ReservationId { get; set; }
    [Required]
    public Guid SenderId { get; set; }
}

public class ConversationDetails
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid ReservationId { get; set; }
    public Guid SenderId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
