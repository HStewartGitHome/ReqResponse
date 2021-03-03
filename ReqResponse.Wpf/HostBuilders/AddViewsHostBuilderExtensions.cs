using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Wpf.ViewModels;
using ReqResponse.Wpf.Views;

namespace ReqResponse.Wpf.HostBuilders
{
    public static class AddViewsHostBuilderExtensions
    {
        public static IHostBuilder AddViews(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<HomeView>();

                services.AddSingleton<RemoteView>();
                services.AddSingleton<ConnectedView>();
                services.AddSingleton<SummaryView>();
                services.AddSingleton<ErrorsView>();
                services.AddSingleton<LocalView>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
            });

            return host;
        }
    }
}