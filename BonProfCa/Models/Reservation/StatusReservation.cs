using System.ComponentModel.DataAnnotations;
using BonProfCa.Models.Interfaces;
using BonProfCa.Utilities;

namespace BonProfCa.Models;

public class StatusReservation : BaseModelOption
{
    [Required]
    public StatusReservationCode Code { get; set; }
}
