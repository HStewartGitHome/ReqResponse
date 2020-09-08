using Microsoft.Extensions.Logging;
using ReqResponse.Blazor.Data.Dapper;
using ReqResponse.Blazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Data.Sim
{
    public class ResponseSimDataService : IResponseDataService
    {
        private List<ResponseDataModel> _responses = null;
        private ILogger<ResponseSimDataService> _logger = null;

        #region constructor

        public ResponseSimDataService(ILogger<ResponseSimDataService> logger)
        {
            _logger = logger;
        }

        #endregion constructor

        public async Task Create(ResponseDataModel response)
        {
            if (_responses == null)
                _responses = new List<ResponseDataModel>();
            _responses.Add(response);
            await Task.Delay(0);
        }

        public async Task CreateAll(List<ResponseDataModel> responses)
        {
            _logger.LogInformation("Deleting all Test Response");
            await DeleteAll();
            _logger.LogInformation("Creating all Test Responses");
            foreach (ResponseDataModel response in responses)
                await Create(response);
            _logger.LogInformation("Finish Creating all Test Responses");
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
            await Task.Delay(0);
            return _responses;
        }

        public async Task DeleteAll()
        {
            _responses = null;
            await Task.Delay(0);
        }

        public async Task<int> GetNextResponseID()
        {
            await Task.Delay(0);
            if (_responses == null)
                return 1;
            else
            {
                int id = 0;
                foreach (ResponseDataModel model in _responses)
                {
                    if (model.Id > id)
                        id = model.Id;
                }
                id++;
                return (id);
            }
        }

        public async Task<int> GetCount()
        {
            await Task.Delay(0);
            if (_responses == null)
                return 0;
            else
                return _responses.Count;
        }
    }
}