using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class OutputHelperFactory : IOutputHelperFactory
    {
        public IOutputHelper Get(bool human)
        {
            if (human)
                return new HumanReadableConsoleOutputHelper();

            return new JsonConsoleOutputHelper();
        }
    }
}