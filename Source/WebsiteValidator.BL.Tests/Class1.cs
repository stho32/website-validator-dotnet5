using System;
using Xunit;
using Xunit.Abstractions;

namespace WebsiteValidator.BL.Tests
{
    public class Class1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Class1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test()
        {
            _testOutputHelper.WriteLine("Hello world");
        }
    }
}
