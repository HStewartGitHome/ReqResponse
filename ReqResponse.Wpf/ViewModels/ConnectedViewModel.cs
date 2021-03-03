namespace ReqResponse.Wpf.ViewModels
{
    public class ConnectedViewModel : CommonResponseViewModel
    {
        public override void SetTitleMessage()
        {
            if (TakenRequests == 0)
                TitleMessage = "Connected ReqResponse Screen";
            else if (TakenRequests >= MaxRequests)
                TitleMessage = "Connected ReqResponse Screen Finish";
            else
                TitleMessage = $"Connected ReqResponse Screen {TakenRequests} of {MaxRequests}";
        }
    }
}