using ReqResponse.Blazor.Data.Dapper;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Data
{
    public interface IDataServiceFactory
    {
        Task<IResponseDataService> GetISimResponseDataService();

        Task<IRequestDataService> GetISimRequestDataService();

        Task<IRequestDataService> GetISqlRequestDataService();

        Task<IResponseDataService> GetISqlResponseDataService();

        Task<IResponseSummaryDataService> GetISqlResponseSummaryDataService();

        Task<IResponseSummaryDataService> GetISimResponseSummaryDataService();
    }
}