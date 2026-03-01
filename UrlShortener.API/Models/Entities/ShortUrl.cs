namespace UrlShortener.API.Models.Entities
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public required string OriginalUrl { get; set; } = null!;
        public required string Slug { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddYears(1);
    }
}
