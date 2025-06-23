using UrlShortener.API.Models;

namespace UrlShortener.API.Services
{
    public class UrlShortenerService
    {
        public bool RequestDataIsValid(ShortenRequest request)
        {
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out var uriResult))
            {
                return (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                       && !string.IsNullOrWhiteSpace(uriResult.Host);
            }

            return false;
        }

    }
}
