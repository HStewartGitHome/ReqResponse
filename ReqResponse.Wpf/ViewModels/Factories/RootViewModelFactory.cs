using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using static ReqResponse.Wpf.Models.Constants;

namespace ReqResponse.Wpf.ViewModels.Factories
{
    public class RootViewModelFactory : IRootViewModelFactory
    {
        private readonly IViewModelFactory<HomeViewModel> _homeFactory;
        private readonly IViewModelFactory<LocalViewModel> _localFactory;
        private readonly IViewModelFactory<RemoteViewModel> _remoteFactory;
        private readonly IViewModelFactory<ConnectedViewModel> _connectedFactory;
        private readonly IViewModelFactory<SummaryViewModel> _summaryFactory;
        private readonly IViewModelFactory<ErrorsViewModel> _errorsFactory;
        private readonly ILogger<RootViewModelFactory> _logger;
        private readonly IConfiguration _config;

        public RootViewModelFactory(IViewModelFactory<HomeViewModel> homeFactory,
                                    IViewModelFactory<LocalViewModel> localFactory,
                                    IViewModelFactory<RemoteViewModel> remoteFactory,
                                    IViewModelFactory<ConnectedViewModel> connectedFactory,
                                    IViewModelFactory<SummaryViewModel> summaryFactory,
                                    IViewModelFactory<ErrorsViewModel> errorsFactory,
                                    ILogger<RootViewModelFactory> logger,
                                    IConfiguration config)

        {
            _homeFactory = homeFactory;
            _localFactory = localFactory;
            _remoteFactory = remoteFactory;
            _connectedFactory = connectedFactory;
            _summaryFactory = summaryFactory;
            _errorsFactory = errorsFactory;
            _logger = logger;
            _config = config;
        }

        public BaseViewModel CreateViewModel(ViewType viewType)
        {
            string str1 = (string)_config.GetValue(typeof(string), "ConnectionStrings:SQLDB");
            string str2 = (string)_config.GetValue(typeof(string), "UseSIM");

            _logger.LogInformation($"Creating view of ViewType: {viewType}  with SQLDB = {str1} and UseSIM={str2}");
            switch (viewType)
            {
                case ViewType.Home:
                    return _homeFactory.CreateViewModel();

                case ViewType.Local:
                    return _localFactory.CreateViewModel();

                case ViewType.Remote:
                    return _remoteFactory.CreateViewModel();

                case ViewType.Connected:
                    return _connectedFactory.CreateViewModel();

                case ViewType.Summary:
                    return _summaryFactory.CreateViewModel();

                case ViewType.Errors:
                    return _errorsFactory.CreateViewModel();

                default:
                    throw new ArgumentException($"The ViewType {viewType} does not have a ViewModel");
            }
        }
    }
}