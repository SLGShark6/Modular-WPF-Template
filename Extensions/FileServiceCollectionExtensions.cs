using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using ModularWPFTemplate.Helpers;

namespace ModularWPFTemplate.Extensions
{
    public static class FileServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default physical and embedded file provider as
        /// IFileProvider as well as themselves. Also adds a composite file
        /// provider factory as the default IFileProvider, that incorporates
        /// all available IFileProviders from the service wrapper at the time
        /// of resolution.
        /// </summary>
        /// <param name="services">Service collection to add the fileproviders to</param>
        /// <returns>The original service collection for method chaining</returns>
        public static IServiceCollection AddDefaultFileProviders(this IServiceCollection services)
        {
            // Add the default embedded file provider
            services.AddDefaultEmbeddedFileProvider()
                // Add the default physical file provider
                .AddDefaultPhysicalFileProvider()
                // Add a composite file provider factory as the default IFileProvider
                .AddCompositeFileProvider();

            return services;
        }

        /// <summary>
        /// Adds a default embedded file provider for the application entry
        /// point.
        /// </summary>
        /// <param name="services">Service collection to add the fileprovider to</param>
        /// <returns>The original service collection for method chaining</returns>
        public static IServiceCollection AddDefaultEmbeddedFileProvider(this IServiceCollection services)
        {
            var embeddedFileProvider = new EmbeddedFileProvider(typeof(App).Assembly);

            services.AddSingleton<EmbeddedFileProvider>(embeddedFileProvider)
                .AddSingleton<IFileProvider>(embeddedFileProvider);

            return services;
        }

        /// <summary>
        /// Adds a default physical file provider with the base path set to the
        /// application location.
        /// </summary>
        /// <param name="services">Service collection to add the fileprovider to</param>
        /// <returns>The original service collection for method chaining</returns>
        public static  IServiceCollection AddDefaultPhysicalFileProvider(this IServiceCollection services)
        {
            var physicalFileProvider = new PhysicalFileProvider(PathHelper.AppBasePath);

            services.AddSingleton<PhysicalFileProvider>(physicalFileProvider)
                .AddSingleton<IFileProvider>(physicalFileProvider);

            return services;
        }

        /// <summary>
        /// Adds a resolver function for a CompositeFileProvider that
        /// incorporates all Physical and Embedded file provider registered to
        /// the service wrapper.
        /// </summary>
        /// <param name="services">Service collection to add the fileprovider to</param>
        /// <returns>The original service collection for method chaining</returns>
        public static IServiceCollection AddCompositeFileProvider(this IServiceCollection services)
        {
            CompositeFileProvider fileProvider = null;

            // Factory function for getting/ creating the composite file provider
            Func<IServiceProvider, CompositeFileProvider> factory = ctx =>
            {
                // If file provider hasn't been created yet
                if (fileProvider == null)
                {
                    // Get the registered physical and embedded file providers
                    var physicalFileProviders = ctx.GetServices<PhysicalFileProvider>() as IEnumerable<IFileProvider>;
                    var embeddedFileProviders = ctx.GetServices<EmbeddedFileProvider>() as IEnumerable<IFileProvider>;

                    // Create a composite provider using all the found file providers
                    fileProvider = new CompositeFileProvider(physicalFileProviders.Concat(embeddedFileProviders));
                }

                return fileProvider;
            };

            // Add the resolver function to the service wrapper
            services.AddSingleton<CompositeFileProvider>(factory)
                .AddSingleton<IFileProvider>(factory);

            return services;
        }
    }
}
