using Microsoft.AspNetCore.Http.Features;
using ReqResponse.Models;
using System;

namespace ReqResponse.Blazor.Models
{
    public class TestResponse
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int ResponseSetId { get; set; }
        public TestRequest Request { get; set; }
        public int RequestId { get; set; }
        public bool IsRemote { get; set; }
        public string ActualValue { get; set; }
        public Result_Options ActualResult { get; set; }
        public bool Success { get; set; }
    }
}