namespace UrlShortener.API.Services.Interfaces;

public interface IUrlValidator
{
    bool RequestDataIsValid(string? url);
}
