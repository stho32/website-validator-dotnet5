using NUnit.Framework;
using WebsiteValidator.BL.Classes;

namespace WebsiteValidator.BL.Tests
{
    [TestFixture]
    public class HtmlValidatorTests
    {
        private HtmlValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new HtmlValidator();
        }

        [Test]
        public void Validate_valides_HTML_gibt_keine_Fehler()
        {
            var html = "<!DOCTYPE html><html><head><title>Test</title></head><body><p>Hello</p></body></html>";

            var result = _validator.Validate(html);

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [Test]
        public void Validate_unclosed_Tag_gibt_Fehler()
        {
            var html = "<html><body><div><span></div></span></body></html>";

            var result = _validator.Validate(html);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
        }

        [Test]
        public void Validate_leerer_String_gibt_keine_Fehler()
        {
            var result = _validator.Validate("");

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [Test]
        public void Validate_null_gibt_keine_Fehler()
        {
            var result = _validator.Validate(null);

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [Test]
        public void Validate_Nicht_HTML_Content_gibt_keine_Fehler()
        {
            var result = _validator.Validate("Just plain text without any HTML.");

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [Test]
        public void Validate_Fehler_enthalten_Zeilenangabe()
        {
            var html = "<html>\n<body>\n<div><span></div></span>\n</body>\n</html>";

            var result = _validator.Validate(html);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors[0], Does.Contain("Line"));
            Assert.That(result.Errors[0], Does.Contain("Col"));
        }

        [Test]
        public void Validate_mehrere_Fehler_werden_alle_gemeldet()
        {
            var html = "<html><body><div><span></div></span><div><em></div></em></body></html>";

            var result = _validator.Validate(html);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Length, Is.GreaterThan(1));
        }
    }
}
