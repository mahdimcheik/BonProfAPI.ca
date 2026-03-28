namespace BonProfCa.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string? Token { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public Guid? UserId { get; set; }
    public UserApp? User { get; set; }
}
