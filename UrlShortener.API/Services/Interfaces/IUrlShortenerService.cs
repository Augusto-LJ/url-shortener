using UrlShortener.API.Models.Request;

namespace UrlShortener.API.Services.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<string> CreateUniqueSlugAsync();
        Task SaveShortUrlAsync(string originalUrl, string slug);
        Task<string?> GetOriginalUrlAsync(string slug);
    }
}
