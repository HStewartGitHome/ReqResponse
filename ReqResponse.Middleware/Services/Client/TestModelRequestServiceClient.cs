using ReqResponse.DataLayer.Models;
using System;
using System.Threading.Tasks;

namespace ReqResponse.Middleware.Services.Client
{
    public class TestModelRequestServiceClient : ITestModelRequestServiceClient
    {
        public readonly ITestRequestServiceClient _service;

        public TestModelRequestServiceClient(ITestRequestServiceClient service)

        {
            _service = service;
        }

        public async Task<TestViewModel> LoadLocalTestResponseAsync()
        {
            TestViewModel model = new();
            model.Responses = await _service.LoadLocalTestResponseAsync();
            SetModelVariables(model,
                         model.Responses.Count,
                         true);

            return model;
        }

        public async Task<TestViewModel> LoadResponseSummaryModelsAsync()
        {
            TestViewModel model = new();
            model.Summaries = await _service.LoadResponseSummaryModelsAsync();
            SetModelVariables(model,
                         model.Summaries.Count,
                         true);
            return model;
        }

        public async Task<TestViewModel> GetReponseSummaryModelBySetIdAsync(int id)
        {
            TestViewModel model = new();
            model.Summary = await _service.GetReponseSummaryModelBySetIdAsync(id);
            SetModelVariables(model,
                         1,
                         true);
            return model;
        }

        public async Task<TestViewModel> GetFailedResponsesForSetAsync(int id)
        {
            TestViewModel model = new();
            model.Responses = await _service.GetFailedResponsesForSetAsync(id);
            SetModelVariables(model,
                         model.Responses.Count,
                         true);
            return model;
        }

        public async Task<TestViewModel> GetTestErrorReportAsync()
        {
            TestViewModel model = new();
            model.Report = await _service.GetTestErrorReportAsync();
            SetModelVariables(model,
                              0,
                              true);

            return model;
        }

        public async Task<TestViewModel> EmailTestErrorReportAsync()
        {
            TestViewModel model = new();
            await _service.EmailTestErrorReportAsync();
            SetModelVariables(model,
                              0,
                              true);

            return model;
        }

        public async Task<TestViewModel> LoadRemoteTestResponseAsync(bool firstTime)
        {
            TestViewModel model = new();
            model.Responses = await _service.LoadRemoteTestResponseAsync(firstTime);
            SetModelVariables(model,
                         model.Responses.Count,
                         firstTime);
            return model;
        }

        public async Task<TestViewModel> LoadConnectedTestResponseAsync(bool firstTime)
        {
            TestViewModel model = new();
            model.Responses = await _service.LoadConnectedTestResponseAsync(firstTime);
            SetModelVariables(model,
                              model.Responses.Count,
                              firstTime);
            return model;
        }

        public async Task<TestViewModel>  ResetAsync(bool remote)
        {
            TestViewModel model = new();
            _service.Reset(remote);
            SetModelVariables(model,
                              1,
                              true);
            await Task.Delay(0);
            return model;
        }

        private void SetModelVariables(TestViewModel model,
                                       int listCount,
                                       bool firstTime)
        {
            model.AddCounts(_service.TakenRequests,
                            _service.MaxRequests,
                           listCount,
                           _service.ErrorString,
                           firstTime);
           

            Console.WriteLine($"Taken Requests {model.TakenRequests}  MaxRequests {model.MaxRequests} CurrentMaxRequests {model.CurrentMaxRequests} Response Count {model.RequestCount} firstTime: {firstTime}");
        }
    }
}