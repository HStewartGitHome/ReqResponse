using ReqResponse.DataLayer.Models;
using ReqResponse.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Middleware.Services.Client
{
    public class TestRequestServiceClient : ITestRequestServiceClient
    {
        public IRequestService _service;
        private bool IsStopping = true;

        public int TakenRequests { get; set; }
        public int MaxRequests { get; set; }

        public TestRequestServiceClient(IRequestService service)
        {
            _service = service;
        }

        #region TestResponse methods

        public event Action UpdateRequested;

        public void CallUpdateRequested()
        {
            UpdateRequested?.Invoke();
        }

        public async Task<List<TestResponse>> LoadLocalTestResponseAsync()
        {
            List<TestResponse> list = await LoadActualTestResponseAsync();

            //List<TestResponse> list = LoadTestResponse();
            //await Task.Delay(0);
            return list;
        }

        public async Task<List<TestResponse>> LoadActualTestResponseAsync()
        {
            IsStopping = true;
            List<TestResponse> list = await _service.ProcessRequest(true, Request_Option.Local, 9999);
            TakenRequests = _service.TakenRequests;
            MaxRequests = _service.MaxRequests;
            return list;
        }

        public List<TestResponse> LoadLocalTestResponses()
        {
            List<TestResponse> list = new List<TestResponse>();

            return list;
        }

        public async Task<List<TestResponse>> LoadRemoteTestResponseAsync(bool firstTime)
        {
            List<TestResponse> list = new List<TestResponse>();

            try
            {
                if (firstTime == true)
                {
                    Reset(true);
                    IsStopping = false;
                }

                list = await _service.ProcessRequest(firstTime, Request_Option.Connected, 9999);
                TakenRequests = _service.TakenRequests;
                MaxRequests = _service.MaxRequests;

                if (IsNeedRequest() == true)
                    CallUpdateRequested();
            }
            catch( Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            return list;
        }

        public async Task<List<TestResponse>> LoadConnectedTestResponseAsync(bool firstTime)
        {
            List<TestResponse> list = new List<TestResponse>();

            try
            {
                if (firstTime == true)
                {
                    Reset(true);
                    IsStopping = false;
                }

                list = await _service.ProcessRequest(firstTime, Request_Option.StayConnected, 9999);
                TakenRequests = _service.TakenRequests;
                MaxRequests = _service.MaxRequests;

                if (IsNeedRequest() == true)
                    CallUpdateRequested();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            return list;
          
        }

        public bool IsNeedRequest()
        {
            if (IsStopping == true)
                return false;
            if (_service == null)
                return false;
            return _service.IsNeedRequest();
        }

        public async Task<List<TestResponse>> RefreshTestRequest()
        {
            List<TestResponse> list;

            if (IsNeedRequest() == true)
            {
                list = await LoadRemoteTestResponseAsync(true);
            }
            else
                list = new List<TestResponse>();

            return list;
        }

        public bool Reset(bool remote)
        {
            if (_service == null)
                return false;
            _service.Reset(remote);
            return true;
        }

        public async Task StopService()
        {
            IsStopping = true;
            if (_service != null)
                await _service.StopService();
            else
                await Task.Delay(0);

        }



        #endregion TestResponse methods

        #region ResponseSummaryModels methods

        public async Task<List<ResponseSummaryModel>> LoadResponseSummaryModelsAsync()
        {

            List<ResponseSummaryModel> responses = await _service.GetAllSummaryModels();
            return responses;
        }

        #endregion ResponseSummaryModels methods

        #region TestErrorReport

        public async Task<TestErrorReport> GetTestErrorReportAsync()
        {
            TestErrorReport report = await _service.GetTestErrorReportAsync();
            return report;
        }

        public async Task EmailTestErrorReportAsync()
        {
            await _service.EmailTestErrorReportAsync();
        }
        #endregion TestErrorReport
    }
}