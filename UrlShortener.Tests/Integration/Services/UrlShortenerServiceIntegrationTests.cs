using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Contexts;
using UrlShortener.API.Data;
using UrlShortener.API.Models.Entities;
using UrlShortener.API.Services;
using UrlShortener.Tests.Shared.Fixtures;

namespace UrlShortener.Tests.Integration.Services;

public class UrlShortenerServiceIntegrationTests
{
    public readonly SlugGenerator _slugGenerator = new();

    private UrlShortenerService CreateService(ApplicationDbContext context)
    {
        return new UrlShortenerService(new UrlShortenerRepository(context), _slugGenerator);
    }

    #region CreateUniqueSlugAsync

    [Fact]
    [Trait("Category", "Integration")]
    public async Task CreateUniqueSlugAsync_SlugDoesNotExist_ReturnsSlug()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
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
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateService(context);

        var existingSlug = await sut.CreateUniqueSlugAsync();
        context.ShortUrls.Add(new ShortUrl("http://test.com", existingSlug));
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
        var context = DbContextFixture.CreateInMemoryContext();
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
        var context = DbContextFixture.CreateSqliteInMemoryContext();
        var sut = CreateService(context);
        const string slug = "duplicate";

        await sut.SaveShortUrlAsync("https://first.com", slug);

        // Act
        var act = async () => await sut.SaveShortUrlAsync("https://second.com", slug);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SaveShortUrlAsync_WhenRequestContainCreatedAtAndExpiresAt_SetsDatesCorrectly()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
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

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetOriginalUrlAsync_SlugExists_ReturnsOriginalUrl()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateService(context);
        const string originalUrl = "https://www.google.com";
        const string slug = "aBc123Xy";
        await sut.SaveShortUrlAsync(originalUrl, slug);

        // Act
        var result = await sut.GetOriginalUrlAsync(slug);

        // Assert
        result.Should().Be(originalUrl);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetOriginalUrlAsync_SlugDoesNotExist_ReturnsNull()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateService(context);
        const string slug = "nonexistent";

        // Act
        var result = await sut.GetOriginalUrlAsync(slug);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetOriginalUrlAsync_MultipleUrls_ReturnsCorrectOne()
    {
        // Arrange
        var context = DbContextFixture.CreateInMemoryContext();
        var sut = CreateService(context);
        await sut.SaveShortUrlAsync("https://first.com", "slug1");
        await sut.SaveShortUrlAsync("https://second.com", "slug2");

        // Act
        var result1 = await sut.GetOriginalUrlAsync("slug1");
        var result2 = await sut.GetOriginalUrlAsync("slug2");

        // Assert
        result1.Should().Be("https://first.com");
        result2.Should().Be("https://second.com");
    }

    #endregion
}
