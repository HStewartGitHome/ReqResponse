using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ReqResponse.Command.HostBuilders
{
    public static class AddLoggingHostBuilderExtensions
    {
        public static IHostBuilder AddLogging(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddLogging(configure =>
                {
                    configure.AddDebug();
                });
            });

            return host;
        }
    }
}