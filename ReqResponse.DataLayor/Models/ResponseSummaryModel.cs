using ReqResponse.Models;
using System;

namespace ReqResponse.DataLayer.Models
{
    public class ResponseSummaryModel
    {
        public ResponseSummaryModel()
        {
            FailedCount = 0;
            SuccessfullCount = 0;
            OkCount = 0;
            ErrorCount = 0;
            Created = DateTime.Now;
            ResponseSetId = 0;
            TimeExecuted = 0;
            RequestOption = Request_Option.None;
        }

        public int Id { get; set; }
        public int ResponseSetId { get; set; }
        public int SuccessfullCount { get; set; }
        public int FailedCount { get; set; }
        public int OkCount { get; set; }
        public int ErrorCount { get; set; }
        public DateTime Created { get; set; }
        public int TimeExecuted { get; set; }
        public Request_Option RequestOption { get; set; }
    }
}