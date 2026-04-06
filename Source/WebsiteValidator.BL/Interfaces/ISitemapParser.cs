namespace WebsiteValidator.BL.Interfaces;

public interface ISitemapParser
{
    string[] ExtractUrls(string? sitemapXml);
}
