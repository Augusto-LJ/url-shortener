using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Contexts;
using UrlShortener.API.Models.Request;
using UrlShortener.API.Services;

namespace UrlShortener.Tests.Unit.Services;
public class UrlShortenerServiceUnitTests
{
    private readonly UrlShortenerService _service;
    private readonly ApplicationDbContext _dummyContext;

    public UrlShortenerServiceUnitTests()
    {
        _dummyContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
        _service = new UrlShortenerService(_dummyContext);
    }

    #region RequestDataIsValid

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
        var sut = _service;

        // Act
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
        var sut = _service;

        // Act
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
        var sut = _service;

        // Act
        var result = sut.RequestDataIsValid(request);

        // Assert
        result.Should().BeFalse();
    }
    #endregion

    #region GenerateSlug
    /// <summary>
    /// Testable subclass that exposes the protected GenerateSlug method for testing.
    /// </summary>
    private class TestableUrlShortenerService(ApplicationDbContext context) : UrlShortenerService(context)
    {
        public string PublicGenerateSlug() => GenerateSlug();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GenerateSlug_ReturnsNonNullOrWhiteSpace()
    {
        // Arrange
        var sut = new TestableUrlShortenerService(_dummyContext);

        // Act
        var slug = sut.PublicGenerateSlug();

        // Assert
        slug.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GenerateSlug_WhenCalled_ReturnsEightCharacterSlug()
    {
        // Arrange
        var sut = new TestableUrlShortenerService(_dummyContext);

        // Act
        var slug = sut.PublicGenerateSlug();

        // Assert
        slug.Should().HaveLength(8);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GenerateSlug_WhenCalled_ReturnsSlugWithOnlyBase62Characters()
    {
        // Arrange
        var sut = new TestableUrlShortenerService(_dummyContext);

        // Act
        var slug = sut.PublicGenerateSlug();

        // Assert
        slug.Should().MatchRegex("^[a-zA-Z0-9]{8}$");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GenerateSlug_WhenCalledMoreThanOnce_ProducesDifferentSlugs()
    {
        // Arrange
        var sut = new TestableUrlShortenerService(_dummyContext);

        // Act
        var slug1 = sut.PublicGenerateSlug();
        var slug2 = sut.PublicGenerateSlug();

        // Assert
        slug1.Should().NotBe(slug2);
    }
    #endregion
}
