using ReqResponse.Command.Models;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Threading.Tasks;

namespace ReqResponse.Command.Services
{
    public class ProcessTestErrorReportService
    {
        private readonly ITestRequestServiceClient _serviceClient;

        public ProcessTestErrorReportService(ITestRequestServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<bool> Process()
        {
            Console.WriteLine($"Processing Local Test Request Service {Parameters.Test} DoEmail {Parameters.DoEmail}");

            TestErrorReport report = await _serviceClient.GetTestErrorReportAsync();

            return true;
        }
    }
}