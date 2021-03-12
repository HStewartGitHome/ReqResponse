
using Microsoft.Extensions.Configuration;
using ReqResponse.DataLayer.Data;
using ReqResponse.DataLayer.Models;
using ReqResponse.Models;
using ReqResponse.Services;
using ReqResponse.Services.Email;
using ReqResponse.Services.XmlAPI;
using ReqResponse.Support;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//
// This class needs to be reorganized and move into separate classes
//

namespace ReqResponse.Middleware.Services
{
    public class RequestService : IRequestService
    {
        #region Private Variables

        private readonly IConfiguration _configuration = null;
        private readonly IDataServiceFactory _dataFactory = null;
        private readonly IServiceFactory _serviceFactory = null;
        private readonly ServerConfiguration _serverConfiguration = null;
        private readonly Options _options = null;

        #endregion Private Variables

        #region Public Variables

        public string ErrorString { get; set; }
        public int MaxRequests { get; set; }
        public int TakenRequests { get; set; }
        public int ResponseSetId { get; set; }
        public bool FirstRequest { get; set; }
        public bool RemoteRequest { get; set; }
        public List<TestRequest> Requests { get; set; } = null;
        public List<TestRequest> ActiveRequests { get; set; } = null;
        public IRequestDataService SimRequestDataService { get; set; } = null;
        public IRequestDataService SqlRequestDataService { get; set; } = null;
        public IResponseDataService SqlResponseDataService { get; set; } = null;
        public IResponseDataService SimResponseDataService { get; set; } = null;
        public IResponseSummaryDataService SqlResponseSummaryDataService { get; set; } = null;
        public IResponseSummaryDataService SimResponseSummaryDataService { get; set; } = null;

  

        #endregion Public Variables

        #region Constructor

        public RequestService(IConfiguration configuration,
                              ServerConfiguration serverConfiguration,
                              IDataServiceFactory dataFactory,
                              IServiceFactory serviceFactory)
        {
            _configuration = configuration;
            _serverConfiguration = serverConfiguration;
            _dataFactory = dataFactory;
            _serviceFactory = serviceFactory;
            _options = new Options();
            _options.SetServer(_serverConfiguration, true);
            MaxRequests = 0;
            TakenRequests = 0;
            ErrorString = "";
        }

        #endregion Constructor


        #region Public Reset

        public void Reset(bool remote)
        {
            IXmlService service;
            if (remote)
                service = _serviceFactory.GetConnectedService();
            else
                service = _serviceFactory.GetLocalService();
            service.Reset();
        }

        #endregion Public Reset

        public async Task StopService()
        {
            IXmlService  service = _serviceFactory.GetConnectedService();
            await service.StopService();
        }

        #region Public ProcessRequest (main)

