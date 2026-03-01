namespace UrlShortener.API.Controllers.Interfaces;

public interface IUrlBuilder
{
    string? BuildShortUrl(string slug);
}
