using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace ThatBlokeCalledJay.BlobLite
{
    /// <summary>
    /// Service extensions for BlobLiteClient
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Add BlobLite to the application.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBlobLiteClient(this IServiceCollection services)
        {
            return services.AddSingleton<IBlobLiteClient, BlobLiteClient>(context =>
                new BlobLiteClient(context.GetService<IOptions<BlobLiteConfiguration>>().Value));
        }

        /// <summary>
        /// Add BlobLite to the application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBlobLiteClient(this IServiceCollection services, Action<BlobLiteConfiguration> configuration)
        {
            return services.AddBlobLiteClient().Configure(configuration);
        }
    }
}