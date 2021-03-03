using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.DataLayer.Data;
using ReqResponse.DataLayer.Data.Dapper;
using ReqResponse.DataLayer.Data.Sim;
using ReqResponse.DataLayer.DataAccess;

namespace ReqResponse.Wpf.HostBuilders
{
    public static class AddDataServiceHostBuilderExtension
    {
        public static IHostBuilder AddDataServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
                services.AddSingleton<RequestSimDataService, RequestSimDataService>();
                services.AddSingleton<RequestSqlDataService, RequestSqlDataService>();
                services.AddSingleton<ResponseSimDataService, ResponseSimDataService>();
                services.AddSingleton<ResponseSqlDataService, ResponseSqlDataService>();
                services.AddSingleton<ResponseSummarySimDataService, ResponseSummarySimDataService>();
                services.AddSingleton<ResponseSummarySqlDataService, ResponseSummarySqlDataService>();
                services.AddSingleton<IDataServiceFactory, DataServiceFactory>();
            });

            return host;
        }
    }
}