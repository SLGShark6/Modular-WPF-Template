using System;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ModularWPFTemplate.Views;
using Microsoft.EntityFrameworkCore;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using ModularWPFTemplate.Extensions;
using ModularWPFTemplate.Services;
using ModularWPFTemplate.Data;
using ModularWPFTemplate.Services.Startup;

namespace ModularWPFTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Application Host
        /// </summary>
        private readonly IHost AppHost;

        /// <summary>
        /// Constructor
        /// </summary>
        public App()
        {
            // Create Application host
            AppHost = CreateHostBuilder(new string[] { }).Build();
        }

        /// <summary>
        /// Application has started, but before any view is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            // Start the application host
            await AppHost.StartAsync();

            // Get a new startup window
            var startupWindow = AppHost.Services.GetRequiredService<StartupWindow>();
            // Run it
            startupWindow.ShowDialog();
        }

        /// <summary>
        /// Application is exiting event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            // Kill the application host gracefully
            await AppHost.StopAsync(TimeSpan.FromSeconds(5));
            // Dispose of the host at the end of execution
            AppHost.Dispose();
        }


        /// <summary>
        /// Create a new builder for the application host with configuration
        /// defaults applied.
        /// </summary>
        /// <param name="args">Host startup arguments</param>
        /// <returns>Configured application host builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                // Add Autofac
                .UseServiceProviderFactory(new AutofacServiceProviderFactory((containerBuilder) =>
                {
                    ConfigureContainer(containerBuilder);
                }))
                // Configure Application services
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context, services);
                });
        }

        /// <summary>
        /// Configure Application services.
        /// </summary>
        /// <param name="context">Application Context</param>
        /// <param name="services">Services Collection to add services to</param>
        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Add App Config
            services.AddApplicationConfiguration(context.Configuration);

            // Add Database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Use SQLite and place the DB in the application folder
                // Example options.UseSqlite(@"Data Source=./application.db");
            });

            // Add File Providers
            services.AddDefaultFileProviders();

            // Add the application views that need to be in the service wrapper
            services.AddAppViews();

            // Add all startup checks
            services.AddTransientsFromAssembly<IStartupCheck>();
        }

        /// <summary>
        /// Autofac specific configuration run after ConfigureServices.
        /// </summary>
        /// <param name="builder">Autofac container builder</param>
        private static void ConfigureContainer(ContainerBuilder builder)
        {

        }
    }
}
