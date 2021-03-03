﻿using ReqResponse.DataLayer.Models;
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
    /// Interaction logic for ConnectedView.xaml
    /// </summary>
    public partial class ConnectedView : UserControl
    {
        private ITestRequestServiceClient _service = null;
        private List<TestResponse> _list = null;
        private ConnectedViewModel _model = null;

        public ConnectedView()
        {
            InitializeComponent();
        }

        private async void DataGrid_LoadedAsync(object sender, RoutedEventArgs e)
        {
            _model = (ConnectedViewModel)DataContext;
            if (_service == null)
                _service = (ITestRequestServiceClient)TestRequesteServiceClientFactory.CreateService();
            _service.UpdateRequested += RefreshMe;
            List<TestResponse> list = await _service.LoadConnectedTestResponseAsync(true);
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
            List<TestResponse> list = await _service.LoadConnectedTestResponseAsync(false);

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

        private async void OnUnloadedAsync(object sender, RoutedEventArgs e)
        {
            if (_service != null)
                await _service.StopService();
        }
    }
}