using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReqResponseWasm.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ErrorReportController : Controller
    {
        private readonly ILogger<SummaryRequestsController> _logger;
        private readonly ITestModelRequestServiceClient _service;

        public ErrorReportController(ILogger<SummaryRequestsController> logger,
                                        ITestModelRequestServiceClient service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<TestViewModel> Get(string param)
        {
            string cmd = "load";
            int id = 1;
            string[] values = param.Split(',').Select(sValue => sValue.Trim()).ToArray();
            if (values.Length > 0)
            {
                cmd = values[0];
                if (values.Length > 1)
                    id = Convert.ToInt32(values[1]);
            }
            _logger.LogInformation($"line=[{param}] cmd={cmd} id={id}");

            TestViewModel model = null;
            if (cmd == "load")
            {
                _logger.LogInformation("GetTestErrorReportAsync");

                model = await _service.GetTestErrorReportAsync();
                _logger.LogInformation($"Finish GetTestErrorReportAsync MaxRequests: {model.MaxRequests}  ErrorString: {model.ErrorString}");
            }

            else  if (cmd == "email")
            {
                _logger.LogInformation("EmailTestErrorReportAsync");

                 model = await _service.EmailTestErrorReportAsync();
                _logger.LogInformation($"Finish EmailTestErrorReportAsync MaxRequests: {model.MaxRequests}  ErrorString: {model.ErrorString}");
            }
            
            return model;
        }
    }
}
