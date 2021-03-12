using ReqResponse.Wpf.ViewModels;
using ReqResponse.Wpf.ViewModels.Factories;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using static ReqResponse.Wpf.Models.Constants;
using static ReqResponse.Wpf.ViewModels.BaseViewModel;

namespace ReqResponse.Wpf.Commands
{
    
    public class UpdateViewCommand : AsyncCommandBase
    {
        private readonly IRootViewModelFactory _viewModelFactory;
        private readonly MainViewModel _mainViewModel;
        private ViewType _currentViewType = ViewType.Unknown;

        public UpdateViewCommand(IRootViewModelFactory viewModelFactory,
                                   MainViewModel mainViewModel)
        {
            _viewModelFactory = viewModelFactory;
            _mainViewModel = mainViewModel;
            IsExecuting = false;
        }
               
        
        

        public override void Execute(object parameter)
        {
            IsExecuting = true;
            ViewType viewType = GetViewTypeFromString((string)parameter);
            if (viewType != _currentViewType)
            {
                _mainViewModel.SelectedViewModel = _viewModelFactory.CreateViewModel(viewType);
                _currentViewType = viewType;
            }
            IsExecuting = false;
        }

        public override Task ExecuteAsync(object parameter)
        {
            Execute(parameter);
            return Task.Delay(0);
        }
    
    }
}