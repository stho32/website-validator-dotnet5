namespace WebsiteValidator.BL.Interfaces
{
    /// <summary>
    /// We have to download pages, so we need to have things that download pages.
    /// </summary>
    public interface IDownloadAWebpage
    {
        IWebpage Download(string url);
    }
}