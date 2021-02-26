using Dapper;
using Microsoft.Extensions.Logging;
using ReqResponse.DataLayer.DataAccess;
using ReqResponse.DataLayer.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ReqResponse.DataLayer.Data.Dapper
{
    public class ResponseSqlDataService : IResponseDataService
    {
        private readonly ISqlDataAccess _dataAccess = null;
        private readonly ILogger<ResponseSqlDataService> _logger = null;

        #region constructor

        public ResponseSqlDataService(ISqlDataAccess dataAccess,
                                     ILogger<ResponseSqlDataService> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        #endregion constructor

        #region public methods

        public async Task Create(ResponseDataModel response)
        {
            var p = new DynamicParameters();
            p.Add("RequestId", response.RequestId);
            p.Add("ResponseSetId", response.ResponseSetId);
            p.Add("ActualValue", response.ActualValue);
            p.Add("ActualResult", (int)response.ActualResult);
            if (response.Success)
                p.Add("Success", "1");
            else
                p.Add("Success", "0");
            p.Add("Created", response.Created.ToString());
            p.Add("TimeExecuted", response.TimeExecuted);
            p.Add("RequestOption", (int)response.RequestOption);
            p.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("dbo.spResponses_Create", p, "SQLDB");

            response.Id = p.Get<int>("Id");
        }

        public async Task CreateAll(List<ResponseDataModel> responses)
        {
            _logger.LogInformation("Deleting all Test Response");
            await DeleteAll();
            await AddAll(responses);
        }

        public async Task AddAll(List<ResponseDataModel> responses)
        {
            _logger.LogInformation("Creating all Test Responses");
            foreach (ResponseDataModel response in responses)
                await Create(response);
            _logger.LogInformation("Finish Creating all Test Responses");
        }

        public async Task<List<ResponseDataModel>> GetAll()
        {
            var responses = await _dataAccess.LoadData<ResponseDataModel, dynamic>("dbo.spResponses_GetAll",
                                                                            new { },
                                                                            "SQLDB");
            return responses;
        }


        public async Task DeleteAll()
        {
            await _dataAccess.SaveData("dbo.spResponses_DeleteAll",
                                           new { },
                                           "SQLDB");
        }

        public async Task<int> GetNextResponseID()
        {
            var p = new DynamicParameters();
            p.Add("NextID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.LoadData<int, dynamic>("dbo.spResponses_GetNextID",
                                                     p,
                                                    "SQLDB");

            int count = p.Get<int>("NextID");
            return count;
        }

        public async Task<int> GetCount()
        {
            var p = new DynamicParameters();
            p.Add("Count", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.LoadData<int, dynamic>("dbo.spResponses_GetCount",
                                                     p,
                                                    "SQLDB");

            int count = p.Get<int>("Count");
            return count;
        }

        #endregion public methods
    }
}