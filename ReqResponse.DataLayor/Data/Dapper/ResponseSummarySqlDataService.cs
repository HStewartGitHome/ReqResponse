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
    public class ResponseSummarySqlDataService : IResponseSummaryDataService
    {
        private readonly ISqlDataAccess _dataAccess = null;
        private readonly ILogger<ResponseSummarySqlDataService> _logger = null;

        public string ErrorMessage { get; set; }

        #region constructor

        public ResponseSummarySqlDataService(ISqlDataAccess dataAccess,
                                             ILogger<ResponseSummarySqlDataService> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
            ErrorMessage = "";
        }

        #endregion constructor

        #region public methods

        public async Task Create(ResponseSummaryModel response)
        {
            var p = new DynamicParameters();
            p.Add("ResponseSetId", response.ResponseSetId);
            p.Add("SuccessfullCount", response.SuccessfullCount);
            p.Add("FailedCount", response.FailedCount.ToString());
            p.Add("OkCount", response.OkCount.ToString());
            p.Add("ErrorCount", response.ErrorCount.ToString());
            p.Add("Created", response.Created.ToString());
            p.Add("TimeExecuted", response.TimeExecuted);
            p.Add("RequestOption", (int)response.RequestOption);
            p.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("dbo.spResponseSummary_Create", p, "SQLDB");
            response.Id = p.Get<int>("Id");
        }

        public async Task Update(ResponseSummaryModel response)
        {
            var p = new DynamicParameters();
            p.Add("Id", response.Id);
            p.Add("ResponseSetId", response.ResponseSetId);
            p.Add("SuccessfullCount", response.SuccessfullCount);
            p.Add("FailedCount", response.FailedCount.ToString());
            p.Add("OkCount", response.OkCount.ToString());
            p.Add("ErrorCount", response.ErrorCount.ToString());
            p.Add("Created", response.Created.ToString());
            p.Add("TimeExecuted", response.TimeExecuted);
            p.Add("RequestOption", (int)response.RequestOption);

            await _dataAccess.SaveData("dbo.spResponseSummary_Update", p, "SQLDB");
        }

        public async Task CreateAll(List<ResponseSummaryModel> responses)
        {
            _logger.LogInformation("Deleting all Test Response Summary");
            await DeleteAll();
            await AddAll(responses);
        }

        public async Task AddAll(List<ResponseSummaryModel> responses)
        {
            _logger.LogInformation("Creating all Test Response Summary");
            foreach (ResponseSummaryModel response in responses)
                await Create(response);
            _logger.LogInformation("Finish Creating all Test Response Summary");
        }

        public async Task<List<ResponseSummaryModel>> GetAll()
        {
            var responses = await _dataAccess.LoadData<ResponseSummaryModel, dynamic>("dbo.spResponseSummary_GetAll",
                                                                            new { },
                                                                            "SQLDB");
            return responses.ToList<ResponseSummaryModel>();
        }

        public async Task<ResponseSummaryModel> GetById(int id)
        {
            var response = await _dataAccess.LoadData<ResponseSummaryModel, dynamic>("dbo.spResponseSummary_GetById",
                                                          new { Id = id },
                                                          "SQLDB");
            return response.FirstOrDefault();
        }

        public async Task<ResponseSummaryModel> GetByResponseSetId(int id)
        {
            var response = await _dataAccess.LoadData<ResponseSummaryModel, dynamic>("dbo.spResponseSummary_GetByResponseSetId",
                                                          new { ResponseSetId = id },
                                                          "SQLDB");
            return response.FirstOrDefault();
        }

        public async Task<int> GetCount()
        {
            var p = new DynamicParameters();
            p.Add("Count", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.LoadData<int, dynamic>("dbo.spResponseSummary_GetCount",
                                                     p,
                                                    "SQLDB");

            int count = p.Get<int>("Count");
            return count;
        }

        public async Task DeleteAll()
        {
            await _dataAccess.SaveData("dbo.spResponseSummary_DeleteAll",
                                           new { },
                                           "SQLDB");
        }

        #endregion public methods
    }
}