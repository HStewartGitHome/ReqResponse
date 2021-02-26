
using Microsoft.Extensions.Logging;
using ReqResponse.Models;
using ReqResponse.Support;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReqResponse.DataLayer.Models;

namespace ReqResponse.DataLayer.Data.Sim
{
    public class RequestSimDataService : IRequestDataService
    {
        private List<TestRequest> _requests = null;
        private readonly ILogger<RequestSimDataService> _logger = null;

        #region constructor

        public RequestSimDataService(ILogger<RequestSimDataService> logger)
        {
            _logger = logger;
        }

        #endregion constructor

        #region public methods

        public async Task<List<TestRequest>> GetTestRequestsAsync()
        {
            if ( (_requests == null ) || (_requests.Count < 1) )
                CreateDefaultTestRequests();

            await Task.Delay(0);
            return _requests;
        }

        public async Task Create(TestRequest request)
        {
            if (_requests == null)
                _requests = new List<TestRequest>();
            _requests.Add(request);
            await Task.Delay(0);
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

        #endregion public methods

        #region private methods

        private void CreateDefaultTestRequests()
        {
            TestRequest request;
            _requests = new List<TestRequest>();
            int Id = 1;

            // empty xml
            request = new TestRequest
            {
                Id = Id++,
                InputXml = "",
                ExpectedValue = "",
                ExpectedResult = Result_Options.NullRequest
            };
            _requests.Add(request);

            // Invalid request  need exact result
            request = CreateTestRequest("Mult", "1", "2", "2", Result_Options.InvalidRequestMethod, Id++);
            _requests.Add(request);

            // successful add requests
            request = CreateTestRequest("Add", "1", "2", "3", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Add", "400", "212", "612", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Add", "0", "0", "0", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Add", "22400", "0", "22400", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Add", "222222", "-222221", "1", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Add", "-5566", "212", "-5354", Result_Options.Ok, Id++);
            _requests.Add(request);

            // invalid add requests
            request = CreateTestRequest("Add", "1", "Test", "3", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Add", "1", "", "612", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Add", "Test", "1", "3", Result_Options.InvalidParameterValue1, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Add", "", "1", "612", Result_Options.InvalidParameterValue1, Id++);
            _requests.Add(request);

            // successful add requests
            request = CreateTestRequest("Subtract", "1", "2", "-1", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Subtract", "400", "212", "188", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Subtract", "0", "0", "0", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Subtract", "22400", "0", "22400", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Subtract", "222222", "-222221", "444443", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Subtract", "-5566", "212", "-5778", Result_Options.Ok, Id++);
            _requests.Add(request);

            // invalid subtract requests
            request = CreateTestRequest("Subtract", "1", "Test", "3", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Subtract", "1", "", "612", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Subtract", "Test", "1", "3", Result_Options.InvalidParameterValue1, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Subtract", "", "1", "612", Result_Options.InvalidParameterValue1, Id++);
            _requests.Add(request);

            // successful multiply requests
            request = CreateTestRequest("Multiply", "1", "2", "2", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Multiply", "400", "212", "84800", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Multiply", "0", "0", "0", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Multiply", "22400", "0", "0", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Multiply", "22400", "3111", "69686400", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Multiply", "-5566", "212", "-1179992", Result_Options.Ok, Id++);
            _requests.Add(request);

            // invalid Multiply requests
            request = CreateTestRequest("Multiply", "1", "Test", "3", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Multiply", "1", "", "612", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Multiply", "Test", "1", "3", Result_Options.InvalidParameterValue1, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Multiply", "", "1", "612", Result_Options.InvalidParameterValue1, Id++);
            _requests.Add(request);

            // Multiply math error
            request = CreateTestRequest("Multiply", "222222", "-222221", "-49382395062", Result_Options.MathError, Id++);
            _requests.Add(request);

            // successful divide requests
            request = CreateTestRequest("Divide", "22400", "3111", "7", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Divide", "4", "2", "2", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Divide", "512", "8", "64", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Divide", "400", "25", "16", Result_Options.Ok, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Divide", "-5566", "212", "-26", Result_Options.Ok, Id++);
            _requests.Add(request);

            // invalid Divide requests
            request = CreateTestRequest("Divide", "1", "Test", "3", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Divide", "1", "", "612", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Divide", "Test", "1", "3", Result_Options.InvalidParameterValue1, Id++);
            _requests.Add(request);
            request = CreateTestRequest("Divide", "", "1", "612", Result_Options.InvalidParameterValue1, Id++);
            _requests.Add(request);

            // Multiply Divide error ( divide by zero )
            request = CreateTestRequest("Divide", "0", "0", "0", Result_Options.InvalidParameterValue2, Id++);
            _requests.Add(request);

            // Invalid request intental error
            request = CreateTestRequest("Multiply", "11", "12", "22", Result_Options.Ok, Id++);
            _requests.Add(request);

            // intential Multiply math error with result ok
            request = CreateTestRequest("Multiply", "422222", "-232221", "-49382395062", Result_Options.Ok, Id++);
            _requests.Add(request);
        }

        private static TestRequest CreateTestRequest(string method,
                                               string value1,
                                               string value2,
                                               string result,
                                               Result_Options option,
                                               int id)
        {
            TestRequest request = new TestRequest
            {
                InputXml = XmlHelper.CreateRequestString(method, value1, value2),
                ExpectedValue = result,
                ExpectedResult = option,
                Id = id,
                Value1 = value1,
                Value2 = value2,
                Method = method
            };

            return request;
        }

        public async Task DeleteAll()
        {
            if (_requests != null)
                _requests = null;
            await Task.Delay(0);
        }

        public async Task<List<TestRequest>> GetAll()
        {
            if ( _requests == null )
                CreateDefaultTestRequests();
            await Task.Delay(0);
            return _requests;
        }

        public async Task<int> GetCount()
        {
            await Task.Delay(0);
            if (_requests == null)
                return 0;
            else
                return _requests.Count;
        }

        public async Task<int> GetNextRequestID()
        {
            await Task.Delay(0);
            if (_requests == null)
                return 1;
            else
            {
                int id = 0;
                foreach (TestRequest model in _requests)
                {
                    if (model.Id > id)
                        id = model.Id;
                }
                id++;
                return (id);
            }
        }

        #endregion private methods
    }
}