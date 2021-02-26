using Dapper;
using Microsoft.Extensions.Logging;
using ReqResponse.DataLayer.DataAccess;
using ReqResponse.DataLayer.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ReqResponse.DataLayer.Data.Dapper
{
    public class RequestSqlDataService : IRequestDataService
    {
        private readonly ISqlDataAccess _dataAccess = null;
        private readonly ILogger<RequestSqlDataService> _logger = null;

        #region constructor

        public RequestSqlDataService(ISqlDataAccess dataAccess,
                                     ILogger<RequestSqlDataService> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        #endregion constructor

        #region public methods

        public async Task<List<TestRequest>> GetTestRequestsAsync()
        {
            return await GetAll();
        }

        public async Task<List<TestRequest>> GetAll()
        {
            var requests = await _dataAccess.LoadData<TestRequest, dynamic>("dbo.spRequests_GetAll",
                                                                            new { },
                                                                            "SQLDB");
            return requests.ToList<TestRequest>();
        }

        public async Task Create(TestRequest request)
        {
            var p = new DynamicParameters();
            p.Add("InputXml", request.InputXml);
            p.Add("Method", request.Method);
            p.Add("Value1", request.Value1);
            p.Add("Value2", request.Value2);
            p.Add("ExpectedResult", (int)request.ExpectedResult);
            p.Add("ExpectedValue", request.ExpectedValue);
            p.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("dbo.spRequests_Create", p, "SQLDB");

            request.Id = p.Get<int>("Id");
        }

        public async Task CreateAll(List<TestRequest> requests)
        {
            _logger.LogInformation("Deleting all Test Requests");
            await DeleteAll();
            _logger.LogInformation("Creating all Test Requests");
            foreach (TestRequest request in requests)
                await Create(request);
            _logger.LogInformation("Finish Creating all Test Requests");
        }

        public async Task DeleteAll()
        {
            await _dataAccess.SaveData("dbo.spRequests_DeleteAll",
                                           new { },
                                           "SQLDB");
        }

        public async Task<int> GetCount()
        {
            var p = new DynamicParameters();
            p.Add("Count", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.LoadData<int, dynamic>("dbo.spRequests_GetCount",
                                                     p,
                                                    "SQLDB");

            int count = p.Get<int>("Count");
            return count;
        }

        public async Task<int> GetNextRequestID()
        {
            var p = new DynamicParameters();
            p.Add("NextID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.LoadData<int, dynamic>("dbo.spRequests_GetNextID",
                                                     p,
                                                    "SQLDB");

            int count = p.Get<int>("NextID");
            return count;
        }

        #endregion public methods
    }
}