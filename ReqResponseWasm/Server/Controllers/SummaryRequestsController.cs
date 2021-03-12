using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReqResponseWasm.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SummaryRequestsController
    {
        private readonly ILogger<SummaryRequestsController> _logger;
        private readonly ITestModelRequestServiceClient _service;

        public SummaryRequestsController(ILogger<SummaryRequestsController> logger,
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
                _logger.LogInformation("Loading ResponseModelSummaryModelsAsync");

                 model = await _service.LoadResponseSummaryModelsAsync();
                _logger.LogInformation($"Finish ResponseModelSummaryModelsAsync MaxRequests: {model.MaxRequests}  ErrorString: {model.ErrorString}");
            }
            else if (cmd == "byid")
            {
                _logger.LogInformation($"GetReponseSummaryModelBySetIdAsync with id {id}");

                model = await _service.GetReponseSummaryModelBySetIdAsync(id);
                _logger.LogInformation($"Finish GetReponseSummaryModelBySetIdAsync MaxRequests: {model.MaxRequests}  ErrorString: {model.ErrorString}");
            }
            else if (cmd == "failed")
            {
                _logger.LogInformation($"GetFailedResponsesModelForSet with id {id}");

                model = await _service.GetFailedResponsesForSetAsync(id);
                _logger.LogInformation($"Finish failed  MaxRequests: {model.MaxRequests}  ErrorString: {model.ErrorString}");
            }
            return model;
        }
    }
}