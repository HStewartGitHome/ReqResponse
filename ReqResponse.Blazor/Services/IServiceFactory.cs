using ReqResponse.Blazor.Models;
using ReqResponse.Blazor.Services.Email;
using ReqResponse.Blazor.Services.XmlAPI;

namespace ReqResponse.Blazor.Services
{
    public interface IServiceFactory
    {
        IXmlService GetConnectedService();
        EmailConfiguration GetEmailConfiguration();
        IEmailService GetEmailService();
        IXmlService GetLocalService();
    }
}