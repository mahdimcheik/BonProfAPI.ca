using BonProfCa.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using BonProfCa.Models.Interfaces;


namespace BonProfCa.Models;

public class TypeAddress : BaseModelOption
{
    [MaxLength(64)]
    public string DisplayName { get; set; }
}
