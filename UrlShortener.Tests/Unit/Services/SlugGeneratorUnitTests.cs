using FluentAssertions;
using UrlShortener.API.Services;

namespace UrlShortener.Tests.Unit.Services;
public class SlugGeneratorUnitTests
{
    public readonly SlugGenerator _slugGenerator;

    public SlugGeneratorUnitTests()
    {
        _slugGenerator = new SlugGenerator();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GenerateSlug_WhenCalled_ReturnsNonNullOrWhiteSpace()
    {
        // Arrange
        var sut = _slugGenerator;

        // Act
        var slug = sut.GenerateSlug();

        // Assert
        slug.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GenerateSlug_WhenCalled_ReturnsEightCharacterSlug()
    {
        // Arrange
        var sut = _slugGenerator;

        // Act
        var slug = sut.GenerateSlug();

        // Assert
        slug.Should().HaveLength(8);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GenerateSlug_WhenCalled_ReturnsSlugWithOnlyBase62Characters()
    {
        // Arrange
        var sut = _slugGenerator;

        // Act
        var slug = sut.GenerateSlug();

        // Assert
        slug.Should().MatchRegex("^[a-zA-Z0-9]{8}$");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GenerateSlug_WhenCalledMoreThanOnce_ProducesDifferentSlugs()
    {
        // Arrange
        var sut = _slugGenerator;

        // Act
        var slug1 = sut.GenerateSlug();
        var slug2 = sut.GenerateSlug();

        // Assert
        slug1.Should().NotBe(slug2);
    }
}
