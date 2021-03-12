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
        private readonly ITestModelRequestServiceClient _serviceClient;

        public ProcessConnectedTestRequestService(ITestModelRequestServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<bool> Process()
        {
            Console.WriteLine($"Processing Connected Test Request Service {Parameters.Test} DoEmail {Parameters.DoEmail}");

            var model = await _serviceClient.LoadConnectedTestResponseAsync(true);
            while (model.IsNeedRequest() == true)
            {
                model = await _serviceClient.LoadConnectedTestResponseAsync(false);
            }
            await _serviceClient.ResetAsync(true);

            return true;
        }
    }
}