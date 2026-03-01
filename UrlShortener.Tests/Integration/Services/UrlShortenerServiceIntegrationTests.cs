using FluentAssertions;
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

        #endregion

        #region GetOriginalUrlAsync

        #endregion
    }
}
