using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonProfCa.Models.Files
{
    public class PrivacyDocument : Document
    {
        [Required]
        public required string Title { get; set; }
        public string?  Description { get; set; }
        [ForeignKey(nameof(Type))]
        public Guid TypeId { get; set; }
        public PrivacyDocumentType Type { get; set; }
    }

    public class PrivacyDocumentDetails
    {
        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public string MimeType { get; set; } = string.Empty;

        public long Size { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public PrivacyDocumentTypeDetails Type { get; set; }
    }
}
