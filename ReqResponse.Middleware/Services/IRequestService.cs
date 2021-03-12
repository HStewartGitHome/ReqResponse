
using ReqResponse.DataLayer.Models;
using ReqResponse.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Middleware.Services
{
    public interface IRequestService
    {
        string ErrorString { get; set; }

        int MaxRequests { get; set; }
        int TakenRequests { get; set; }

        bool IsNeedRequest();


        Task<List<TestResponse>> ProcessRequest(bool firstRequest,
                                                Request_Option reqOption,
                                                int requestLimit);

        Task<List<ResponseSummaryModel>> GetAllSummaryModels();
        Task<ResponseSummaryModel> GetReponseSummaryModelBySetId(int setId);
        Task<List<TestResponse>> GetFailedResponsesForSet(int setId);
        Task EmailTestErrorReportAsync();
        Task<TestErrorReport> GetTestErrorReportAsync();
        void Reset(bool remote);
        Task StopService();
    }
}