        public async Task<List<TestResponse>> ProcessRequest(bool firstRequest,
                                                             Request_Option reqOption,
                                                             int requestLimit)
        {
            List<TestResponse> responses = new List<TestResponse>();

            try
            {
                List<TestRequest> takenRequests = new List<TestRequest>();
                int Id = await GetNextResponseID();
                Request_Option processReqOption = reqOption;

                await InitializeServices();

                int requestCount = 0;
                if (Requests != null)
                    requestCount = Requests.Count;
                int activeRequestCount = 0;
                if (ActiveRequests != null)
                    activeRequestCount = ActiveRequests.Count;

                //if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                    Console.WriteLine($"Client {DateTime.Now} firstRequest: {firstRequest} RequestCount: { requestCount} ActiveRequestCount: {activeRequestCount}");


                FirstRequest = firstRequest;
                if ((firstRequest && (Requests != null)))
                {
                    Requests = null;
                }

                if (Requests == null)
                    Requests = await InitializeRequests();

                if (firstRequest)
                {
                    ActiveRequests = Requests;
                    MaxRequests = Requests.Count;
                    TakenRequests = 0;
                    ResponseSetId = await GetResponseSetID();
                }

                DateTime created = DateTime.Now;
                if (reqOption == Request_Option.StayConnected)
                {

                    AttemptPrimarySwitchBack();

               

                    created = DateTime.Now;
                    processReqOption = await ProcessConnect(Request_Option.StayConnected,
                                                            Request_Option.Connected);
               
                    
                }

                requestLimit = GetRequestLimit(processReqOption);

                int limit = requestLimit;
                if (ActiveRequests.Count < limit)
                    limit = ActiveRequests.Count;
                for (int index = 0; index < limit; index++)
                {
                    if (processReqOption == Request_Option.Connected)
                    {
                        if ( index == 0 )
                            AttemptPrimarySwitchBack();

                        if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine($"Client {DateTime.Now} doing connect for Connected");

                        created = DateTime.Now;
                        processReqOption = await ProcessConnect(Request_Option.Connected,
                                                                Request_Option.Connected);
                      
                    }
                    else if (index > 0)
                        created = DateTime.Now;

                    TestRequest request = ActiveRequests[index];
                    TestResponse response = await ProcessRequest(request, created, processReqOption);
                    if (response != null)
                    {
                        response.Id = Id++;
                        response.ResponseSetId = ResponseSetId;
                        response.RequestOption = reqOption;

                        responses.Add(response);

                        if (RemoteRequest)
                        {
                            if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                                Console.WriteLine($"Client {DateTime.Now} Adding request {request.Id} to count of {takenRequests.Count}");
                            takenRequests.Add(request);
                        }
                    }
                    else
                    {
                        if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine($"Client {DateTime.Now} terminating loop");
                        index = limit;
                    }

                    if (processReqOption == Request_Option.Connected)
                    {
                        if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine($"Client {DateTime.Now} doing disconnect for Connected");

                        await _serviceFactory.GetConnectedService().Disconnnect();
                    }
                }

                if (reqOption == Request_Option.StayConnected)
                {
                    if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                        Console.WriteLine($"Client {DateTime.Now} doing disconnect for StayConnected");

                    await _serviceFactory.GetConnectedService().Disconnnect();
                }

                foreach (TestRequest request in takenRequests)
                    ActiveRequests.Remove(request);

                if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                    Console.WriteLine($"Client {DateTime.Now} Adding count of {takenRequests.Count} to count of {TakenRequests}");

                TakenRequests += takenRequests.Count;

                if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                    Console.WriteLine($"Client {DateTime.Now} reqOption is {reqOption} befor Calling Request Refresh");

                
                if (responses.Count > 0)
                    await SaveResponse(responses);
            }
            catch( Exception e )
            {
                Console.WriteLine($"Exception processing requests {e}");
            }
            return responses;
        }

        #endregion Public ProcessRequest (main)

        private void AttemptPrimarySwitchBack()
        {
            if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} Checking if can PrimarySwitchBack: {_serverConfiguration.PrimarySwitchBack} OpPrimary: {_serverConfiguration.OnPrimary}");

            if ((_serverConfiguration.PrimarySwitchBack == true) && (_serverConfiguration.OnPrimary == false))
            {

                _serverConfiguration.OnPrimary = true;
                _options.SetServer(_serverConfiguration, true);

                if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                    Console.WriteLine($"Client {DateTime.Now} attempting to switch back to primay using host {_options.HostName} on Port {_options.Port}");

            }
        }

