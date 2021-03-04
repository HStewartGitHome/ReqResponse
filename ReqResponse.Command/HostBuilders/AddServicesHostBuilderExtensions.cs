using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Command.Services;

namespace ReqResponse.Command.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<ProcessLocalTestRequestService, ProcessLocalTestRequestService>();
                services.AddSingleton<ProcessRemoteTestRequestService, ProcessRemoteTestRequestService>();
                services.AddSingleton<ProcessConnectedTestRequestService, ProcessConnectedTestRequestService>();
                services.AddSingleton<ProcessTestSummaryService, ProcessTestSummaryService>();
                services.AddSingleton<ProcessTestErrorReportService, ProcessTestErrorReportService>();
                services.AddSingleton<ProcessTestEmailService, ProcessTestEmailService>();
                services.AddSingleton<ProcessTestService, ProcessTestService>();
            });

            return host;
        }
    }
}