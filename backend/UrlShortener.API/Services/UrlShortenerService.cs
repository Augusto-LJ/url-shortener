using System.Security.Cryptography;
using System.Text;
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

        public string GenerateHash(string longUrl)
        {
            byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(longUrl));
            string base64 = Convert.ToBase64String(hashBytes);
            string cleanBase64 = base64.Replace("+", "").Replace("/", "").Replace("=", "");

            return cleanBase64[..8];

        }
    }
}
