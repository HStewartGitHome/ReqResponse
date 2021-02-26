using ReqResponse.Models;
using System;

namespace ReqResponse.DataLayer.Models
{
    public class ResponseDataModel
    {
        public ResponseDataModel()
        {
            Id = 0;
            RequestId = 0;
            ActualValue = "";
            ActualResult = Result_Options.Unknown;
            Success = false;
            Created = DateTime.Now;
            ResponseSetId = 0;
            TimeExecuted = 0;
            RequestOption = Request_Option.None;
        }

        public ResponseDataModel(TestResponse response)
        {
            Id = response.Id;
            RequestId = response.Request.Id;
            ActualValue = response.ActualValue;
            ActualResult = response.ActualResult;
            Success = response.Success;
            Created = response.Created;
            ResponseSetId = 0;
            TimeExecuted = response.TimeExecuted;
            RequestOption = response.RequestOption;
        }

        public int Id { get; set; }
        public int RequestId { get; set; }
        public int ResponseSetId { get; set; }
        public string ActualValue { get; set; }
        public Result_Options ActualResult { get; set; }
        public bool Success { get; set; }
        public DateTime Created { get; set; }
        public int TimeExecuted { get; set; }
        public Request_Option RequestOption { get; set; }
    }
}