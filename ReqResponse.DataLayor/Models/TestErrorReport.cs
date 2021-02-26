using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ReqResponse.DataLayer.Models
{
    public class TestErrorReport
    {
        public TestErrorReport()
        {
            ErrorCount = 0;
            ErrorSet = 0;
            Created = DateTime.Now;
            CurrentLastErrorDateTime = DateTime.Now;
            LastErrorDateTime = DateTime.Now;
            Message = "1 error(s) occurred";
        }
        public int ErrorCount { get; set; }
        public int ErrorSet { get; set; }
        public DateTime CurrentLastErrorDateTime { get; set; }
        public DateTime LastErrorDateTime { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
    }
}
