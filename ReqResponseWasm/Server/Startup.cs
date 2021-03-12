using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.DataLayer.Data;
using ReqResponse.DataLayer.Data.Dapper;
using ReqResponse.DataLayer.Data.Sim;
using ReqResponse.DataLayer.DataAccess;
using ReqResponse.Middleware.Services;
using ReqResponse.Middleware.Services.Client;
using ReqResponse.Models;
using ReqResponse.Services;
using ReqResponse.Services.Email;
using ReqResponse.Services.XmlAPI;
using ReqResponse.Support;

namespace ReqResponseWasm.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigFactory.SetConfiguration(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            EmailConfiguration emailConfig;
            ServerConfiguration serverConfig;
            ConfigHelper.CreateConfigurations(Configuration,
                                              out emailConfig,
                                              out serverConfig);

            services.AddSingleton(emailConfig);
            services.AddSingleton(serverConfig);

            services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
            services.AddSingleton<RequestSimDataService, RequestSimDataService>();
            services.AddSingleton<RequestSqlDataService, RequestSqlDataService>();
            services.AddSingleton<ResponseSimDataService, ResponseSimDataService>();
            services.AddSingleton<ResponseSqlDataService, ResponseSqlDataService>();
            services.AddSingleton<ResponseSummarySimDataService, ResponseSummarySimDataService>();
            services.AddSingleton<ResponseSummarySqlDataService, ResponseSummarySqlDataService>();
            services.AddSingleton<IDataServiceFactory, DataServiceFactory>();
            services.AddSingleton<LocalXmlService, LocalXmlService>();
            services.AddSingleton<ConnectedXmlService, ConnectedXmlService>();
            services.AddSingleton<EmailService, EmailService>();
            services.AddSingleton<IServiceFactory, ServiceFactory>();
            services.AddSingleton<IRequestService, RequestService>();
            services.AddSingleton<ITestRequestServiceClient, TestRequestServiceClient>();
            services.AddSingleton<ITestModelRequestServiceClient, TestModelRequestServiceClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}