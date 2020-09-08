using ReqResponse.Blazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Data
{
    public interface IResponseSummaryDataService
    {
        string ErrorMessage { get; set; }

        Task AddAll(List<ResponseSummaryModel> responses);

        Task Create(ResponseSummaryModel response);

        Task CreateAll(List<ResponseSummaryModel> responses);

        Task DeleteAll();

        Task<List<ResponseSummaryModel>> GetAll();

        Task<ResponseSummaryModel> GetById(int id);

        Task<ResponseSummaryModel> GetByResponseSetId(int id);
        Task<int> GetCount();
        Task Update(ResponseSummaryModel response);
    }
}