using ReqResponse.Blazor.Models;
using ReqResponse.Blazor.Services.Email;
using ReqResponse.Blazor.Services.XmlAPI;

namespace ReqResponse.Blazor.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IXmlService _localService = null;
        private readonly IXmlService _connectedService = null;
        private readonly IEmailService _emailService = null;
        private readonly EmailConfiguration _emailConfiguration = null;

        #region constructor

        public ServiceFactory(LocalXmlService localService,
                              ConnectedXmlService connectedService,
                              EmailService emailService,
                              EmailConfiguration emailConfiguration)
        {
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