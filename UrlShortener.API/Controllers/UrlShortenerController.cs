using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Controllers.Interfaces;
using UrlShortener.API.Models.Request;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    public class UrlShortenerController(IUrlShortenerService urlShortenerService,
                                        IUrlValidator urlValidator,
                                        IUrlBuilder urlBuilder) : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService = urlShortenerService;
        private readonly IUrlValidator _urlValidator = urlValidator;
        private readonly IUrlBuilder _urlBuilder = urlBuilder;

        /// <summary>
        /// Returns a short URL
        /// </summary>
        /// <returns>Short URL</returns>
        [HttpPost("shorten")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ShortenAsync([FromBody] ShortenRequest request)
        {
            if (!_urlValidator.RequestDataIsValid(request.Url))
                return BadRequest("Invalid URL.");

            var slug = await _urlShortenerService.CreateUniqueSlugAsync();
            await _urlShortenerService.SaveShortUrlAsync(request.Url, slug);

            var shortUrl = _urlBuilder.BuildShortUrl(slug, Request);
            return Ok(shortUrl);
        }

        /// <summary>
        /// Redirects to the original URL
        /// </summary>
        /// <returns>Redirects to the original URL</returns>
        [HttpGet("{slug}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status302Found)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RedirectToOriginalUrl([FromRoute] string slug)
        {
            var longUrl = await _urlShortenerService.GetOriginalUrlAsync(slug);

            if (!string.IsNullOrWhiteSpace(longUrl))
                return Redirect(longUrl);

            return NotFound($"URL not found for the following slug: {slug}");
        }
    }
}
