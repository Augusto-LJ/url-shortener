namespace UrlShortener.API.Models
{
    public class ShortenRequest
    {
        /// <summary>
        /// Long URL
        /// </summary>
        /// <example>https://www.google.com.br</example>
        public string Url { get; set; }
    }
}
