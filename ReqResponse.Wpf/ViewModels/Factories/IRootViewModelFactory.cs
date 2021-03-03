

using static ReqResponse.Wpf.Models.Constants;

namespace ReqResponse.Wpf.ViewModels.Factories
{
    public interface IRootViewModelFactory
    {
        BaseViewModel CreateViewModel(ViewType viewType);
    }
}