        private async Task<Request_Option> ProcessConnect( Request_Option reqOption,
                                                           Request_Option retOption)
        {
            Request_Option result = reqOption;

            if (await _serviceFactory.GetConnectedService().Connnect(_options.HostName, _options.Port) == false)
            {
                // if we can't connect than switch to connected mode


                if (_serverConfiguration.AllowBackup == true)
                {
                    if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                        Console.WriteLine($"Client {DateTime.Now} doing connect for StayConnected on backup");

                    _serverConfiguration.OnPrimary = false;
                    _options.SetServer(_serverConfiguration, false);

                    if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                        Console.WriteLine($"Client {DateTime.Now} using backup on host {_options.HostName} on Port {_options.Port}");

                    if (await _serviceFactory.GetConnectedService().Connnect(_options.HostName, _options.Port) == false)
                    {

                        if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine($"Client {DateTime.Now} failed for StayConnected  with backup switching to Connected");

                        result = Request_Option.Connected;
                    }

                }
                else
                {
                    if (_options.DebugOption == Debug_Option.NetworkClientDataConsole)
                        Console.WriteLine($"Client {DateTime.Now} failed for StayConnected with primary switching to Connected");
                    retOption = Request_Option.Connected;
                }
            }

            return result;
        }

        #region Private ProcessRequest

        private async Task<TestResponse> ProcessRequest(TestRequest request,
                                                        DateTime created,
                                                        Request_Option reqOption)
        {
            IXmlService service;
            TestResponse response = new TestResponse
            {
                Request = request,
                Success = false,
                RequestId = request.Id
            };

            if (reqOption == Request_Option.Connected)
                service = _serviceFactory.GetConnectedService();
            else if (reqOption == Request_Option.StayConnected)
                service = _serviceFactory.GetConnectedService();
            else
                service = _serviceFactory.GetLocalService();

            string xmlResponse = await service.ExecuteRequest(request.InputXml);
            Response resp = XmlHelper.DeserializeObject<Response>(xmlResponse);
            response.ActualResult = resp.Result;
            response.ActualValue = resp.ResultValue;
            response.Created = created;
            response.MakeTimeExecuted();

            if (response.Request.ExpectedResult == response.ActualResult)
            {
                response.Success = true;
                if (response.ActualResult == Result_Options.Ok)
                {
                    if (response.Request.ExpectedValue.CompareTo(response.ActualValue) != 0)
                    {
                        response.Success = false;
                        response.ActualResult = Result_Options.ValueMismatch;
                    }
                }
            }

            return response;
        }

        #endregion Private ProcessRequest

        #region GetRequestLimit

        public int GetRequestLimit(Request_Option reqOption)
        {
            int requestLimit;
            if (reqOption == Request_Option.Connected)
            {
                RemoteRequest = true;
                requestLimit = _options.NetLimit;
            }
            else if (reqOption == Request_Option.StayConnected)
            {
                RemoteRequest = true;
                requestLimit = _options.StayNetLimit;
            }
            else
                requestLimit = 9999;

            return requestLimit;
        }

        #endregion GetRequestLimit

        #region Public GetReponseSummaryModelBySetId

        public async Task<ResponseSummaryModel> GetReponseSummaryModelBySetId(int setId)
        {
            List<ResponseSummaryModel> models = null;

            await InitializeServices();
            ResponseSummaryModel model = await SimResponseSummaryDataService.GetByResponseSetId(setId);
            if (model == null)
                models = await GetAllSummaryModels();
            if (models != null)
                model = await SimResponseSummaryDataService.GetByResponseSetId(setId);

            return model;
        }

        #endregion Public GetReponseSummaryModelBySetId

        #region Public GetFailedResponsesForSet

        public async Task<List<TestResponse>> GetFailedResponsesForSet(int setId)
        {
            List<TestResponse> responses = null;

            if (Requests == null)
                Requests = await InitializeRequests();

            await LoadIfNeedResponses();
            await LoadIfNeedResponseSummary();

            List<ResponseDataModel> models = await SimResponseDataService.GetAll();
            foreach (ResponseDataModel model in models)
            {
                if ((model.ResponseSetId == setId) && (model.Success == false))
                {
                    TestResponse response = await MakeTestResponseFromModel(model);
                    if (responses == null)
                        responses = new List<TestResponse>();
                    responses.Add(response);
                }
            }

            return responses;
        }

