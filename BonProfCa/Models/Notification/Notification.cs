using BonProfCa.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models
{
    public class Notification: BaseModelOption    
    {
        [Required]
        public required string Message { get; set; }
        [Required]
        public bool IsSeen { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public UserApp? User { get; set; }
    }
}
