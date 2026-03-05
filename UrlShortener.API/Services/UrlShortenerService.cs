using UrlShortener.API.Data;
using UrlShortener.API.Models.Entities;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Services
{
    public class UrlShortenerService(IUrlShortenerRepository repository,
                                     ISlugGenerator slugGenerator) : IUrlShortenerService
    {
        private readonly IUrlShortenerRepository _repository = repository;
        private readonly ISlugGenerator _slugGenerator = slugGenerator;

        public async Task<string> CreateUniqueSlugAsync()
        {
            const int maxAttempts = 10;

            for (int i = 0; i < maxAttempts; i++)
            {
                var slug = _slugGenerator.GenerateSlug();

                if (!await _repository.SlugExistsAsync(slug))
                    return slug;
            }

            throw new InvalidOperationException("Failed to generate a unique slug.");
        }

        public async Task SaveShortUrlAsync(string originalUrl, string slug)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(originalUrl);
            ArgumentException.ThrowIfNullOrWhiteSpace(slug);

            var shortUrl = new ShortUrl(originalUrl, slug);

            await _repository.SaveShortUrlAsync(shortUrl);
        }

        public async Task<string?> GetOriginalUrlAsync(string slug)
        {
            var shortUrl = await _repository.GetOriginalUrlBySlugAsync(slug);

            return shortUrl?.OriginalUrl;
        }
    }
}
