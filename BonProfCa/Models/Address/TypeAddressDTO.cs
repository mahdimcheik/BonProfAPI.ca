using System.ComponentModel.DataAnnotations;
using BonProfCa.Models;

namespace BonProfCa.Models;

public class TypeAddressDetails
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }

    [Required]
    public string Color { get; set; }
    public string? Icon { get; set; }
    public string DisplayName { get; set; }
    public TypeAddressDetails()
    {
    }
    public TypeAddressDetails(TypeAddress typeAddress)
    {
        Id = typeAddress.Id;
        Name = typeAddress.Name;
        Color = typeAddress.Color;
        Icon = typeAddress.Icon;
        DisplayName = typeAddress.DisplayName;
    }
}
