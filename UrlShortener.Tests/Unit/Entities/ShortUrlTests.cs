using FluentAssertions;
using UrlShortener.API.Models.Entities;

namespace UrlShortener.Tests.Unit.Entities;
public class ShortUrlTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public void ShortUrl_GivenUrlAndSlug_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const string expectedOriginalUrl = "https://www.example.com";
        const string expectedSlug = "abc123";

        // Act
        var shortUrl = new ShortUrl(expectedOriginalUrl, expectedSlug);

        // Assert
        shortUrl.OriginalUrl.Should().Be(expectedOriginalUrl);
        shortUrl.Slug.Should().Be(expectedSlug);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void ShortUrl_WhenCreated_ShouldSetCreatedAtCorrectly()
    {
        // Arrange
        const string originalUrl = "https://www.example.com";
        const string slug = "abc123";

        // Act
        var shortUrl = new ShortUrl(originalUrl, slug);

        // Assert
        shortUrl.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void ShortUrl_WhenCreated_ShouldSetExpiresAtOneYearAfterCreatedAt()
    {
        // Arrange
        const string originalUrl = "https://www.example.com";
        const string slug = "abc123";

        // Act
        var shortUrl = new ShortUrl(originalUrl, slug);

        // Assert
        shortUrl.ExpiresAt.Should().BeCloseTo(shortUrl.CreatedAt.AddYears(1), TimeSpan.FromSeconds(1));
    }
}
