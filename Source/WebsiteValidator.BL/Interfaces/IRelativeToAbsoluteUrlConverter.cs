namespace WebsiteValidator.BL.Interfaces
{
    public interface IRelativeToAbsoluteUrlConverter
    {
        string ToAbsoluteUrl(string baseUrl, string relativeUrl);
    }
}