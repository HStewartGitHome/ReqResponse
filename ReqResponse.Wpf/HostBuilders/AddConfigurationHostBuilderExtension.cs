using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Models;
using ReqResponse.Support;

namespace ReqResponse.Wpf.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfigurations(this IHostBuilder host)
        {

            EmailConfiguration emailConfig;
            ServerConfiguration serverConfig;
            IConfiguration configuration = ConfigHelper.CreateConfiguration("appsettings.json",
                                           out emailConfig,
                                           out serverConfig);
            ConfigFactory.SetConfiguration(configuration);

            host.ConfigureServices(services =>
            {
                services.AddSingleton<IConfiguration>(configuration);
                services.AddSingleton(emailConfig);
                services.AddSingleton(serverConfig);
            });

            return host;
        }
    }
}