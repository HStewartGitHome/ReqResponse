using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Models;

namespace ReqResponse.Wpf.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfigurations(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                string settingsFile = "appsettings.json";
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(settingsFile)
                    .Build();

                services.AddSingleton<IConfiguration>(configuration);

                var emailConfig = configuration
                    .GetSection("EmailConfiguration")
                    .Get<EmailConfiguration>();
                services.AddSingleton(emailConfig);
            });

            return host;
        }
    }
}