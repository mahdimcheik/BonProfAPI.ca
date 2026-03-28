using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BonProfCa.Models.Interfaces;

namespace BonProfCa.Models;
public class Student: BaseModel
{
    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    
    public UserApp? User { get; set; }
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
