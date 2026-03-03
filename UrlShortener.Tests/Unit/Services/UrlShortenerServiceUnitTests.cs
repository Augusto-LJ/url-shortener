using FluentAssertions;
using Moq;
using UrlShortener.API.Data;
using UrlShortener.API.Services;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.Tests.Unit.Services;
public class UrlShortenerServiceUnitTests
{
    private readonly Mock<IUrlShortenerRepository> _repositoryMock;
    private readonly Mock<ISlugGenerator> _slugGeneratorMock;
    private readonly UrlShortenerService _sut;

    public UrlShortenerServiceUnitTests()
    {
        _repositoryMock = new Mock<IUrlShortenerRepository>();
        _slugGeneratorMock = new Mock<ISlugGenerator>();
        _sut = new UrlShortenerService(_repositoryMock.Object, _slugGeneratorMock.Object);
    }

    #region CreateUniqueSlugAsync
    [Fact]
    [Trait("Category", "Unit")]
    public async Task CreateUniqueSlugAsync_SlugDoesNotExist_ReturnsGeneratedSlug()
    {
        // Arrange
        const string expectedSlug = "aBc12345";
        _slugGeneratorMock.Setup(x => x.GenerateSlug()).Returns(expectedSlug);
        _repositoryMock.Setup(x => x.SlugExistsAsync(expectedSlug)).ReturnsAsync(false);

        // Act
        var result = await _sut.CreateUniqueSlugAsync();

        // Assert
        result.Should().Be(expectedSlug);
        _slugGeneratorMock.Verify(x => x.GenerateSlug(), Times.Once);
        _repositoryMock.Verify(x => x.SlugExistsAsync(expectedSlug), Times.Once);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CreateUniqueSlugAsync_SlugExistsOnFirstAttempt_RetriesAndReturnsNewSlug()
    {
        // Arrange
        const string existingSlug = "exsSLG12";
        const string newSlug = "newSLG12";

        _slugGeneratorMock.SetupSequence(x => x.GenerateSlug())
            .Returns(existingSlug)
            .Returns(newSlug);

        _repositoryMock.Setup(x => x.SlugExistsAsync(existingSlug)).ReturnsAsync(true);
        _repositoryMock.Setup(x => x.SlugExistsAsync(newSlug)).ReturnsAsync(false);

        // Act 
        var result = await _sut.CreateUniqueSlugAsync();

        // Assert
        result.Should().Be(newSlug);
        _slugGeneratorMock.Verify(x => x.GenerateSlug(), Times.Exactly(2));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CreateUniqueSlugAsync_AllAttemptsCollide_ThrowsInvalidOperationException()
    {
        // Arrange
        const string collidingSlug = "collide1";

        _slugGeneratorMock.Setup(x => x.GenerateSlug()).Returns(collidingSlug);
        _repositoryMock.Setup(x => x.SlugExistsAsync(collidingSlug)).ReturnsAsync(true);

        // Act
        var result = () => _sut.CreateUniqueSlugAsync();

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Failed to generate a unique slug.");
        _slugGeneratorMock.Verify(x => x.GenerateSlug(), Times.Exactly(10));
    }
    #endregion
}
