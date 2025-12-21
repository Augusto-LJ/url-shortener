using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Models.Entities;

namespace UrlShortener.API.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ShortUrl> ShortUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShortUrl>(entity =>
            {
                entity.Property(x => x.Slug)
                      .IsRequired()
                      .HasMaxLength(8);

                entity.HasIndex(x => x.Slug)
                      .IsUnique();
            });
        }
    }
}
