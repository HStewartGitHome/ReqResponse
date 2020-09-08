using System;

namespace ReqResponse.Models
{
    [Serializable]
    public class Response
    {
        public Result_Options Result { get; set; }
        public string ResultValue { get; set; }
    }
}