using ReqResponse.DataLayer.Models;
using ReqResponse.Wpf.Models;
using System.Collections.Generic;

namespace ReqResponse.Wpf.ViewModels
{
    public class CommonResponseViewModel : BaseViewModel
    {
        public List<TestResponse> TestResponseList { get; set; }
       
        public List<TestResponseModel> TestResponseModelList { get; set; }

        public int TakenRequests { get; set;  }
        public int MaxRequests { get; set; }

        public CommonResponseViewModel()
        {
            TakenRequests = 0;
            MaxRequests = 0;
            SetTitleMessage();
        }

        public void SetTestResponseList(List<TestResponse> list)
        {
            TestResponseList = list;
            TestResponseModelList = new List<TestResponseModel>();
            foreach (TestResponse response in list)
            {
                TestResponseModel model = new TestResponseModel(response);
                TestResponseModelList.Add(model);
            }
            TakenRequests = TestResponseModelList.Count;
        }
    }
}