using ReqResponse.Models;
using ReqResponse.Services.Methods;
using ReqResponse.Services.Network;
using ReqResponse.Support;
using System;
using System.Threading.Tasks;

namespace ReqResponse.Services
{
    public class ConnectService : CommonService
    {
        private Options PrivateOptions { get; set; }

        public ConnectService()
        {
            LastResult = Result_Options.Unknown;
            IsConnectedService = true;
            PrivateOptions = Factory.GetOptions();
        }

    

        public override string ExecuteXMLRequest(string xmlRequest)
        {
            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"ExecuteXMLRequest {DateTime.Now} Start....");


            Result_Options result = DeserializeRequest(xmlRequest, out Request request);

            string xml;
            if (request == null)
            {
                if (result == Result_Options.Unknown)
                    result = Result_Options.NullRequest;

                xml = CreateNullResponse(result);
            }
            else if (Client.SendRequest(xmlRequest, PrivateOptions.HostName, PrivateOptions.Port) == true)
                xml = Client.XmlResult;
            else
                xml = CreateNullResponse(Result_Options.FailedConnection);

            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"ExecuteXMLRequest {DateTime.Now} Finish....");

            return xml;
        }

       

        public override async Task<bool> Connnect()
        {
            string host = PrivateOptions.HostName;
            int port = PrivateOptions.Port;

            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} connecting to Host: {host} on Port: {port}");

            bool result = Client.Connect(host,port);
            await Task.Delay(0);
            return result;
        }

        public override async Task<bool> Disconnnect()
        {
            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} disconnecting");
            bool result = Client.Disconnect();
            await Task.Delay(0);
            return result; 
        }
    }
}