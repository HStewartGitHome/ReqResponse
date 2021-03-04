
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Middleware.Services;
using ReqResponse.Middleware.Services.Client;
using ReqResponse.Services;
using ReqResponse.Services.Email;
using ReqResponse.Services.XmlAPI;

namespace ReqResponse.Command.HostBuilders
{
    public static class AddAppServiceHostBuilderExtensions
    {
        public static IHostBuilder AddAppServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<LocalXmlService, LocalXmlService>();
                services.AddSingleton<ConnectedXmlService, ConnectedXmlService>();
                services.AddSingleton<EmailService, EmailService>();
                services.AddSingleton<IServiceFactory, ServiceFactory>();
                services.AddSingleton<IRequestService, RequestService>();
                services.AddSingleton<ITestRequestServiceClient, TestRequestServiceClient>();
            });

            return host;
        }
    }
}