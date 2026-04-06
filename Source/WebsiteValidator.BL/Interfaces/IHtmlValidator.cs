namespace WebsiteValidator.BL.Interfaces
{
    public interface IHtmlValidator
    {
        HtmlValidationResult Validate(string html);
    }

    public class HtmlValidationResult
    {
        public bool IsValid { get; }
        public string[] Errors { get; }

        public HtmlValidationResult(bool isValid, string[] errors)
        {
            IsValid = isValid;
            Errors = errors;
        }
    }
}
