using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Wpf.Commands;
using System.Windows.Input;

namespace ReqResponse.Wpf.HostBuilders
{
    public static class AddCommandHostBuilderExtensions
    {
        public static IHostBuilder AddCommands(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<ICommand, UpdateViewCommand>();
            });

            return host;
        }
    }
}
