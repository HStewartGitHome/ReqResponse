using ReqResponse.Models;

namespace ReqResponse.Services.Methods
{
    public interface IBaseMethod
    {
        Response ExecuteRequest(Request request);

        void SetMethodInfo(MethodInfo info);
    }
}