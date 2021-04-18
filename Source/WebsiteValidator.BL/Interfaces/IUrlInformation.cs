using System.Net;

namespace WebsiteValidator.BL.Interfaces
{
    public interface IUrlInformation
    {
        string Url { get; }
        string[] Links { get; }
        HttpStatusCode HttpResponseCode { get; }
    }


}