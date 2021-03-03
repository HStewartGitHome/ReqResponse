namespace ReqResponse.Wpf.ViewModels
{
    public class RemoteViewModel : CommonResponseViewModel
    {
        public override void SetTitleMessage()
        {
            if (TakenRequests == 0)
                TitleMessage = "Remote ReqResponse Screen";
            else if (TakenRequests >= MaxRequests)
                TitleMessage = "Remote ReqResponse Screen Finish";
            else
                TitleMessage = $"Remote ReqResponse Screen {TakenRequests} of {MaxRequests}";
        }
    }
}