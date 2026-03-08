using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.API.Controllers;
using UrlShortener.API.Controllers.Interfaces;
using UrlShortener.API.Models.Request;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.Tests.Unit.Controller;

public class UrlShortenerControllerUnitTests
{
    private readonly Mock<IUrlShortenerService> _serviceMock;
    private readonly Mock<IUrlValidator> _validatorMock;
    private readonly Mock<IUrlBuilder> _urlBuilderMock;
    private readonly UrlShortenerController _sut;

    public UrlShortenerControllerUnitTests()
    {
        _serviceMock = new Mock<IUrlShortenerService>();
        _validatorMock = new Mock<IUrlValidator>();
        _urlBuilderMock = new Mock<IUrlBuilder>();
        _sut = new UrlShortenerController(
            _serviceMock.Object,
            _validatorMock.Object,
            _urlBuilderMock.Object);
    }

    #region ShortenAsync
    [Fact]
    [Trait("Category", "Unit")]
    public async Task ShortenAsync_ValidUrl_ReturnsOkWithShortUrl()
    {
        // Arrange
        var request = new ShortenRequest { Url = "https://example.com" };
        const string slug = "aBc12345";
        const string shortUrl = "http://test.com/aBc12345";

        _validatorMock.Setup(x => x.RequestDataIsValid(request.Url)).Returns(true);
        _serviceMock.Setup(x => x.CreateUniqueSlugAsync()).ReturnsAsync(slug);
        _urlBuilderMock.Setup(x => x.BuildShortUrl(slug)).Returns(shortUrl);

        // Act
        var result = await _sut.ShortenAsync(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(shortUrl);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ShortenAsync_ValidUrl_CallsSaveShortUrlAsync()
    {
        // Arrange
        var request = new ShortenRequest { Url = "https://example.com" };
        const string slug = "aBc12345";
        _validatorMock.Setup(x => x.RequestDataIsValid(request.Url)).Returns(true);
        _serviceMock.Setup(x => x.CreateUniqueSlugAsync()).ReturnsAsync(slug);
        _urlBuilderMock.Setup(x => x.BuildShortUrl(slug)).Returns("http://test.com/aBc12345");

        // Act
        await _sut.ShortenAsync(request);

        // Assert
        _serviceMock.Verify(x => x.SaveShortUrlAsync(request.Url, slug), Times.Once);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ShortenAsync_InvalidUrl_ReturnsBadRequest()
    {
        // Arrange
        var request = new ShortenRequest { Url = "invalid-url" };
        _validatorMock.Setup(x => x.RequestDataIsValid(request.Url)).Returns(false);

        // Act
        var result = await _sut.ShortenAsync(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.Value.Should().Be("Invalid URL.");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ShortenAsync_InvalidUrl_DoesNotCallService()
    {
        // Arrange
        var request = new ShortenRequest { Url = "invalid-url" };
        _validatorMock.Setup(x => x.RequestDataIsValid(request.Url)).Returns(false);

        // Act
        await _sut.ShortenAsync(request);

        // Assert
        _serviceMock.Verify(x => x.CreateUniqueSlugAsync(), Times.Never);
        _serviceMock.Verify(x => x.SaveShortUrlAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
    #endregion

    #region RedirectToOriginalUrl
    [Fact]
    [Trait("Category", "Unit")]
    public async Task RedirectToOriginalUrl_SlugExists_ReturnsRedirect()
    {
        // Arrange
        const string slug = "aBc12345";
        const string originalUrl = "https://example.com";
        _serviceMock.Setup(x => x.GetOriginalUrlAsync(slug)).ReturnsAsync(originalUrl);

        // Act
        var result = await _sut.RedirectToOriginalUrl(slug);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(originalUrl);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RedirectToOriginalUrl_SlugDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        const string slug = "notfound";
        _serviceMock.Setup(x => x.GetOriginalUrlAsync(slug)).ReturnsAsync((string?)null);

        // Act
        var result = await _sut.RedirectToOriginalUrl(slug);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.Value.Should().Be($"URL not found for the following slug: {slug}");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RedirectToOriginalUrl_EmptyOriginalUrl_ReturnsNotFound()
    {
        // Arrange
        const string slug = "aBc12345";
        _serviceMock.Setup(x => x.GetOriginalUrlAsync(slug)).ReturnsAsync(string.Empty);

        // Act
        var result = await _sut.RedirectToOriginalUrl(slug);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RedirectToOriginalUrl_WhitespaceOriginalUrl_ReturnsNotFound()
    {
        // Arrange
        const string slug = "aBc12345";
        _serviceMock.Setup(x => x.GetOriginalUrlAsync(slug)).ReturnsAsync("   ");

        // Act
        var result = await _sut.RedirectToOriginalUrl(slug);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    #endregion
}
