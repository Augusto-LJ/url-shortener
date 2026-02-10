using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Contexts;
using UrlShortener.API.Models.Request;
using UrlShortener.API.Services;

namespace UrlShortener.Tests.Unit.Services;
public class UrlShortenerServiceUnitTests
{
    private readonly UrlShortenerService _service;

    public UrlShortenerServiceUnitTests()
    {
        var dummyContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

        _service = new UrlShortenerService(dummyContext);
    }

    [Theory]
    [Trait("Category", "Unit")]
    [InlineData("http://example.com")]
    [InlineData("https://example.com")]
    [InlineData("https://example.com/path")]
    [InlineData("https://example.com?x=1")]
    [InlineData("http://example.com:8080")]
    [InlineData("https://sub.example.com")]
    public void RequestDataIsValid_ValidUrls_ReturnsTrue(string url)
    {
        // Arrange
        var request = new ShortenRequest { Url = url };

        // Act
        var sut = _service;
        var result = sut.RequestDataIsValid(request);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [Trait("Category", "Unit")]
    [InlineData("ftp://example.com")]
    [InlineData("file:///c:/test")]
    [InlineData("mailto:test@test.com")]
    [InlineData("example.com")]
    [InlineData("/relative/path")]
    [InlineData("http://")]
    [InlineData("https://")]
    [InlineData("")]
    [InlineData("   ")]
    public void RequestDataIsValid_InvalidUrls_ReturnsFalse(string url)
    {
        // Arrange
        var request = new ShortenRequest { Url = url };

        // Act
        var sut = _service;
        var result = sut.RequestDataIsValid(request);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void RequestDataIsValid_NullUrl_ReturnsFalse()
    {
        // Arrange
        var request = new ShortenRequest { Url = null };

        // Act
        var sut = _service;
        var result = sut.RequestDataIsValid(request);

        // Assert
        result.Should().BeFalse();
    }
}
