using ReqResponse.Models;
using ReqResponse.Support;
using System;

namespace ReqResponse.Services
{
    public class Service : CommonService
    {
        public Service()
        {
            LastResult = Result_Options.Unknown;
            IsConnectedService = false;
        }

        public override string ExecuteXMLRequest(string xmlRequest)
        {
            IsActive = true;
            string xmlResponse = "";
            ExceptionHappen = false;
            // first deserialize xmlRequest
            Result_Options result = DeserializeRequest(xmlRequest, out Request request);


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
                    ExceptionHappen = true;
                }

                if ((response == null) || ExceptionHappen)
                {
                    response = new Response();

                    if (ExceptionHappen)
                        response.Result = result;
                    else
                        response.Result = Result_Options.NullRequest;
                }

                if (response != null)
                {
                    result = SerializeResponse(response, out xmlResponse);
                    if ( ExceptionHappen == false )
                        result = response.Result;
                }
            }

            if (result == Result_Options.Unknown)
            {
                result = Result_Options.NullRequest;
                xmlResponse = CreateNullResponse(result) ;
            }
            LastResult = result;
            IsActive = false;
            return xmlResponse;
        }
    }
}