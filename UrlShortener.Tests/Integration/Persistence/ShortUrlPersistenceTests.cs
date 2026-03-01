using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Contexts;
using UrlShortener.API.Services;

namespace UrlShortener.Tests.Integration.Persistence;

public class ShortUrlPersistenceTests
{
    private readonly ApplicationDbContext _db;
    private readonly UrlShortenerService _service;
    private readonly SlugGenerator _slugGenerator;

    public ShortUrlPersistenceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _db = new ApplicationDbContext(options);
        _slugGenerator = new SlugGenerator();
        _service = new UrlShortenerService(_db, _slugGenerator);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SaveShortUrlAsync_ValidData_PersistsEntity()
    {
        // Arrange
        const string originalUrl = "https://www.google.com";
        const string slug = "aBc123Xy";

        // Act
        await _service.SaveShortUrlAsync(originalUrl, slug);

        var persistedEntity = await _db.ShortUrls.FirstOrDefaultAsync();

        // Assert
        persistedEntity.Should().NotBeNull();
        persistedEntity!.OriginalUrl.Should().Be(originalUrl);
        persistedEntity.Slug.Should().Be(slug);
    }
}