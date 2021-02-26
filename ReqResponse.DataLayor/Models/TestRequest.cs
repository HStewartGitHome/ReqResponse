using ReqResponse.Models;

namespace ReqResponse.DataLayer.Models
{
    public class TestRequest
    {
        public int Id { get; set; }
        public string InputXml { get; set; }
        public string Method { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }

        public Result_Options ExpectedResult { get; set; }
        public string ExpectedValue { get; set; }
    }
}