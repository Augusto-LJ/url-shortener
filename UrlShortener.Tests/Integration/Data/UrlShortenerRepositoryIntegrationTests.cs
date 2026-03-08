using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Contexts;
using UrlShortener.API.Data;
using UrlShortener.API.Models.Entities;
using UrlShortener.Tests.Shared.Fixtures;

namespace UrlShortener.Tests.Integration.Data;

public class UrlShortenerRepositoryIntegrationTests
{
    private UrlShortenerRepository CreateRepository(ApplicationDbContext context)
    {
        return new UrlShortenerRepository(context);
    }

    #region SlugExistsAsync

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SlugExistsAsync_SlugExists_ReturnsTrue()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateRepository(context);
        const string slug = "existing";

        context.ShortUrls.Add(new ShortUrl("https://example.com", slug));
        await context.SaveChangesAsync();

        // Act
        var result = await sut.SlugExistsAsync(slug);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SlugExistsAsync_SlugDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateRepository(context);
        const string slug = "noexists";

        // Act
        var result = await sut.SlugExistsAsync(slug);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region SaveShortUrlAsync

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SaveShortUrlAsync_ValidData_PersistsEntity()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateRepository(context);
        const string originalUrl = "https://www.google.com";
        const string slug = "aBc123Xy";
        var shortUrl = new ShortUrl(originalUrl, slug);

        // Act
        await sut.SaveShortUrlAsync(shortUrl);
        var persistedEntity = await context.ShortUrls.FirstOrDefaultAsync(x => x.Slug == slug);

        // Assert
        persistedEntity.Should().NotBeNull();
        persistedEntity!.OriginalUrl.Should().Be(originalUrl);
        persistedEntity.Slug.Should().Be(slug);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SaveShortUrlAsync_DuplicateSlug_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = DbContextFixture.CreateSqliteInMemoryContext();
        var sut = CreateRepository(context);
        const string slug = "duplicat";

        var firstUrl = new ShortUrl("https://first.com", slug);
        var secondUrl = new ShortUrl("https://second.com", slug);

        await sut.SaveShortUrlAsync(firstUrl);

        // Act
        var act = async () => await sut.SaveShortUrlAsync(secondUrl);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Slug collision detected.");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SaveShortUrlAsync_SetsCreatedAtAndExpiresAt()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateRepository(context);
        const string slug = "withDate";
        var beforeSave = DateTime.UtcNow;
        var shortUrl = new ShortUrl("https://example.com", slug);

        // Act
        await sut.SaveShortUrlAsync(shortUrl);
        var entity = await context.ShortUrls.FirstAsync(x => x.Slug == slug);

        // Assert
        entity.CreatedAt.Should().BeOnOrAfter(beforeSave);
        entity.ExpiresAt.Should().BeAfter(entity.CreatedAt);
    }

    #endregion

    #region GetOriginalUrlBySlugAsync

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetOriginalUrlBySlugAsync_SlugExists_ReturnsShortUrl()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateRepository(context);
        const string originalUrl = "https://www.google.com";
        const string slug = "aBc123Xy";

        context.ShortUrls.Add(new ShortUrl(originalUrl, slug));
        await context.SaveChangesAsync();

        // Act
        var result = await sut.GetOriginalUrlBySlugAsync(slug);

        // Assert
        result.Should().NotBeNull();
        result!.OriginalUrl.Should().Be(originalUrl);
        result.Slug.Should().Be(slug);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetOriginalUrlBySlugAsync_SlugDoesNotExist_ReturnsNull()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateRepository(context);
        const string slug = "noexists";

        // Act
        var result = await sut.GetOriginalUrlBySlugAsync(slug);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetOriginalUrlBySlugAsync_MultipleUrls_ReturnsCorrectOne()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateRepository(context);

        context.ShortUrls.AddRange(
            new ShortUrl("https://first.com", "first123"),
            new ShortUrl("https://second.com", "second12"),
            new ShortUrl("https://third.com", "third123")
        );
        await context.SaveChangesAsync();

        // Act
        var result = await sut.GetOriginalUrlBySlugAsync("second12");

        // Assert
        result.Should().NotBeNull();
        result!.OriginalUrl.Should().Be("https://second.com");
        result.Slug.Should().Be("second12");
    }

    #endregion
}
