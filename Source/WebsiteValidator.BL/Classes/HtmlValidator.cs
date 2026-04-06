using System;
using System.Linq;
using HtmlAgilityPack;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class HtmlValidator : IHtmlValidator
    {
        public HtmlValidationResult Validate(string html)
        {
            if (string.IsNullOrEmpty(html))
                return new HtmlValidationResult(true, Array.Empty<string>());

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var errors = doc.ParseErrors?
                .Select(e => $"Line {e.Line}, Col {e.LinePosition}: {e.Reason}")
                .ToArray() ?? Array.Empty<string>();

            return new HtmlValidationResult(errors.Length == 0, errors);
        }
    }
}
