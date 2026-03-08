using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using UrlShortener.API.Contexts;
using UrlShortener.API.Models.Entities;

namespace UrlShortener.API.Data;

public class UrlShortenerRepository(ApplicationDbContext context) : IUrlShortenerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> SlugExistsAsync(string slug)
    {
        return await _context.ShortUrls
            .AsNoTracking()
            .AnyAsync(x => x.Slug == slug);
    }
    
    public async Task SaveShortUrlAsync(ShortUrl shortUrl)
    {
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
    
    public async Task<ShortUrl?> GetOriginalUrlBySlugAsync(string slug)
    {
        return await _context.ShortUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == slug);
    }

    private static bool IsUniqueViolation(DbUpdateException ex)
    {
        // Check for PostgreSQL unique constraint violation
        if (ex.InnerException is PostgresException pg &&
            pg.SqlState == PostgresErrorCodes.UniqueViolation)
            return true;

        // Check for SQLite unique constraint violation (error code 19)
        if (ex.InnerException is SqliteException sqlite &&
            sqlite.SqliteErrorCode == 19) // SQLITE_CONSTRAINT
            return true;

        return false;
    }
}
