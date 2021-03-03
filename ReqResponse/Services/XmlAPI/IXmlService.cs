using System.Threading.Tasks;

namespace ReqResponse.Services.XmlAPI
{
    public interface IXmlService
    {
        Task<bool> Connnect();
        Task<bool> Disconnnect();
        Task<string> ExecuteRequest(string request);
        void Reset();
        Task StopService();
    }
}