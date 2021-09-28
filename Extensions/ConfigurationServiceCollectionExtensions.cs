using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularWPFTemplate.Configuration;

namespace ModularWPFTemplate.Extensions
{
    public static class ConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds application configuration from the configuration provider
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Application Settings
            services.Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));

            return services;
        }
    }
}
