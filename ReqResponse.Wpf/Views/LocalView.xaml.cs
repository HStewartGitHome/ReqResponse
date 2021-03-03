using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using ReqResponse.Middleware.Services.Client.Factories;
using ReqResponse.Wpf.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ReqResponse.Wpf.Views
{
    /// <summary>
    /// Interaction logic for LocalView.xaml
    /// </summary>
    public partial class LocalView : UserControl
    {
        private LocalViewModel _model = null;

        public LocalView()
        {
            InitializeComponent();
        }

        private async void DataGrid_LoadedAsync(object sender, RoutedEventArgs e)
        {
            _model = (LocalViewModel)DataContext;
            ITestRequestServiceClient service = TestRequesteServiceClientFactory.CreateService();
            List<TestResponse> list = await service.LoadLocalTestResponseAsync();
            _model.MaxRequests = service.MaxRequests;

            UpdateList(list);
        }

        private void UpdateList(List<TestResponse> list)
        {
            _model.SetTestResponseList(list);
            _model.SetTitleMessage();

            TheGrid.ItemsSource = _model.TestResponseModelList;
        }
    }
}