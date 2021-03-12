
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Threading.Tasks;

namespace ReqResponseWasm.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RemoteRequestsController
    {
        private readonly ILogger<RemoteRequestsController> _logger;
        private readonly ITestModelRequestServiceClient _service;

        public RemoteRequestsController(ILogger<RemoteRequestsController> logger,
                                                    ITestModelRequestServiceClient service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<TestViewModel> Get(string param)
        {
            bool firstTime = true;
            _logger.LogInformation($"param: {param}");
   

            TestViewModel model;
            if (param == "reset")
            {

                _logger.LogInformation("ResetAsync");

                model = await _service.ResetAsync(true);
                _logger.LogInformation($"Finish ResetAsync MaxRequests: {model.MaxRequests}  ErrorString: {model.ErrorString}");
            }

            else
            {
                if (param == "false")
                    firstTime = false;

                _logger.LogInformation($"LoadRemoteTestResponseAsync  firstTime: {firstTime}");

                model = await _service.LoadRemoteTestResponseAsync(firstTime);
                _logger.LogInformation($"Finish LoadRemoteTestResponseAsync MaxRequests: {model.MaxRequests}  ErrorString: {model.ErrorString}");
            }

            return model;
        }
    }
}