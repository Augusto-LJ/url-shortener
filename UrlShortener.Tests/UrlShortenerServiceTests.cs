using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Contexts;
using UrlShortener.API.Services;

namespace UrlShortener.Tests;

public class UrlShortenerServiceTests
{
    private readonly ApplicationDbContext _db;
    private readonly UrlShortenerService _service;

    public UrlShortenerServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _db = new ApplicationDbContext(options);
        _service = new UrlShortenerService(_db);
    }

    [Fact]
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