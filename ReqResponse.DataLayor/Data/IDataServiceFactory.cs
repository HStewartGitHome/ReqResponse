using ReqResponse.DataLayer.Data;
using System.Threading.Tasks;

namespace ReqResponse.DataLayer.Data
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