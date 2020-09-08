using Microsoft.Extensions.Logging;
using ReqResponse.Blazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Data.Sim
{
    public class ResponseSummarySimDataService : IResponseSummaryDataService
    {
        private List<ResponseSummaryModel> _summaryModels = null;
        private ILogger<ResponseSummarySimDataService> _logger = null;

        public string ErrorMessage { get; set; }

        #region constructor

        public ResponseSummarySimDataService(ILogger<ResponseSummarySimDataService> logger)
        {
            _logger = logger;
            ErrorMessage = "";
        }

        #endregion constructor

        #region public methods

        public async Task Create(ResponseSummaryModel response)
        {
            await Task.Delay(0);
            if (_summaryModels == null)
                _summaryModels = new List<ResponseSummaryModel>();
            _summaryModels.Add(response);
        }

        public async Task Update(ResponseSummaryModel response)
        {
            ResponseSummaryModel foundModel = await GetByResponseSetId(response.ResponseSetId);
            if (foundModel != null)
                _summaryModels.Remove(foundModel);
            _summaryModels.Add(response);
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
            await Task.Delay(0);
            return _summaryModels;
        }

        public async Task<ResponseSummaryModel> GetById(int id)
        {
            ResponseSummaryModel foundModel = null;
            await Task.Delay(0);
            if (_summaryModels == null)
                _summaryModels = new List<ResponseSummaryModel>();

            foreach (ResponseSummaryModel model in _summaryModels)
            {
                if (model.Id == id)
                    foundModel = model;
            }

            return foundModel;
        }

        public async Task<ResponseSummaryModel> GetByResponseSetId(int id)
        {
            ResponseSummaryModel foundModel = null;
            await Task.Delay(0);
            if (_summaryModels == null)
                _summaryModels = new List<ResponseSummaryModel>();

            foreach (ResponseSummaryModel model in _summaryModels)
            {
                if (model.ResponseSetId == id)
                    foundModel = model;
            }

            return foundModel;
        }

        public async Task DeleteAll()
        {
            _summaryModels = null;
            await Task.Delay(0);
        }

        public async Task<int> GetCount()
        {
            await Task.Delay(0);
            if (_summaryModels == null)
                return 0;
            else
                return _summaryModels.Count;
        }

        #endregion public methods
    }
}