        #endregion Public GetFailedResponsesForSet

        #region Public GetAllSummaryModels

        public async Task<List<ResponseSummaryModel>> GetAllSummaryModels()
        {
            List<ResponseSummaryModel> models;

            await InitializeServices();
            models = await SqlResponseSummaryDataService.GetAll();
            if (models != null)
                await SimResponseSummaryDataService.CreateAll(models);
            return models;
        }

        #endregion Public GetAllSummaryModels

        #region Public IsNeedRequest

        public bool IsNeedRequest()
        {
            if (MaxRequests == 0)
                return false;
            if (TakenRequests < MaxRequests)
                return true;
            else
                return false;
        }

        #endregion Public IsNeedRequest

        #region Public GetTestErrorReport

        public async Task<TestErrorReport> GetTestErrorReportAsync()
        {
            TestErrorReport report = new TestErrorReport();
            await LoadIfNeedResponseSummary();

            report.CurrentLastErrorDateTime = GetLastEmailDateTime();
            List<ResponseSummaryModel> models = await SimResponseSummaryDataService.GetAll();
            DateTime LastTime = report.CurrentLastErrorDateTime;
            foreach (ResponseSummaryModel model in models)
            {
                if ((model.FailedCount > 0) && (model.Created > report.CurrentLastErrorDateTime))
                {
                    report.ErrorCount += model.FailedCount;
                    report.ErrorSet++;
                    if (model.Created > LastTime)
                        LastTime = model.Created;
                }
            }
            report.Message = "(" + report.ErrorCount.ToString() + ") errors occurred";
            report.LastErrorDateTime = LastTime;
            return report;
        }

        #endregion Public GetTestErrorReport

        #region Public EmailTestErrorReport

        public async Task EmailTestErrorReportAsync()
        {
            TestErrorReport report = new TestErrorReport();
            List<ResponseSummaryModel> errorModels = new List<ResponseSummaryModel>();
            await LoadIfNeedResponseSummary();
            await LoadIfNeedResponses();

            report.CurrentLastErrorDateTime = GetLastEmailDateTime();
            List<ResponseSummaryModel> models = await SimResponseSummaryDataService.GetAll();
            DateTime LastTime = report.CurrentLastErrorDateTime;
            foreach (ResponseSummaryModel model in models)
            {
                if ((model.FailedCount > 0) && (model.Created > report.CurrentLastErrorDateTime))
                {
                    report.ErrorCount += model.FailedCount;
                    report.ErrorSet++;
                    if (model.Created > LastTime)
                        LastTime = model.Created;
                    errorModels.Add(model);
                }
            }
            report.Message = "(" + report.ErrorCount.ToString() + ") errors occurred";
            report.LastErrorDateTime = LastTime;

            if (report.ErrorCount > 0)
                await CreateAndEmailReport(report, errorModels);
            SetEmailDateTime();
        }

        #endregion Public EmailTestErrorReport

        #region private CreateAndEmailReport

        private async Task CreateAndEmailReport(TestErrorReport errorReport,
                                                 List<ResponseSummaryModel> errorModels)
        {
            List<string> strs = new List<string>();

            string str = "Error Report for " + errorReport.Created.ToString() + " Last Report Time = " + errorReport.CurrentLastErrorDateTime.ToString();
            strs.Add(str);
            str = "   Errors Occured = " + errorReport.ErrorCount.ToString();
            strs.Add(str);
            str = "   Error Sets     = " + errorReport.ErrorSet.ToString();
            strs.Add(str);
            strs.Add("");

            foreach (ResponseSummaryModel summary in errorModels)
            {
                await AddSummaryToEmail(strs, summary);
            }

            IEmailService service = _serviceFactory.GetEmailService();
            await service.EmailErrorReportStrings(strs);
        }

        #endregion private CreateAndEmailReport

        #region AddSummaryToEmail

