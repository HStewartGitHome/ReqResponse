using ReqResponse.Command.Models;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Command.Services
{
    public class ProcessRemoteTestRequestService
    {
        private readonly ITestModelRequestServiceClient _serviceClient;

        public ProcessRemoteTestRequestService(ITestModelRequestServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<bool> Process()
        {
            Console.WriteLine($"Processing Remote Test Request Service {Parameters.Test} DoEmail {Parameters.DoEmail}");

            var model = await _serviceClient.LoadRemoteTestResponseAsync(true);
            while (model.IsNeedRequest() == true)
            {
                model = await _serviceClient.LoadRemoteTestResponseAsync(false);
            }
            await _serviceClient.ResetAsync(true);

            return true;
        }
    }
}