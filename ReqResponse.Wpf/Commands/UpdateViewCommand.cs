using ReqResponse.Wpf.ViewModels;
using ReqResponse.Wpf.ViewModels.Factories;
using System;
using System.Windows.Input;
using static ReqResponse.Wpf.Models.Constants;
using static ReqResponse.Wpf.ViewModels.BaseViewModel;

namespace ReqResponse.Wpf.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private readonly IRootViewModelFactory _viewModelFactory;
        private readonly MainViewModel _mainViewModel;
        private ViewType _currentViewType = ViewType.Unknown;
      

        public UpdateViewCommand(IRootViewModelFactory viewModelFactory,
                                   MainViewModel mainViewModel)
        {
            _viewModelFactory = viewModelFactory;
            _mainViewModel = mainViewModel;
        }

      

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            ViewType viewType = GetViewTypeFromString((string)parameter);
            if (viewType != _currentViewType)
            {
                _mainViewModel.SelectedViewModel = _viewModelFactory.CreateViewModel(viewType);
                _currentViewType = viewType;
            }
        }
    }
}