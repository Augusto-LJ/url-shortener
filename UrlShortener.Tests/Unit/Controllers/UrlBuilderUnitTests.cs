using FluentAssertions;
using Microsoft.AspNetCore.Http;
using UrlShortener.API.Controllers;

namespace UrlShortener.Tests.Unit.Controllers;

public class UrlBuilderUnitTests
{
    private readonly UrlBuilder _sut;

    public UrlBuilderUnitTests()
    {
        _sut = new UrlBuilder();
    }

    #region BuildShortUrl
    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_ValidSlugAndHttpRequest_ReturnsFormattedShortUrl()
    {
        // Arrange
        const string slug = "abc123";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("localhost:5000");

        // Act
        var result = _sut.BuildShortUrl(slug, httpContext.Request);

        // Assert
        result.Should().Be("http://localhost:5000/abc123");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_HttpsScheme_ReturnsHttpsUrl()
    {
        // Arrange
        const string slug = "xyz789";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("example.com");

        // Act
        var result = _sut.BuildShortUrl(slug, httpContext.Request);

        // Assert
        result.Should().Be("https://example.com/xyz789");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_HostWithPort_IncludesPortInUrl()
    {
        // Arrange
        const string slug = "test123";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("api.example.com", 8443);

        // Act
        var result = _sut.BuildShortUrl(slug, httpContext.Request);

        // Assert
        result.Should().Be("https://api.example.com:8443/test123");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_DifferentSlugs_ReturnsDifferentUrls()
    {
        // Arrange
        const string slug1 = "abc123";
        const string slug2 = "xyz789";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("example.com");

        // Act
        var result1 = _sut.BuildShortUrl(slug1, httpContext.Request);
        var result2 = _sut.BuildShortUrl(slug2, httpContext.Request);

        // Assert
        result1.Should().NotBe(result2);
        result1.Should().EndWith("/abc123");
        result2.Should().EndWith("/xyz789");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_SingleCharacterSlug_ReturnsValidUrl()
    {
        // Arrange
        const string slug = "a";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("localhost");

        // Act
        var result = _sut.BuildShortUrl(slug, httpContext.Request);

        // Assert
        result.Should().Be("http://localhost/a");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_LongSlug_ReturnsValidUrl()
    {
        // Arrange
        const string slug = "abcdefghijklmnopqrstuvwxyz";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("example.com");

        // Act
        var result = _sut.BuildShortUrl(slug, httpContext.Request);

        // Assert
        result.Should().Be("https://example.com/abcdefghijklmnopqrstuvwxyz");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_SlugWithSpecialCharacters_ReturnsUrlWithSlug()
    {
        // Arrange
        const string slug = "abc-123_def";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("example.com");

        // Act
        var result = _sut.BuildShortUrl(slug, httpContext.Request);

        // Assert
        result.Should().Be("http://example.com/abc-123_def");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_LocalhostWithoutPort_ReturnsValidUrl()
    {
        // Arrange
        const string slug = "test";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("localhost");

        // Act
        var result = _sut.BuildShortUrl(slug, httpContext.Request);

        // Assert
        result.Should().Be("http://localhost/test");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void BuildShortUrl_SubdomainHost_ReturnsUrlWithSubdomain()
    {
        // Arrange
        const string slug = "abc123";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("api.subdomain.example.com");

        // Act
        var result = _sut.BuildShortUrl(slug, httpContext.Request);

        // Assert
        result.Should().Be("https://api.subdomain.example.com/abc123");
    }
    #endregion
}