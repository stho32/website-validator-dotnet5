namespace WebsiteValidator.BL.Interfaces
{
    public interface IUrlInformation
    {
        string Url { get; }
        string[] Links { get; }
        int HttpResponseCode { get; }
    }


}