using System.ComponentModel.DataAnnotations;

namespace BonProfCa.Models;

public class GenderDetails(Gender gender)
{
    public Guid Id => gender.Id;
    public string Name => gender.Name;
    public string Color => gender.Color;
    public string? Icon => gender.Icon;
}
