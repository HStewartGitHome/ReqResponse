using ReqResponse.Blazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Data
{
    public interface IRequestDataService
    {
        Task Create(TestRequest request);

        Task CreateAll(List<TestRequest> requests);

        Task DeleteAll();

        Task<List<TestRequest>> GetAll();

        Task<int> GetCount();

        Task<List<TestRequest>> GetTestRequestsAsync();
    }
}