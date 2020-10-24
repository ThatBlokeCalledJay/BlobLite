using Microsoft.Extensions.Configuration;

namespace ThatBlokeCalledJay.BlobLite.Tests
{
    public static class CommonTestHelpers
    {
        public static IConfiguration LoadConfiguration(string fileName = "appsettings.test.json")
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<BlobLiteTests>()
                .AddJsonFile(fileName, true).Build();
        }
    }
}