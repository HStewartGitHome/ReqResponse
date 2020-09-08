﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders.Embedded;
using ReqResponse.Blazor.Data;
using ReqResponse.Blazor.Data.Dapper;
using ReqResponse.Blazor.Models;
using ReqResponse.Blazor.Services.Email;
using ReqResponse.Blazor.Services.XmlAPI;
using ReqResponse.Models;
using ReqResponse.Services.Methods;
using ReqResponse.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ReqResponse.Blazor.Services
{
    public class RequestService : IRequestService
    {
        #region Private Variables
        private IConfiguration _configuration = null;
        private IDataServiceFactory _dataFactory = null;
        private IServiceFactory _serviceFactory = null;
        private Options _options = null;
        #endregion

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
        public event Action RefreshRequested;
        #endregion

        #region Constructor
        public RequestService(IConfiguration configuration,
                              IDataServiceFactory dataFactory,
                              IServiceFactory serviceFactory)
        {
            _configuration = configuration;
            _dataFactory = dataFactory;
            _serviceFactory = serviceFactory;
            _options = new Options();
            MaxRequests = 0;
            TakenRequests = 0;
            ErrorString = "";
        }
        #endregion

        #region Public CallRequestRefresh
        public void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
        #endregion

        #region Public ProcessRequest (main)
        public async Task<List<TestResponse>> ProcessRequest(bool firstRequest,
                                                             bool remoteRequest,
                                                             int requestLimit)
        {
            List<TestResponse> responses = new List<TestResponse>();
            List<TestRequest> takenRequests = new List<TestRequest>();
            int Id = await GetNextResponseID();

            await InitializeServices();

            FirstRequest = firstRequest;
            RemoteRequest = remoteRequest;
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

            if (remoteRequest)
                requestLimit = _options.NetLimit;
            else
                requestLimit = 9999;
            int limit = requestLimit;
            if (ActiveRequests.Count < limit)
                limit = ActiveRequests.Count;
            for (int index = 0; index < limit; index++)
            {
                TestRequest request = ActiveRequests[index];
                TestResponse response = await ProcessRequest(request, remoteRequest);
                if (response != null)
                {
                    response.Id = Id++;
                    response.ResponseSetId = ResponseSetId;
                    responses.Add(response);

                    if (remoteRequest)
                        takenRequests.Add(request);
                }
            }

            foreach (TestRequest request in takenRequests)
                ActiveRequests.Remove(request);
            TakenRequests += takenRequests.Count;

            if (remoteRequest)
                CallRequestRefresh();

            if (responses.Count > 0)
                await SaveResponse(responses);
            return responses;
        }
        #endregion

        #region Public GetReponseSummaryModelBySetId
        public async Task<ResponseSummaryModel> GetReponseSummaryModelBySetId(int setId)
        {
            ResponseSummaryModel model = null;
            List<ResponseSummaryModel> models = null;

            await InitializeServices();
            model = await SimResponseSummaryDataService.GetByResponseSetId(setId);
            if (model == null)
                models = await GetAllSummaryModels();
            if (models != null)
                model = await SimResponseSummaryDataService.GetByResponseSetId(setId);

            return model;
        }
        #endregion

        #region Public GetFailedResponsesForSet
        public async Task<List<TestResponse>> GetFailedResponsesForSet(int setId)
        {
            List<TestResponse> responses = null;
            List<ResponseDataModel> models = null;
            await LoadIfNeedResponses();
            await LoadIfNeedResponseSummary();

            models = await SimResponseDataService.GetAll();
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
        #endregion

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
        #endregion

        #region Public Reset
        public async Task Reset(bool remote)
        {
            IXmlService service;
            await Task.Delay(0);
            if (remote)
                service = _serviceFactory.GetConnectedService();
            else
                service = _serviceFactory.GetLocalService();
            service.Reset();
        }
        #endregion

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
        #endregion

        #region Public GetTestErrorReport
        public async Task<TestErrorReport> GetTestErrorReport()
        {
            TestErrorReport report = new TestErrorReport();
            List<ResponseSummaryModel> models = null;
            await LoadIfNeedResponseSummary();

            report.CurrentLastErrorDateTime = GetLastEmailDateTime();
            models = await SimResponseSummaryDataService.GetAll();
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
        #endregion

        #region Public EmailTestErrorReport
        public async Task EmailTestErrorReport()
        {
            TestErrorReport report = new TestErrorReport();
            List<ResponseSummaryModel> models = null;
            List<ResponseSummaryModel> errorModels = new List<ResponseSummaryModel>();
            await LoadIfNeedResponseSummary();
            await LoadIfNeedResponses();

            report.CurrentLastErrorDateTime = GetLastEmailDateTime();
            models = await SimResponseSummaryDataService.GetAll();
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
        #endregion

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

        #region AddSummaryToEmail
        private async Task  AddSummaryToEmail( List<string> strs, 
                                 ResponseSummaryModel summary)
        {
            string str;
            str = "   Summary SetId=" + summary.ResponseSetId.ToString() + " Created:" + summary.Created.ToString() + " Failed Errors:" + summary.FailedCount.ToString();
            strs.Add(str);
            strs.Add("");

            List<TestResponse> responses = await GetFailedResponsesForSet(summary.ResponseSetId);
            foreach(TestResponse response in responses)
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
        #endregion

        #region Private ProcessRequest
        private async Task<TestResponse> ProcessRequest(TestRequest request,
                                                        bool remoteRequest)
        {
            IXmlService service;
            TestResponse response = new TestResponse();
            response.Request = request;
            response.Success = false;
            response.RequestId = request.Id;

            if (remoteRequest)
                service = _serviceFactory.GetConnectedService();
            else
                service = _serviceFactory.GetLocalService();

            string xmlResponse = await service.ExecuteRequest(request.InputXml);
            Response resp = XmlHelper.DeserializeObject<Response>(xmlResponse);
            response.ActualResult = resp.Result;
            response.ActualValue = resp.ResultValue;
            response.Created = DateTime.Now;


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
        #endregion


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

            await InitializeServices();
            await SqlResponseDataService.AddAll(models);

            ResponseSummaryModel summary = new ResponseSummaryModel();
            Update(summary, responses);

            if (FirstRequest)
                await SqlResponseSummaryDataService.Create(summary);
            else
            {
                ResponseSummaryModel model = await SqlResponseSummaryDataService.GetByResponseSetId(summary.ResponseSetId);
                if (model != null)
                {
                    model.SuccessfullCount += summary.SuccessfullCount;
                    model.FailedCount += summary.FailedCount;
                    model.OkCount += summary.OkCount;
                    model.ErrorCount += summary.ErrorCount;
                    await SqlResponseSummaryDataService.Update(model);
                }
                else
                    await SqlResponseSummaryDataService.Create(summary);
            }
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
            }
        }
        private async Task<TestResponse> MakeTestResponseFromModel(ResponseDataModel model)
        {
            List<TestRequest> requests = await SimRequestDataService.GetAll();
            TestRequest request = null;
            TestResponse response = new TestResponse();
            response.Id = model.Id;
            response.Created = model.Created;
            response.ResponseSetId = model.ResponseSetId;
            response.Success = model.Success;
            response.ActualResult = model.ActualResult;
            response.ActualValue = model.ActualValue;

            foreach (TestRequest req in requests)
            {
                if (req.Id == model.RequestId)
                    request = req;
            }
            if (request == null)
            {
                response.Request = null;
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
            DateTime time;
            string str = _configuration.GetValue<string>("LastEmailDateTime");
            if (DateTime.TryParse(str, out time) == false)
                time = DateTime.Now;

            return time;
        }

        private void SetEmailDateTime()
        {
            string str = DateTime.Now.ToString();
            _configuration["LastEmailDateTime"] = str;
        }


        #endregion
    }
}