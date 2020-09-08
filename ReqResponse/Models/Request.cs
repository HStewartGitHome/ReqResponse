using System;

namespace ReqResponse.Models
{
    [Serializable]
    public class Request
    {
        public string Method { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }
}