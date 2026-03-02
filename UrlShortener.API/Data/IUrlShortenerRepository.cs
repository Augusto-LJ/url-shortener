using UrlShortener.API.Models.Entities;

namespace UrlShortener.API.Data;

public interface IUrlShortenerRepository
{
    Task<bool> SlugExistsAsync(string slug);
    Task SaveShortUrlAsync(ShortUrl shortUrl);
    Task<ShortUrl?> GetOriginalUrlBySlugAsync(string slug);
}
