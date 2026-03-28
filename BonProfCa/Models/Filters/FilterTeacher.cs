namespace BonProfCa.Models.Filters;

public class FilterTeacher
{
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
    public string? FullName { get; set; }
    public string? CursusName { get; set; }
    public List<Guid> CategoryIds { get; set; }
    public List<Guid> LevelIds { get; set; }
    public int First { get; set; }
    public int? Row { get; set; }

    public double? Long { get; set; }
    public double? Lat { get; set; }
    public int Radius { get; set; } = 10;
}