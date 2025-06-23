using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Models;
using UrlShortener.API.Services;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    public class UrlShortenerController : Controller
    {
        /// <summary>
        /// Returns a short URL
        /// </summary>
        /// <returns>Short URL</returns>
        [HttpPost("shorten")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Shorten([FromBody] ShortenRequest request)
        {
            var urlShortenerService = new UrlShortenerService();

            if (urlShortenerService.RequestDataIsValid(request))
                return Ok();
            return BadRequest("Invalid URL.");
        }
    }
}
