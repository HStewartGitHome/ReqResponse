﻿using ReqResponse.DataLayer.Models;
using ReqResponse.Middleware.Services.Client;
using ReqResponse.Middleware.Services.Client.Factories;
using ReqResponse.Wpf.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ReqResponse.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SummaryView.xaml
    /// </summary>
    public partial class SummaryView : UserControl
    {
        private SummaryViewModel _model = null;

        public SummaryView()
        {
            InitializeComponent();
        }

        private async void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {

            _model = (SummaryViewModel)DataContext;
            ITestModelRequestServiceClient service = TestModelRequestServiceClientFactory.CreateService();
            var testModel = await service.LoadResponseSummaryModelsAsync();

            UpdateList(testModel.Summaries);
        }


        private void UpdateList(List<ResponseSummaryModel> list)
        {
            _model.SetResponseSummaryModelList(list);
            _model.SetTitleMessage();

            TheGrid.ItemsSource = _model.ResponseSummaryModelList;
        }
    }
}