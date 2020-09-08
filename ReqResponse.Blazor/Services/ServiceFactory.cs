using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReqResponse.Blazor.Models;
using ReqResponse.Blazor.Services.Email;
using ReqResponse.Blazor.Services.XmlAPI;

namespace ReqResponse.Blazor.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private IConfiguration _configuration = null;
        private ILogger<ServiceFactory> _logger = null;
        private IXmlService _localService = null;
        private IXmlService _connectedService = null;
        private IEmailService _emailService = null;
        private EmailConfiguration _emailConfiguration = null;

        #region constructor

        public ServiceFactory(IConfiguration configuration,
                              ILogger<ServiceFactory> logger,
                              LocalXmlService localService,
                              ConnectedXmlService connectedService,
                              EmailService emailService,
                              EmailConfiguration emailConfiguration)
        {
            _configuration = configuration;
            _logger = logger;
            _localService = localService;
            _connectedService = connectedService;
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
        }

        #endregion constructor

        public IXmlService GetLocalService()
        {
            return _localService;
        }

        public IXmlService GetConnectedService()
        {
            return _connectedService;
        }

        public IEmailService GetEmailService()
        {
            return _emailService;
        }

        public EmailConfiguration GetEmailConfiguration()
        {
            return _emailConfiguration;
        }
    }
}