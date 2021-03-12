using System.Threading.Tasks;

namespace ReqResponse.Services.XmlAPI
{
    public interface IXmlService
    {
        Task<bool> Connnect(string hostName,
                            int port);
        Task<bool> Disconnnect();
        Task<string> ExecuteRequest(string request);
        void Reset();
        Task StopService();
    }
}