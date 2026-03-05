namespace UrlShortener.API.Models.Entities
{
    public class ShortUrl
    {
        public int Id { get; private set; }
        public string OriginalUrl { get; private set; }
        public string Slug { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        public ShortUrl(string originalUrl, string slug)
        {
            OriginalUrl = originalUrl;
            Slug = slug;
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = CreatedAt.AddYears(1);
        }
    }
}