        private async Task AddSummaryToEmail(List<string> strs,
                                 ResponseSummaryModel summary)
        {
            string str;
            str = "   Summary SetId=" + summary.ResponseSetId.ToString() + " Created:" + summary.Created.ToString() + " Failed Errors:" + summary.FailedCount.ToString();
            strs.Add(str);
            strs.Add("");

            List<TestResponse> responses = await GetFailedResponsesForSet(summary.ResponseSetId);
            foreach (TestResponse response in responses)
            {
                str = "      ResponseId:    " + @response.Id.ToString();
                strs.Add(str);
                str = "      Created:       " + @response.Created.ToString();
                strs.Add(str);
                str = "      Request.Id:    " + @response.Request.Id.ToString();
                strs.Add(str);
                str = "      Method:        " + @response.Request.Method.ToString();
                strs.Add(str);
                str = "      Value1:        " + @response.Request.Value1.ToString();
                strs.Add(str);
                str = "      Value2:        " + @response.Request.Value2.ToString();
                strs.Add(str);
                str = "      ActualValue:   " + @response.ActualValue.ToString();
                strs.Add(str);
                str = "      ExpectedResult:" + @response.Request.ExpectedResult.ToString();
                strs.Add(str);
                str = "      ActualResult:   " + @response.ActualResult.ToString();
                strs.Add(str);
            }
            strs.Add("");
        }

        #endregion AddSummaryToEmail

        #region Private routines

        public async Task InitializeServices()
        {
            if (SqlRequestDataService == null)
                SqlRequestDataService = await _dataFactory.GetISqlRequestDataService();
            if (SimRequestDataService == null)
                SimRequestDataService = await _dataFactory.GetISimRequestDataService();
            if (SqlResponseDataService == null)
                SqlResponseDataService = await _dataFactory.GetISqlResponseDataService();
            if (SimResponseDataService == null)
                SimResponseDataService = await _dataFactory.GetISimResponseDataService();
            if (SqlResponseSummaryDataService == null)
                SqlResponseSummaryDataService = await _dataFactory.GetISqlResponseSummaryDataService();
            if (SimResponseSummaryDataService == null)
                SimResponseSummaryDataService = await _dataFactory.GetISimResponseSummaryDataService();
        }

        private async Task<int> GetNextResponseID()
        {
            int Id = 1;

            await InitializeServices();
            int count = await SqlResponseDataService.GetCount();
            if (count > 0)
                Id = await SqlResponseDataService.GetNextResponseID();

            return Id;
        }

        private async Task<int> GetResponseSetID()
        {
            int Id = 1;

            await InitializeServices();
            int count = await SqlResponseDataService.GetCount();
            if (count > 0)
                Id = count + 1;

            return Id;
        }

        public async Task SaveResponse(List<TestResponse> responses)
        {
            var models = MakeResponseDataModels(responses);
            await InitializeServices();
            await SqlResponseDataService.AddAll(models);
            await UpdateSummary(responses);
        }

        public List<ResponseDataModel> MakeResponseDataModels(List<TestResponse> responses)
        {
            List<ResponseDataModel> models = new List<ResponseDataModel>();

            foreach (TestResponse response in responses)
            {
                ResponseDataModel model = new ResponseDataModel(response);
                if (model != null)
                {
                    model.ResponseSetId = ResponseSetId;
                    models.Add(model);
                }
            }
            return models;
        }

        private async Task UpdateSummary(List<TestResponse> responses)
        {
            ResponseSummaryModel summary;
            if (FirstRequest)
                summary = new ResponseSummaryModel();
            else
                summary = await SqlResponseSummaryDataService.GetByResponseSetId(ResponseSetId);
            Update(summary, responses);

            if (FirstRequest)
                await SqlResponseSummaryDataService.Create(summary);
            else
                await SqlResponseSummaryDataService.Update(summary);
        }

