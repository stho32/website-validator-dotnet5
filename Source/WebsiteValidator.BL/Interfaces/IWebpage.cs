namespace WebsiteValidator.BL.Interfaces
{
    /// <summary>
    /// In this application we are focusing on webpages
    /// </summary>
    public interface IWebpage
    {
        /// <summary>
        /// The absolute url to the resource.
        /// </summary>
        string AbsoluteUrl { get; }

        /// <summary>
        /// The raw content as string.
        /// </summary>
        string RawContent { get; }

        /// <summary>
        /// The HTTP code that was returned.
        /// </summary>
        int HttpCode { get; }
    }

    /// <summary>
    /// We have to download pages, so we need to have things that download pages.
    /// </summary>
    public interface IDownloadAWebpage
    {
        IWebpage Download(string url);
    }

    /// <summary>
    /// We need to extract the urls.
    /// </summary>
    public interface IUrlExtractor
    {
        string[] ExtractUrls(IWebpage webpage);
    }

    /// <summary>
    /// The results are more or less sever.
    /// </summary>
    public enum ValidationMessageSeverityEnum
    {
        Low,
        Middle,
        High
    }

    public interface IValidationMessage
    {
        ValidationMessageSeverityEnum Severity { get; }
        string Message { get; }
    }

    public interface IValidator
    {
        IValidationMessage[] Validate(IWebpage webpage);
    }

    public interface IValidatedWebpage : IWebpage
    {
        IValidationMessage[] ValidationMessages { get; }
    }
}