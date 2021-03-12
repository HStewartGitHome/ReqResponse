using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ReqResponseWasm.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalRequestsController : Controller
    {
        private readonly ILogger<LocalRequestsController> _logger;
        private readonly ITestModelRequestServiceClient _service;

        public LocalRequestsController(ILogger<LocalRequestsController> logger,
                                      ITestModelRequestServiceClient service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<TestViewModel> Get()
        {
            _logger.LogInformation("Loading LocalTestResonseSync");
            TestViewModel model = await _service.LoadLocalTestResponseAsync();
            _logger.LogInformation("Finish LocalTestResonseSync MaxRequests: {model.MaxRequests}  ErrorString: {model.ErrorString}");
            return model;
        }
    }
}