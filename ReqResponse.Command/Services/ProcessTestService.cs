using ReqResponse.Command.Models;
using System;
using System.Threading.Tasks;

namespace ReqResponse.Command.Services
{
    public class ProcessTestService
    {
        private readonly ProcessLocalTestRequestService _localService;
        private readonly ProcessRemoteTestRequestService _remoteService;
        private readonly ProcessConnectedTestRequestService _connectedService;
        private readonly ProcessTestSummaryService _summaryService;
        private readonly ProcessTestErrorReportService _errorReportService;
        private readonly ProcessTestEmailService _emailService;

        public ProcessTestService(ProcessLocalTestRequestService localService,
                                  ProcessRemoteTestRequestService remoteService,
                                  ProcessConnectedTestRequestService connectedService,
                                  ProcessTestSummaryService summaryService,
                                  ProcessTestErrorReportService errorReportService,
                                  ProcessTestEmailService emailService)
        {
            _localService = localService;
            _remoteService = remoteService;
            _connectedService = connectedService;
            _summaryService = summaryService;
            _errorReportService = errorReportService;
            _emailService = emailService;
        }

        public async Task<bool> Process()
        {
            bool result = true;

            Parameters.ErrorReportNotExecuted = true;
            Console.WriteLine($"Processing Test Service DoEmail {Parameters.DoEmail}");

            foreach (Tests test in Parameters.TestToPerform)
            {
                if (await Perform(test) == false)
                {
                    result = false;
                }
                else if (test == Tests.TestErrors)
                {
                    Parameters.ErrorReportNotExecuted = false;
                }
            }

            if ((result == true) && (Parameters.DoEmail == true))
            {
                if (Parameters.ErrorReportNotExecuted == true)
                {
                    result = await _errorReportService.Process();
                }

                if (result == true)
                    result = await _emailService.Process();
            }

            return result;
        }

        public async Task<bool> Perform(Tests test)
        {
            bool result = false;

            Console.WriteLine($"Performing Test Service {test}");

            switch (test)
            {
                case Tests.TestLocalRequest:
                    result = await _localService.Process();
                    break;

                case Tests.TestRemoteRequest:
                    result = await _remoteService.Process();
                    break;

                case Tests.TestConnectedRequest:
                    result = await _connectedService.Process();
                    break;

                case Tests.TestSummary:
                    result = await _summaryService.Process();
                    break;

                case Tests.TestErrors:
                    result = await _errorReportService.Process();
                    break;

                default:
                    await Task.Delay(0);
                    break;
            }

            return result;
        }
    }
}