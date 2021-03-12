using System;
using System.Collections.Generic;

namespace ReqResponse.DataLayer.Models
{
    public class TestViewModel
    {
        public List<TestResponse> Responses { get; set; }
        public List<ResponseSummaryModel> Summaries { get; set; }
        public ResponseSummaryModel Summary { get; set; }
        public TestErrorReport Report { get; set;  }
        public int MaxRequests { get; set; }
        public int CurrentMaxRequests { get; set; }
        public int RequestCount { get; set; }
        public int TakenRequests { get; set; }
        public int CurrentTakenRequests { get; set; }
        public string ErrorString { get; set; }
        public bool  IsNeedRequest()
        {
            bool result = false;
            if ((CurrentTakenRequests < CurrentMaxRequests) && (CurrentMaxRequests != 0))
                result = true;

            return result;
        }

        public void AddCounts(int taken,
                               int max,
                               int listCount,
                               string errorString,
                               bool firstTime)
        {
            if (firstTime == true)
            {
                MaxRequests = max;
                CurrentMaxRequests = max;   
            }
            else
            {
                TakenRequests = taken;
                CurrentMaxRequests = max;
            }

            TakenRequests = taken;
            CurrentTakenRequests = taken;
            RequestCount = listCount;
            ErrorString = errorString;
        }

        public void AddResponses( List<TestResponse> list)
        {
            foreach (TestResponse obj in list)
                Responses.Add(obj);
            CurrentTakenRequests = Responses.Count;
        }
    }
}