using BonProfCa.Models.Interfaces;

namespace BonProfCa.Models;

public class Language : BaseModelOption
{
    public ICollection<Teacher>? Teachers { get; set; }
}
