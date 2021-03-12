using ReqResponse.DataLayer.Models;
using System.Threading.Tasks;

namespace ReqResponse.Middleware.Services.Client
{
    public interface ITestModelRequestServiceClient
    {
        Task<TestViewModel> EmailTestErrorReportAsync();
        Task<TestViewModel> GetFailedResponsesForSetAsync(int id);
        Task<TestViewModel> GetReponseSummaryModelBySetIdAsync(int id);
        Task<TestViewModel> GetTestErrorReportAsync();
        Task<TestViewModel> LoadConnectedTestResponseAsync(bool firstTime);
        Task<TestViewModel> LoadLocalTestResponseAsync();
        Task<TestViewModel> LoadRemoteTestResponseAsync(bool firstTime);
        Task<TestViewModel> LoadResponseSummaryModelsAsync();
        Task<TestViewModel> ResetAsync(bool remote);
    }
}