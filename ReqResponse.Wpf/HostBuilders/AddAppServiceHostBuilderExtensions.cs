using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Middleware.Services;
using ReqResponse.Services;
using ReqResponse.Services.Email;
using ReqResponse.Services.XmlAPI;

namespace ReqResponse.Wpf.HostBuilders
{
    public static class AddAppServiceHostBuilderExtensions
    {
        public static IHostBuilder AddAppServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddScoped<LocalXmlService, LocalXmlService>();
                services.AddScoped<ConnectedXmlService, ConnectedXmlService>();
                services.AddSingleton<EmailService, EmailService>();
                services.AddSingleton<IServiceFactory, ServiceFactory>();
                services.AddScoped<IRequestService, RequestService>();
            });

            return host;
        }
    }
}