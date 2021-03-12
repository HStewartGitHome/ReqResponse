using ReqResponse.Command.Models;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Command.Services
{
    public class ProcessTestSummaryService
    {
        private readonly ITestModelRequestServiceClient _serviceClient;

        public ProcessTestSummaryService(ITestModelRequestServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<bool> Process()
        {
            Console.WriteLine($"Processing Local Test Request Service {Parameters.Test} DoEmail {Parameters.DoEmail}");

            var list = await _serviceClient.LoadLocalTestResponseAsync();

            return true;
        }
    }
}