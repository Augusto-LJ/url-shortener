using UrlShortener.API.Models.Request;

namespace UrlShortener.API.Services
{
    public interface IUrlShortenerService
    {
        bool RequestDataIsValid(ShortenRequest request);
        Task<string?> CreateUniqueSlugAsync();
        Task SaveShortUrlAsync(string originalUrl, string slug);
    }
}
