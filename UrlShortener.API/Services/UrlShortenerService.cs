using Microsoft.EntityFrameworkCore;
using Npgsql;
using UrlShortener.API.Contexts;
using UrlShortener.API.Models.Entities;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Services
{
    public class UrlShortenerService(ApplicationDbContext context,
                                     ISlugGenerator slugGenerator) : IUrlShortenerService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ISlugGenerator _slugGenerator = slugGenerator;

        public async Task<string> CreateUniqueSlugAsync()
        {
            const int maxAttempts = 10;

            for (int i = 0; i < maxAttempts; i++)
            {
                var slug = _slugGenerator.GenerateSlug();

                if (!await _context.ShortUrls.AnyAsync(x => x.Slug == slug))
                    return slug;
            }

            throw new InvalidOperationException("Failed to generate a unique slug.");
        }

        public async Task SaveShortUrlAsync(string originalUrl, string slug)
        {
            if (string.IsNullOrWhiteSpace(originalUrl) || string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("OriginalUrl and slug must not be empty.");

            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                Slug = slug
            };

            _context.ShortUrls.Add(shortUrl);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                throw new InvalidOperationException("Slug collision detected.", ex);
            }
        }

        private static bool IsUniqueViolation(DbUpdateException ex)
        {
            return ex.InnerException is PostgresException pg
                   && pg.SqlState == PostgresErrorCodes.UniqueViolation;
        }

        public async Task<string?> GetOriginalUrlAsync(string slug)
        {
            var shortUrl = await _context.ShortUrls.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);

            return shortUrl?.OriginalUrl;
        }
    }
}
