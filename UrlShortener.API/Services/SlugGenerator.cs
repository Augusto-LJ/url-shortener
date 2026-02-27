using System.Security.Cryptography;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Services;

public class SlugGenerator : ISlugGenerator
{
    private const string Base62Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int SlugLength = 8;

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
