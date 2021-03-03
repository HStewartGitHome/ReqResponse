using ReqResponse.Models;
using ReqResponse.Services.Email;
using ReqResponse.Services.XmlAPI;

namespace ReqResponse.Services
{
    public interface IServiceFactory
    {
        IXmlService GetConnectedService();
        EmailConfiguration GetEmailConfiguration();
        IEmailService GetEmailService();
        IXmlService GetLocalService();
    }
}