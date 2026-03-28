namespace BonProfCa.Models;

public class FileUrl
{
    public string Url { get; set; }
}

public class FileInfoResponse
{
    public required string Name { get; set; }
    public required string Url { get; set; }
    public DateTimeOffset? UploadDate { get; set; }
}
