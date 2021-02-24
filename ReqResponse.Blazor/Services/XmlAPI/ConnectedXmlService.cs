﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReqResponse.Services;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Services.XmlAPI
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
                _service = null;
            _service = new ConnectService();
        }
    }
}