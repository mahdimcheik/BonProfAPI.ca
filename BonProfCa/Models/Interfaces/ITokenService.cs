namespace BonProfCa.Services.Interfaces;

public interface ITokenService
{
    Task GetAsync(string serviceName);
}