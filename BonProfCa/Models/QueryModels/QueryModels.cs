namespace BonProfCa.Models;

public class CustomTableState
{
    public int First { get; set; }
    public int Rows { get; set; }
    public List<SortCriterion>? Sorts { get; set; } = [];
    public Dictionary<string, Filter>? Filters { get; set; }
    public string? Search { get; set; }
}

public class SortCriterion
{
    public string Field { get; set; }
    public int Order { get; set; } // 1 asc, -1 desc
}

public class Filter
{
    public object Value { get; set; }
    public string MatchMode { get; set; }
    public bool SpecialFilter { get; set; }
}

