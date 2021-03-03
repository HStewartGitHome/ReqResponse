using ReqResponse.Wpf.Commands;
using ReqResponse.Wpf.ViewModels.Factories;
using System.Windows.Input;

namespace ReqResponse.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region SelectedViewModel
        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        #endregion

        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel(IRootViewModelFactory viewModelFactory)
        {
            UpdateViewCommand = new UpdateViewCommand(viewModelFactory, this);
            UpdateViewCommand.Execute("Home");
          
        }
    }
}