using BonProfCa.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models.Files
{
    public class PrivacyDocumentType : BaseModelOption
    {
        public required string DisplayName { get; set; }
    }

    public class PrivacyDocumentTypeDetails
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public required string DisplayName { get; set; }
    }
}
