using ReqResponse.Models;
using ReqResponse.Services.Methods;
using ReqResponse.Support;
using System;

namespace ReqResponse.Services
{
    public class Service : IService
    {
        public Result_Options LastResult { get; set; }
        public bool IsConnectedService { get; set; }

        public Service()
        {
            LastResult = Result_Options.Unknown;
            IsConnectedService = false;
        }

        public Response ExecuteRequest(Request request)
        {
            Response response = new Response();
            BaseMethod method = null;

            LastResult = Result_Options.Unknown;

            if (request.Method.CompareTo("Add") == 0)
                method = new Add();
            else if (request.Method.CompareTo("Subtract") == 0)
                method = new Subtract();
            else if (request.Method.CompareTo("Multiply") == 0)
                method = new Multiply();
            if (request.Method.CompareTo("Divide") == 0)
                method = new Divide();

            if (method != null)
                response = method.ExecuteRequest(request);
            else
                response.Result = Result_Options.InvalidRequestMethod;

            LastResult = response.Result;
            return response;
        }

        public string ExecuteXMLRequest(string xmlRequest)
        {
            Result_Options result = Result_Options.Unknown;
            string xmlResponse = "";
            Request request;
            bool exceptionHappen = false;

            // first deserialize xmlRequest
            try
            {
                request = (Request)XmlHelper.DeserializeObject<Request>(xmlRequest);
            }
            catch (Exception)
            {
                result = Result_Options.ExceptionParsingRequest;
                request = null;
                exceptionHappen = true;
            }

            Response response;
            if (request != null)
            {
                // execute the request  and get response
                try
                {
                    response = ExecuteRequest(request);
                }
                catch (Exception)
                {
                    response = null;
                    result = Result_Options.Exception;
                    exceptionHappen = true;
                }

                if ((response == null) || exceptionHappen)
                {
                    response = new Response();
                    if (exceptionHappen)
                        response.Result = result;
                    else
                        response.Result = Result_Options.NullRequest;
                }

                if (response != null)
                {
                    result = response.Result;
                    try
                    {
                        xmlResponse = (string)XmlHelper.SerializeObject<Response>(typeof(Response), response);
                    }
                    catch (Exception)
                    {
                        result = Result_Options.ExceptionParseResponse;
                        xmlResponse = "";
                    }
                }
            }

            if (result == Result_Options.Unknown)
            {
                result = Result_Options.NullRequest;
                response = new Response
                {
                    Result = result
                };

                try
                {
                    xmlResponse = (string)XmlHelper.SerializeObject<Response>(typeof(Response), response);
                }
                catch (Exception)
                {
                    result = Result_Options.ExceptionParseResponse;
                }
            }
            LastResult = result;
            return xmlResponse;
        }
    }
}