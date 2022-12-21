using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot.Services
{
    public class PictureService
    {
        private readonly HttpClient _http;

        public PictureService(HttpClient http)
            => _http = http;

        public async Task<Stream> GetCat()
        {
            var resp = await _http.GetAsync("https://cataas.com/cat");
            return await resp.Content.ReadAsStreamAsync();
        }

        public async Task<String> GetMeme(string redditSub = "dankmemes")
        {
            string response = await _http.GetStringAsync($"https://meme-api.com/gimme/{redditSub}");
            dynamic data = JsonConvert.DeserializeObject(response);
            return data.url;
        }
    }
}
