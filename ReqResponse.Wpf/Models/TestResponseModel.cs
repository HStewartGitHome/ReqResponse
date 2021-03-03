using ReqResponse.DataLayer.Models;

namespace ReqResponse.Wpf.Models
{
    public class TestResponseModel
    {
        public TestResponseModel(TestResponse response)
        {
            Id = response.Id.ToString();
            RequestOption = response.RequestOption.ToString();
            Created = response.Created.ToString();
            TimeExecuted = response.TimeExecuted.ToString();
            Success = response.Success.ToString();
            ResponseSetId = response.ResponseSetId.ToString();
            RequestId = response.Request.Id.ToString();
            Method = response.Request.Method;
            Value1 = response.Request.Value1;
            Value2 = response.Request.Value2;
            ActualValue = response.ActualValue;
            ExpectedValue = response.Request.ExpectedValue;
            ExpectedResult = response.Request.ExpectedResult.ToString();
            ActualResult = response.ActualResult.ToString();
        }

        public string Id { get; set; }
        public string RequestOption { get; set; }
        public string Created { get; set; }
        public string TimeExecuted { get; set; }
        public string Success { get; set; }
        public string ResponseSetId { get; set; }
        public string RequestId { get; set; }
        public string Method { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string ActualValue { get; set; }
        public string ExpectedValue { get; set; }
        public string ExpectedResult { get; set; }
        public string ActualResult { get; set; }
    }
}