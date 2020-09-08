using ReqResponse.Blazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Data.Dapper
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