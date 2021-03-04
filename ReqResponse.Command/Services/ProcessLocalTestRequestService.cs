using ReqResponse.Command.Models;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Command.Services
{
    public class ProcessLocalTestRequestService 
    {
        private readonly ITestRequestServiceClient _serviceClient;
        public ProcessLocalTestRequestService(ITestRequestServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<bool> Process()
        {
            Console.WriteLine($"Processing Local Test Request Service {Parameters.Test} DoEmail {Parameters.DoEmail}");

            List<TestResponse> list = await _serviceClient.LoadLocalTestResponseAsync();
            

            return true;
        }
    }
}