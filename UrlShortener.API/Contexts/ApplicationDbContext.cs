using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Models.Entities;

namespace UrlShortener.API.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}
