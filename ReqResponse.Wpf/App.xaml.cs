using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Middleware.Services;
using ReqResponse.Middleware.Services.Client.Factories;
using ReqResponse.Wpf.HostBuilders;
using System.Windows;

namespace ReqResponse.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .AddLogging()
                .AddCommands()
                .AddDataServices()
                .AddAppServices()
                .AddConfigurations()
                .AddViewModels()
                .AddViews();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            IRequestService requestService = _host.Services.GetRequiredService<IRequestService>();

            TestRequesteServiceClientFactory.SetService(requestService);
            TestModelRequestServiceClientFactory.SetService(requestService);

            Window window = _host.Services.GetRequiredService<MainWindow>();

            window.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}