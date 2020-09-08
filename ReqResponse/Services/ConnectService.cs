using ReqResponse.Models;
using ReqResponse.Services.Methods;
using ReqResponse.Services.Network;
using ReqResponse.Support;
using System;

namespace ReqResponse.Services
{
    public class ConnectService : IService
    {
        public Result_Options LastResult { get; set; }

        public bool IsConnectedService { get; set; }

        public ConnectService()
        {
            LastResult = Result_Options.Unknown;
            IsConnectedService = true;
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
            Request request;
            string xmlResponse = "";

            // first deserialize xmlRequest
            try
            {
                request = (Request)XmlHelper.DeserializeObject<Request>(xmlRequest);
            }
            catch (Exception e)
            {
                result = Result_Options.ExceptionParsingRequest;
                request = null;
            }

            if (request == null)
            {
                if (result == Result_Options.Unknown)
                    result = Result_Options.NullRequest;

                return CreateNullResponse(result);
            }
            else if (Client.SendOldRequest(xmlRequest, "localhost", 11000) == true)
                return Client.XmlResult;
            else
                return CreateNullResponse(Result_Options.FailedConnection);
        }

        public string CreateNullResponse(Result_Options result)
        {
            string xmlResponse = "";
            LastResult = result; ;

            Response response = new Response
            {
                Result = LastResult
            };

            try
            {
                xmlResponse = (string)XmlHelper.SerializeObject<Response>(typeof(Response), response);
            }
            catch (Exception e)
            {
                LastResult = Result_Options.ExceptionParseResponse;
            }

            return xmlResponse;
        }
    }
}