import axios from 'axios';

export async function shortenUrl(longUrl: string): Promise<{ shortUrl: string }> {
  const response = await axios.post(`${import.meta.env.VITE_API_BASE_URL}/shorten`, {
    url: longUrl
  });

  return response.data;
}
