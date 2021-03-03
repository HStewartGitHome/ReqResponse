namespace ReqResponse.Wpf.ViewModels
{
    public class LocalViewModel : CommonResponseViewModel
    {
        public override void SetTitleMessage()
        {
            if (TakenRequests == 0)
                TitleMessage = "Local ReqResponse Screen";
            else if (TakenRequests >= MaxRequests)
                TitleMessage = "Local ReqResponse Screen Finish";
        }
    }
}