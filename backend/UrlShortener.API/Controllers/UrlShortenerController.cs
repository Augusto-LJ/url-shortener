using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Models.Request;
using UrlShortener.API.Services;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    public class UrlShortenerController(IUrlShortenerService urlShortenerService) : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService = urlShortenerService;

        /// <summary>
        /// Returns a short URL
        /// </summary>
        /// <returns>Short URL</returns>
        [HttpPost("shorten")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ShortenAsync([FromBody] ShortenRequest request)
        {
            if (_urlShortenerService.RequestDataIsValid(request))
            {
                var slug = await urlShortenerService.CreateUniqueSlugAsync();
                await urlShortenerService.SaveShortUrlAsync(request.Url, slug);

                var shortUrl = $"{Request.Scheme}://{Request.Host}/{slug}";
                return Ok(shortUrl);
            }

            return BadRequest("Invalid URL.");
        }
    }
}
