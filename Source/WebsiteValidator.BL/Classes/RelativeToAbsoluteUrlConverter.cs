using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class RelativeToAbsoluteUrlConverter : IRelativeToAbsoluteUrlConverter
    {
        public string ToAbsoluteUrl(string baseUrl, string relativeUrl)
        {
            var result = baseUrl;

            if (!result.EndsWith("/"))
            {
                result += "/";
            }

            if (relativeUrl.StartsWith("/"))
            {
                var temp = relativeUrl.Remove(0, 1); // remove "/"
                result += temp;
            }

            return result;
        }
    }
}