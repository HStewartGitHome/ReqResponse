using ReqResponse.DataLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.DataLayer.Data
{
    public interface IResponseDataService
    {
        Task AddAll(List<ResponseDataModel> responses);

        Task Create(ResponseDataModel response);

        Task CreateAll(List<ResponseDataModel> responses);

        Task DeleteAll();

        Task<int> GetNextResponseID();

        Task<int> GetCount();
        Task<List<ResponseDataModel>> GetAll();
    }
}