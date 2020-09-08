using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReqResponse.Services;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Services.XmlAPI
{
    public class LocalXmlService : IXmlService
    {
        private IConfiguration _configuration = null;
        private ILogger<LocalXmlService> _logger = null;
        private IService _service = null;

        public LocalXmlService(IConfiguration configuration,
                                ILogger<LocalXmlService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _service = new Service();
        }

        public async Task<string> ExecuteRequest(string request)
        {
            string result = "";

            result = _service.ExecuteXMLRequest(request);

            await Task.Delay(0);
            return result;
        }

        public void Reset()
        {
            if (_service != null)
                _service = null;
            _service = new Service();
        }
    }
}