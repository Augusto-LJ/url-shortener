using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Contexts;
using UrlShortener.API.Models.Entities;
using UrlShortener.API.Services;

namespace UrlShortener.Tests.Integration.Services
{
    public class UrlShortenerServiceIntegrationTests
    {
        public readonly SlugGenerator _slugGenerator = new();
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private ApplicationDbContext CreateSqliteInMemoryContext()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private UrlShortenerService CreateService(ApplicationDbContext context)
        {
            return new UrlShortenerService(context, _slugGenerator);
        }

        #region CreateUniqueSlugAsync

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CreateUniqueSlugAsync_SlugDoesNotExist_ReturnsSlug()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var sut = CreateService(context);

            // Act
            var slug = await sut.CreateUniqueSlugAsync();

            // Assert
            slug.Should().NotBeNullOrWhiteSpace();
            (await context.ShortUrls.AnyAsync(x => x.Slug == slug)).Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CreateUniqueSlugAsync_SlugAlreadyExists_GeneratesDifferentSlug()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var sut = CreateService(context);

            var existingSlug = await sut.CreateUniqueSlugAsync();
            context.ShortUrls.Add(new ShortUrl { OriginalUrl = "http://test.com", Slug = existingSlug });
            await context.SaveChangesAsync();

            // Act
            var newSlug = await sut.CreateUniqueSlugAsync();

            // Assert
            newSlug.Should().NotBeNullOrWhiteSpace();
            newSlug.Should().NotBe(existingSlug);
        }

        #endregion

        #region SaveShortUrlAsync
        [Fact]
        [Trait("Category", "Integration")]
        public async Task SaveShortUrlAsync_ValidData_PersistsEntity()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var sut = CreateService(context);
            const string originalUrl = "https://www.google.com";
            const string slug = "aBc123Xy";

            // Act
            await sut.SaveShortUrlAsync(originalUrl, slug);
            var persistedEntity = await context.ShortUrls.FirstOrDefaultAsync(x => x.Slug == slug);

            // Assert
            persistedEntity!.OriginalUrl.Should().Be(originalUrl);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task SaveShortUrlAsync_DuplicateSlug_ThrowsDbUpdateException()
        {
            // Arrange
            var context = CreateSqliteInMemoryContext();
            var sut = CreateService(context);
            const string slug = "duplicate";

            await sut.SaveShortUrlAsync("https://first.com", slug);

            // Act
            var act = async () => await sut.SaveShortUrlAsync("https://second.com", slug);

            // Assert
            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task SaveShortUrlAsync_WhenRequestContainCreatedAtAndExpiresAt_SetsDatesCorrectly()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var sut = CreateService(context);
            const string slug = "withDate";
            var beforeSave = DateTime.UtcNow;

            // Act
            await sut.SaveShortUrlAsync("https://example.com", slug);
            var entity = await context.ShortUrls.FirstAsync(x => x.Slug == slug);

            // Assert
            entity.CreatedAt.Should().BeOnOrAfter(beforeSave);
            entity.ExpiresAt.Should().BeAfter(entity.CreatedAt);
        }
        #endregion

        #region GetOriginalUrlAsync

        #endregion
    }
}
