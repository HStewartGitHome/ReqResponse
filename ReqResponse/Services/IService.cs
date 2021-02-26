using ReqResponse.Models;
using System.Threading.Tasks;

namespace ReqResponse.Services
{
    public interface IService
    {
        Result_Options LastResult { get; set; }
        bool IsConnectedService { get; set; }
        bool ExceptionHappen { get; set; }

        Task<bool> Connnect();
        Task<bool> Disconnnect();
        Response ExecuteRequest(Request request);

        string ExecuteXMLRequest(string xmlRequest);
    }
}