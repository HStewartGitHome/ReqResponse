using ReqResponse.Command.Models;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Command.Services
{
    public class ProcessConnectedTestRequestService
    {
        private readonly ITestRequestServiceClient _serviceClient;

        public ProcessConnectedTestRequestService(ITestRequestServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<bool> Process()
        {
            Console.WriteLine($"Processing Connected Test Request Service {Parameters.Test} DoEmail {Parameters.DoEmail}");

            List<TestResponse> list = await _serviceClient.LoadConnectedTestResponseAsync(true);
            while (_serviceClient.IsNeedRequest() == true)
            {
                list = await _serviceClient.LoadConnectedTestResponseAsync(false);
            }
            _serviceClient.Reset(true);

            return true;
        }
    }
}