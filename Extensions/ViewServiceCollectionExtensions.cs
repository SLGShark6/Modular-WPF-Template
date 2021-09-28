using Microsoft.Extensions.DependencyInjection;
using ModularWPFTemplate.Views;

namespace ModularWPFTemplate.Extensions
{
    public static class ViewServiceCollectionExtensions
    {
        /// <summary>
        /// Add application views to the service collection.
        /// </summary>
        /// <param name="services">Service collection to the views to</param>
        /// <returns>The provided service collection for method chaining</returns>
        public static IServiceCollection AddAppViews(this IServiceCollection services)
        {
            // Add Windows
            services.AddAppWindows();

            return services;
        }

        /// <summary>
        /// Add the application window views to the provided service collection
        /// </summary>
        /// <param name="services">Service collection to the views to</param>
        /// <returns>The provided service collection for method chaining</returns>
        public static IServiceCollection AddAppWindows(this IServiceCollection services)
        {
            services.AddTransient<StartupWindow>();
            services.AddTransient<MainWindow>();

            return services;
        }
    }
}
