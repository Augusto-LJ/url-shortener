using UrlShortener.API.Controllers.Interfaces;

namespace UrlShortener.API.Controllers;

public class UrlBuilder : IUrlBuilder
{
    public string? BuildShortUrl(string slug, HttpRequest request)
    {
        return $"{request.Scheme}://{request.Host}/{slug}";
    }
}
