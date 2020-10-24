using Microsoft.Extensions.Configuration;

namespace ThatBlokeCalledJay.BlobLite.Tests
{
    public static class CommonTestHelpers
    {
        public static IConfiguration LoadConfiguration(string fileName = null)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<BlobLiteTests>();
            
            if (!string.IsNullOrWhiteSpace(fileName))
                builder.AddJsonFile(fileName, true);

            return builder.Build();
        }
    }
}