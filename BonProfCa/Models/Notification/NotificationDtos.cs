namespace BonProfCa.Models
{
    public class NotificationDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsSeen { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class FilterNotification
    {
        public bool? IsSeen { get; set; }
        public int First { get; set; } = 10;
        public int? Row { get; set; }
    }
}
