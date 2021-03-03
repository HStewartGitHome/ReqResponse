using ReqResponse.DataLayer.Models;
using System.Collections.Generic;

namespace ReqResponse.Wpf.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {
        public List<ResponseSummaryModel> ResponseSummaryModelList { get; set; }

        public SummaryViewModel()
        {
            SetTitleMessage();
        }

        public override void SetTitleMessage()
        {
            TitleMessage = "Summary ReqResponse Screen";
        }

        public void SetResponseSummaryModelList(List<ResponseSummaryModel> list)
        {
            ResponseSummaryModelList = list;
        }

    }
}