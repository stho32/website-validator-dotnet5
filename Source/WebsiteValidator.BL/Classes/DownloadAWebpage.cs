using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class DownloadAWebpage : IDownloadAWebpage
    {
        public async Task<IWebpage> Download(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();
            return new Webpage(url, pageContents, response.StatusCode);
        }
    }
}