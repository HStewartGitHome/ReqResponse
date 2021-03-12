using ReqResponse.Models;
using ReqResponse.Services.Methods;
using ReqResponse.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqResponse.Services
{
    public class CommonService : IService
    {

        public Result_Options LastResult { get; set; }
        public bool IsConnectedService { get; set; }
        public bool ExceptionHappen { get; set; }

        public bool IsActive { get; set; }
        
        public bool IsStopping { get; set; }

        public virtual async Task StopService()
        {
            IsStopping = true;
            await Task.Delay(0);
        }

        public virtual Task<bool> Connnect(string hostName,
                                           int port)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Disconnnect()
        {
            throw new NotImplementedException();
        }
        public  virtual bool Reset()
        {
            throw new NotImplementedException();
        }
    

        public virtual Response ExecuteRequest(Request request)
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

        public virtual string ExecuteXMLRequest(string xmlRequest)
        {
            throw new NotImplementedException();
        }

        public virtual Result_Options DeserializeRequest( string xmlRequest,
                                                          out Request request)
        {

            Result_Options result = Result_Options.Unknown;
            try
            {
                request = (Request) XmlHelper.DeserializeObject<Request>(xmlRequest);
            }
            catch (Exception)
            {
                result = Result_Options.ExceptionParsingRequest;
                request = null;
                ExceptionHappen = true;
            }

            return result;
        }

        public virtual Result_Options SerializeResponse( Response response,
                                                         out string xmlResponse)
        {

            Result_Options result = Result_Options.Unknown;
            string xml = "";
            try
            {
                
                xml = (string)XmlHelper.SerializeObject<Response>(typeof(Response),  response);   
            }
            catch (Exception)
            {
                result = Result_Options.ExceptionParseResponse;
                ExceptionHappen = true;
            }

            xmlResponse = xml;
            return result;
        }

        public virtual string CreateNullResponse(Result_Options result)
        {
            LastResult = result;

            Response response = new Response
            {
                Result = LastResult
            };


            _ = SerializeResponse(response, out string xmlResponse);

            return xmlResponse;
        }
    }
}
