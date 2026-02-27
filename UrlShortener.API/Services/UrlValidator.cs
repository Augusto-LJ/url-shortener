using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Services;

public class UrlValidator : IUrlValidator
{
    public bool RequestDataIsValid(string? url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
        {
            return (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                   && !string.IsNullOrWhiteSpace(uriResult.Host);
        }

        return false;
    }
}
