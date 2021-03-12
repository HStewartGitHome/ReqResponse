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

        private async void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (LocalViewModel)DataContext;
            ITestModelRequestServiceClient service = TestModelRequestServiceClientFactory.CreateService();
            var testModel = await service.LoadLocalTestResponseAsync();
            _model.MaxRequests = testModel.MaxRequests;


            UpdateList(testModel.Responses);
        }

        private void UpdateList(List<TestResponse> list)
        {
            _model.SetTestResponseList(list);
            _model.SetTitleMessage();

            TheGrid.ItemsSource = _model.TestResponseModelList;
        }
    }
}