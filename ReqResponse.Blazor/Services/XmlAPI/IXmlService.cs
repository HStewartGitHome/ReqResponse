using System.Threading.Tasks;

namespace ReqResponse.Blazor.Services.XmlAPI
{
    public interface IXmlService
    {
        Task<string> ExecuteRequest(string request);
        void Reset();
    }
}