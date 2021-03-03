using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Wpf.ViewModels;
using ReqResponse.Wpf.ViewModels.Factories;

namespace ReqResponse.Wpf.HostBuilders
{
    public static class AddViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<LocalViewModel>();
                services.AddSingleton<RemoteViewModel>();
                services.AddSingleton<ConnectedViewModel>();
                services.AddSingleton<SummaryViewModel>();
                services.AddSingleton<ErrorsViewModel>();

                services.AddSingleton<IRootViewModelFactory, RootViewModelFactory>();
                services.AddSingleton<IViewModelFactory<HomeViewModel>, HomeViewModelFactory>();
                services.AddSingleton<IViewModelFactory<LocalViewModel>, LocalViewModelFactory>();
                services.AddSingleton<IViewModelFactory<RemoteViewModel>, RemoteViewModelFactory>();
                services.AddSingleton<IViewModelFactory<ConnectedViewModel>, ConnectedViewModelFactory>();
                services.AddSingleton<IViewModelFactory<SummaryViewModel>, SummaryViewModelFactory>();
                services.AddSingleton<IViewModelFactory<ErrorsViewModel>, ErrorsViewModelFactory>();
            });

            return host;
        }
    }
}