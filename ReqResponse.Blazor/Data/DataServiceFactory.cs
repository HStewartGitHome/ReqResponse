using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReqResponse.Blazor.Data.Dapper;
using ReqResponse.Blazor.Data.Sim;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Data
{
    public class DataServiceFactory : IDataServiceFactory
    {
        private IConfiguration _configuration = null;
        private IRequestDataService _simRequestDataService = null;
        private IRequestDataService _sqlRequestDataService = null;
        private IResponseDataService _simResponseDataService = null;
        private IResponseDataService _sqlResponseDataService = null;
        private IResponseSummaryDataService _simResponseSummaryDataService = null;
        private IResponseSummaryDataService _sqlResponseSummaryDataService = null;
        private ILogger<DataServiceFactory> _logger = null;

        #region Constructor

        public DataServiceFactory(IConfiguration configuration,
                                ILogger<DataServiceFactory> logger,
                                RequestSimDataService simRequestDataService,
                                RequestSqlDataService sqlRequestDataService,
                                ResponseSimDataService simResponseDataService,
                                ResponseSqlDataService sqlResponseDataService,
                                ResponseSummarySimDataService simResponseSummaryDataService,
                                ResponseSummarySqlDataService sqlResponseSummaryDataService)
        {
            _configuration = configuration;
            _logger = logger;
            _simRequestDataService = simRequestDataService;
            _sqlRequestDataService = sqlRequestDataService;
            _simResponseDataService = simResponseDataService;
            _sqlResponseDataService = sqlResponseDataService;
            _simResponseSummaryDataService = simResponseSummaryDataService;
            _sqlResponseSummaryDataService = sqlResponseSummaryDataService;
        }

        #endregion Constructor

        #region GetISimRequestDataService

        public async Task<IRequestDataService> GetISimRequestDataService()
        {
            await Task.Delay(0);
            return _simRequestDataService;
        }

        #endregion GetISimRequestDataService

        #region GetISqlRequestDataService

        public async Task<IRequestDataService> GetISqlRequestDataService()
        {
            await Task.Delay(0);
            return _sqlRequestDataService;
        }

        #endregion GetISqlRequestDataService

        #region GetISimResponseDataService

        public async Task<IResponseDataService> GetISimResponseDataService()
        {
            await Task.Delay(0);
            return _simResponseDataService;
        }

        #endregion GetISimResponseDataService

        #region GetISqlResponseDataService

        public async Task<IResponseDataService> GetISqlResponseDataService()
        {
            await Task.Delay(0);
            return _sqlResponseDataService;
        }

        #endregion GetISqlResponseDataService

        #region GetISimResponseSummaryDataService

        public async Task<IResponseSummaryDataService> GetISimResponseSummaryDataService()
        {
            await Task.Delay(0);
            return _simResponseSummaryDataService;
        }

        #endregion GetISimResponseSummaryDataService

        #region GetISqlResponseSummaryDataService

        public async Task<IResponseSummaryDataService> GetISqlResponseSummaryDataService()
        {
            await Task.Delay(0);
            return _sqlResponseSummaryDataService;
        }

        #endregion GetISqlResponseSummaryDataService
    }
}