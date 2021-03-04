using ReqResponse.Command.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Threading.Tasks;

namespace ReqResponse.Command.Services
{
    public class ProcessTestEmailService
    {
        private readonly ITestRequestServiceClient _serviceClient;

        public ProcessTestEmailService(ITestRequestServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<bool> Process()
        {
            bool result = false;
            Console.WriteLine($"Processing Email Service {Parameters.Test} DoEmail {Parameters.DoEmail}");

            if (Parameters.DoEmail == true)
            {
                await _serviceClient.EmailTestErrorReportAsync();
                result = true;
            }
            else
                await Task.Delay(0);


            return result;
        }
    }
}