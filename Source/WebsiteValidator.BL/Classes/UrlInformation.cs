using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class UrlInformation : IUrlInformation
    {
        public string Url { get; }
        public string[] Links { get; }
        public int HttpResponseCode { get; }

        public UrlInformation(string url, string[] links, int httpResponseCode)
        {
            Url = url;
            Links = links;
            HttpResponseCode = httpResponseCode;
        }
    }
}