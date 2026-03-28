using BonProfCa.Models.Interfaces;

namespace BonProfCa.Models;

public class CategoryCursus : BaseModelOption
{
    public ICollection<Cursus> Cursuses { get; set; } = new List<Cursus>();
}
