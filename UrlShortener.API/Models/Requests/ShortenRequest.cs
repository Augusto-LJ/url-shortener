namespace UrlShortener.API.Models.Request
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
