using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Blazor.Services;
using ReqResponse.DataLayer.Data;
using ReqResponse.DataLayer.Data.Dapper;
using ReqResponse.DataLayer.Data.Sim;
using ReqResponse.DataLayer.DataAccess;
using ReqResponse.Models;
using ReqResponse.Services.Email;
using ReqResponse.Services.XmlAPI;

namespace ReqResponse.Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            var emailConfig = Configuration
                         .GetSection("EmailConfiguration")
                         .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddScoped<ISqlDataAccess, SqlDataAccess>();
            services.AddScoped<RequestSimDataService, RequestSimDataService>();
            services.AddScoped<RequestSqlDataService, RequestSqlDataService>();
            services.AddScoped<ResponseSimDataService, ResponseSimDataService>();
            services.AddScoped<ResponseSqlDataService, ResponseSqlDataService>();
            services.AddScoped<ResponseSummarySimDataService, ResponseSummarySimDataService>();
            services.AddScoped<ResponseSummarySqlDataService, ResponseSummarySqlDataService>();
            services.AddScoped<IDataServiceFactory, DataServiceFactory>();
            services.AddScoped<LocalXmlService, LocalXmlService>();
            services.AddScoped<ConnectedXmlService, ConnectedXmlService>();
            services.AddScoped<EmailService, EmailService>();
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<IRequestService, RequestService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}