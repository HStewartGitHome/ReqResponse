using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using ReqResponse.Middleware.Services.Client.Factories;
using ReqResponse.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ReqResponse.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ErrorsView.xaml
    /// </summary>
    public partial class ErrorsView : UserControl
    {
        private ErrorsViewModel _model = null;

        public ErrorsView()
        {
            InitializeComponent();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            _model = (ErrorsViewModel)DataContext;
            ITestModelRequestServiceClient service = TestModelRequestServiceClientFactory.CreateService();
            var TestModel = await service.GetTestErrorReportAsync();
            _model.SetErrorReport(TestModel.Report);
        }


        private async void OnClickEmail(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you wish to email Error Report?", "Error Report", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ITestModelRequestServiceClient service = TestModelRequestServiceClientFactory.CreateService();
                await service.EmailTestErrorReportAsync();
            }
        }
    }
}