        private void Update(ResponseSummaryModel summary,
                                List<TestResponse> responses)
        {
            summary.ResponseSetId = ResponseSetId;
            foreach (TestResponse response in responses)
            {
                if (response.Success == true)
                    summary.SuccessfullCount++;
                else
                    summary.FailedCount++;

                if (response.ActualResult == Result_Options.Ok)
                    summary.OkCount++;
                else
                    summary.ErrorCount++;

                summary.TimeExecuted += response.TimeExecuted;
                Console.WriteLine($"RequestOption: {response.RequestOption} Id {response.RequestId} Response Time: {response.TimeExecuted}  Total Time: {summary.TimeExecuted}  OkCount:  {summary.OkCount} ErrorCount: {summary.ErrorCount}");
                summary.RequestOption = response.RequestOption;
            }
        }

        private async Task<TestResponse> MakeTestResponseFromModel(ResponseDataModel model)
        {
            List<TestRequest> requests = await SimRequestDataService.GetAll();
            TestRequest request = null;
            TestResponse response = new TestResponse
            {
                Id = model.Id,
                Created = model.Created,
                ResponseSetId = model.ResponseSetId,
                Success = model.Success,
                ActualResult = model.ActualResult,
                ActualValue = model.ActualValue,
                TimeExecuted = model.TimeExecuted,
                RequestOption = model.RequestOption
            };

            foreach (TestRequest req in requests)
            {
                if (req.Id == model.RequestId)
                    request = req;
            }
            if (request == null)
            {
                response.Request = new TestRequest
                {
                    Method = "NULL",
                    Value1 = "NULL",
                    Value2 = "NULL"
                };
                response.RequestId = model.RequestId;
            }
            else
            {
                response.Request = request;
                response.RequestId = request.Id;
            }

            return response;
        }

        private async Task LoadIfNeedResponses()
        {
            List<ResponseDataModel> models;

            await InitializeServices();
            int simCount = await SimResponseDataService.GetCount();
            int sqlCount = await SqlResponseDataService.GetCount();
            if ((sqlCount != simCount) || (simCount == 0))
            {
                models = await SqlResponseDataService.GetAll();
                await SimResponseDataService.CreateAll(models);
            }
        }

        private async Task LoadIfNeedResponseSummary()
        {
            List<ResponseSummaryModel> models;

            await InitializeServices();
            int simCount = await SimResponseSummaryDataService.GetCount();
            int sqlCount = await SqlResponseSummaryDataService.GetCount();
            if ((sqlCount != simCount) || (simCount == 0))
            {
                models = await SqlResponseSummaryDataService.GetAll();
                await SimResponseSummaryDataService.CreateAll(models);
            }
        }

        private async Task<List<TestRequest>> InitializeRequests()
        {
            List<TestRequest> requests = null;
            bool updateSql = false;

            await InitializeServices();
            if (await SimRequestDataService.GetCount() > 0)
                requests = await SimRequestDataService.GetAll();
            else if (await SqlRequestDataService.GetCount() > 0)
            {
                requests = await SqlRequestDataService.GetAll();
                await SimRequestDataService.CreateAll(requests);
            }

            if (requests == null)
            {
                requests = await SimRequestDataService.GetTestRequestsAsync();
                if ((requests != null) && (requests.Count > 0))
                    updateSql = true;
            }
            else if (await SqlRequestDataService.GetCount() == 0)
                updateSql = true;

            if ((updateSql && (requests != null)))
                await SqlRequestDataService.CreateAll(requests);

            return requests;
        }

        private DateTime GetLastEmailDateTime()
        {
            string str = _configuration["LastEmailDateTime"];
            if (DateTime.TryParse(str, out DateTime time) == false)
                time = DateTime.Now;

            return time;
        }

        private void SetEmailDateTime()
        {
            string str = DateTime.Now.ToString();
            _configuration["LastEmailDateTime"] = str;
        }

        #endregion Private routines
    }
}