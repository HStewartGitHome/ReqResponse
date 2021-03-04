using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using ReqResponse.Middleware.Services.Client.Factories;
using ReqResponse.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ReqResponse.Wpf.Views
{
    /// <summary>
    /// Interaction logic for RemoteView.xaml
    /// </summary>
    public partial class RemoteView : UserControl
    {
        private ITestRequestServiceClient _service = null;
        private List<TestResponse> _list = null;
        private RemoteViewModel _model = null;

        public RemoteView()
        {
            InitializeComponent();
        }

        private async void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (RemoteViewModel)DataContext;

            if (_service == null)
                _service = (ITestRequestServiceClient)TestRequesteServiceClientFactory.CreateService();
            _service.UpdateRequested += RefreshMe;
            List<TestResponse> list = await _service.LoadRemoteTestResponseAsync(true);
            _model.MaxRequests = _service.MaxRequests;

            _list = new List<TestResponse>();
            foreach (TestResponse response in list)
            {
                _list.Add(response);
            }

            UpdateList(list);
        }

        private void RefreshMe()
        {
            if (_service.IsNeedRequest() == true)
            {
                _ = Task.Run(async () =>
                    {
                        try
                        {
                            await AddRequestsAsync();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    });
            }
            else
                _service.Reset(true);
        }

        private async Task AddRequestsAsync()
        {
            List<TestResponse> list = await _service.LoadRemoteTestResponseAsync(false);

            foreach (TestResponse response in list)
            {
                _list.Add(response);
            }

            list = new List<TestResponse>();
            foreach (TestResponse response in _list)
            {
                list.Add(response);
            }

            this.Dispatcher.Invoke(() =>
            {
                UpdateList(list);
            });
        }

        private void UpdateList(List<TestResponse> list)
        {
            _model.SetTestResponseList(list);
            _model.SetTitleMessage();

            TheGrid.ItemsSource = _model.TestResponseModelList;
        }

        private async void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_service != null)
                await _service.StopService();
        }
    }
}