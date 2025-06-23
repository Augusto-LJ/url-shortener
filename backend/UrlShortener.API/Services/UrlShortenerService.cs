using System.Security.Cryptography;
using System.Text;
using UrlShortener.API.Models;

namespace UrlShortener.API.Services
{
    public class UrlShortenerService
    {
        private const string Base62Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int SlugLength = 8;

        public bool RequestDataIsValid(ShortenRequest request)
        {
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out var uriResult))
            {
                return (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                       && !string.IsNullOrWhiteSpace(uriResult.Host);
            }

            return false;
        }

        public string GenerateSlug()
        {
            var bytes = new byte[8];
            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(bytes);

            var chars = new char[SlugLength];
            for (int i = 0; i < SlugLength; i++)
                chars[i] = Base62Chars[bytes[i] % Base62Chars.Length];

            return new string(chars);
        }
    }
}
