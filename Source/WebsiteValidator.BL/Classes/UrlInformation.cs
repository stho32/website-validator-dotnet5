using System.Net;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class UrlInformation : IUrlInformation
    {
        public string Url { get; }
        public string[] Links { get; }
        public HttpStatusCode HttpResponseCode { get; }
        public string Content { get; }

        public UrlInformation(string url, string[] links, HttpStatusCode httpResponseCode, string content)
        {
            Url = url;
            Links = links;
            HttpResponseCode = httpResponseCode;
            Content = content;
        }
    }
}