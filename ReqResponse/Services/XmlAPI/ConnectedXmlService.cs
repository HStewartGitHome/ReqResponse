
using ReqResponse.Services;
using System.Threading.Tasks;

namespace ReqResponse.Services.XmlAPI
{
    public class ConnectedXmlService : IXmlService
    {
      
        private IService _service = null;

        public ConnectedXmlService()
        {
            _service = new ConnectService();
        }

        public async Task<string> ExecuteRequest(string request)
        {
            string result = _service.ExecuteXMLRequest(request);
            await Task.Delay(0);
            return result;
        }

        public void Reset()
        {
            if (_service != null)
            {
                _service.Reset();
                _service = null;
            }
            _service = new ConnectService();
        }

        public async Task StopService()
        {
            await _service.StopService();
        }

        public async Task<bool> Connnect(string hostName,
                                         int port)
        {
            return await _service.Connnect(hostName, port);
        }

        public async Task<bool> Disconnnect()
        {
            return await _service.Disconnnect();
        }
    }
}