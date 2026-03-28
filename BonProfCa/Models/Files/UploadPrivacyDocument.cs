using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models.Files
{
    public class UploadPrivacyDocument
    {
        [Required]
        public required string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public Guid TypeId { get; set; }

        [Required]
        public required IFormFile File { get; set; }
    }
}
