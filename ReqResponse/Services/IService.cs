using ReqResponse.Models;

namespace ReqResponse.Services
{
    public interface IService
    {
        Result_Options LastResult { get; set; }
        bool IsConnectedService { get; set; }

        Response ExecuteRequest(Request request);

        string ExecuteXMLRequest(string xmlRequest);
    }
}