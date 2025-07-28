import axios from 'axios';

export async function shortenUrl(longUrl: string): Promise<{ shortUrl: string }> {
  const response = await axios.post('https://localhost:7294/shorten', {
    url: longUrl
  });

  return response.data;
}
