using System;
using System.Net;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class UrlInformation : IUrlInformation
    {
        public string Url { get; }
        public string[] Links { get; }
        public int ContentSizeInBytes { get; }
        public HttpStatusCode HttpResponseCode { get; }
        public string Content { get; }
        public string ContentWithoutHtml { get; }
        public bool IsHtmlValid { get; }
        public string[] HtmlErrors { get; }

        public UrlInformation(string url, string[] links, HttpStatusCode httpResponseCode, string content, string contentWithoutHtml, int contentSizeInBytes,
            bool isHtmlValid = true, string[] htmlErrors = null)
        {
            Url = url;
            Links = links;
            HttpResponseCode = httpResponseCode;
            Content = content;
            ContentWithoutHtml = contentWithoutHtml;
            ContentSizeInBytes = contentSizeInBytes;
            IsHtmlValid = isHtmlValid;
            HtmlErrors = htmlErrors ?? Array.Empty<string>();
        }
    }
}