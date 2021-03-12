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
            PrivateOptions = ConfigFactory.GetOptions();
        }

    

        public override string ExecuteXMLRequest(string xmlRequest)
        {
            IsActive = true;
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

            IsActive = false;
            return xml;
           
        }

       

        public override async Task<bool> Connnect(string hostName,
                                                  int port)
        {
            PrivateOptions.HostName = hostName;
            PrivateOptions.Port = port;
            string host = PrivateOptions.HostName;
           

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


        public override bool Reset()
        {
            if (Client.IsConnected())
                Client.Disconnect();
            return true;
        }

        public override async Task StopService()
        {
            IsStopping = true;
            while (IsActive == true)
            {
                await Task.Delay(200);
            }
            Reset();
        }
    }
}