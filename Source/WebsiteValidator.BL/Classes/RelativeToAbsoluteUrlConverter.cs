using System;
using System.Linq;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class UrlToAbsolutUrlConverter : IUrlToAbsolutUrlConverter
    {
        private static readonly string[] IgnoredSchemes = { "mailto:", "tel:", "javascript:", "data:", "ftp:" };

        public string ToAbsoluteUrl(string baseUrl, string url)
        {
            if (string.IsNullOrWhiteSpace(url) || url.StartsWith("#"))
            {
                return null;
            }

            if (IgnoredSchemes.Any(scheme => url.StartsWith(scheme, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            if (url.EndsWith("/"))
            {
                url = url.TrimEnd('/');
            }

            if (url.StartsWith("https://") || url.StartsWith("http://"))
            {
                return url;
            }

            if (url.StartsWith("//"))
            {
                return url;
            }

            var result = baseUrl;

            if (!result.EndsWith("/"))
            {
                result += "/";
            }

            if (url.StartsWith("/"))
            {
                var temp = url.Remove(0, 1);
                result += temp;
            }
            else
            {
                result += url;
            }

            if (result.EndsWith("/"))
            {
                result = result.TrimEnd('/');
            }
            return result;
        }

        public string[] ToAbsoluteUrl(string baseUrl, string[] links)
        {
            return links
                .Select(link => ToAbsoluteUrl(baseUrl, link))
                .Where(link => link != null)
                .ToArray();
        }
    }
}
