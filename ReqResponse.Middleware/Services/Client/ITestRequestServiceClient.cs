using ReqResponse.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Middleware.Services.Client
{
    public interface ITestRequestServiceClient
    {
        int TakenRequests { get; set; }
        int MaxRequests { get; set; }
        string ErrorString { get; set; }

        event Action UpdateRequested;

        List<TestResponse> LoadLocalTestResponses();
        Task<List<TestResponse>> LoadLocalTestResponseAsync();
        Task<List<TestResponse>> LoadRemoteTestResponseAsync(bool firstTime);
        Task<List<TestResponse>> LoadConnectedTestResponseAsync(bool firstTime);
        void CallUpdateRequested();
        bool IsNeedRequest();
        Task<List<TestResponse>>  RefreshTestRequest();
        bool Reset(bool remote);

        Task<List<ResponseSummaryModel>> LoadResponseSummaryModelsAsync();
        Task<TestErrorReport> GetTestErrorReportAsync();
        Task EmailTestErrorReportAsync();
        Task StopService();
        Task<ResponseSummaryModel> GetReponseSummaryModelBySetIdAsync(int id);
        Task<List<TestResponse>> GetFailedResponsesForSetAsync(int id);
    }
}