using ReqResponse.DataLayer.Models;
using ReqResponse.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Services
{
    public interface IRequestService
    {
        string ErrorString { get; set; }

        int MaxRequests { get; set; }
        int TakenRequests { get; set; }

        bool IsNeedRequest();

        event Action RefreshRequested;

        void CallRequestRefresh();

        Task<List<TestResponse>> ProcessRequest(bool firstRequest,
                                                Request_Option reqOption,
                                                int requestLimit);

        Task<List<ResponseSummaryModel>> GetAllSummaryModels();
        Task<ResponseSummaryModel> GetReponseSummaryModelBySetId(int setId);
        Task<List<TestResponse>> GetFailedResponsesForSet(int setId);
        Task EmailTestErrorReport();
        Task<TestErrorReport> GetTestErrorReport();
        Task Reset(bool remote);
    }
}