using ReqResponse.Models;
using ReqResponse.Support;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ReqResponse.Services.Network
{
    public static class Client
    {
        public static string XmlResult { get; set; } 
        

        private static TcpClient CurrentClient = null;
        private static NetworkStream CurrentStream = null;
        private static readonly Options PrivateOptions = ConfigFactory.GetOptions();
      

        public static bool SendRequest(string xml,
                                       string hostName,
                                        int port)
        {
            bool result = false;
            var encoding = Encoding.ASCII;
            Byte[] bytes = new Byte[PrivateOptions.BufferSize];
            string data;
            int i;
          
            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} Start....");

            try
            {
                XmlResult = "";
                if (CurrentClient != null)
                {

                    NetworkStream stream = null;
                    if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                        Console.WriteLine($"Client {DateTime.Now} after connect");
                
                    if (CurrentStream == null)
                    {
                        if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine($"Client {DateTime.Now} using CurrentClient stream");

                        stream = CurrentClient.GetStream();
                    }
                    else
                    {
                        if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine($"Client {DateTime.Now} using CurrentStream");
                        stream = CurrentStream;
                    }
                    if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                        Console.WriteLine($"Client {DateTime.Now} after getstream");

                    if (stream == null)
                    {
                        if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine($"Client {DateTime.Now} stream is null");
                    }
                    else
                    { 
                        stream.ReadTimeout = PrivateOptions.ReadTimeout;

                        if (stream.CanWrite)
                        {
                            var bytesToSend = encoding.GetBytes(xml + "\r\n");
                            stream.Write(bytesToSend, 0, bytesToSend.Length);
                        }

                        if (stream.CanRead)
                        {


                            while ((result == false) && (((i = stream.Read(bytes, 0, bytes.Length)) != 0)))
                            {
                                // Translate data bytes to a ASCII string.
                                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                                    Console.WriteLine($"Client {DateTime.Now} Received: {data}");
                                else if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataTrace)
                                    Trace.TraceInformation("Received: {0}", data);
                                XmlResult = data;
                                result = true;
                            }
                        }
                    }

                    if (CurrentStream == null)
                    {
                        if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine($"Client {DateTime.Now} removing stream");

                        RemoveStream(stream);
                    }
                }
                else
                {
                    if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                        Console.WriteLine($"Client {DateTime.Now} CurrentClient is null");
                    result = false;
                }

         
             
            }
            catch (IOException)
            {
                Trace.TraceError("--- Connection lost");
                Console.WriteLine($"Client {DateTime.Now} ---Connection lost");
            }
            catch (SocketException ex)
            {
                Trace.TraceError("--- Can't connect: " + ex.Message);
                Console.WriteLine($"Client {DateTime.Now} ---Cant connect: {ex.Message}");
            }
            catch(Exception ex)
            {
                Trace.TraceError("--- Exception " + ex.Message);
                Console.WriteLine($"Client {DateTime.Now} ---Exception: {ex.Message}");
            }


            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} Finish.... with result {result}");

            return result;
        }

        public static TcpClient CommonConnect(string hostName, 
                                               int port)
        {
            TcpClient retClient = null;

            try
            {
               
                LingerOption lingerOption = new LingerOption(true, 0);

                TcpClient tcpClient = new TcpClient
                { SendTimeout = PrivateOptions.SendTimeout, ReceiveTimeout = PrivateOptions.ReceiveTimeout, LingerState = lingerOption };
                //operations

                if (tcpClient != null)
                {
                    tcpClient.Connect(hostName, port);
                    if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                        Console.WriteLine($"Client {DateTime.Now} after Connect");

                }


                retClient = tcpClient;
            }
            catch (IOException)
            {
                Trace.TraceError("--- Connection lost");
                Console.WriteLine($"Client {DateTime.Now} ---Connection lost");
            }
            catch (SocketException ex)
            {
                Trace.TraceError("--- Can't connect: " + ex.Message);
                Console.WriteLine($"Client {DateTime.Now} ---Cant connect: {ex.Message}");
            }
            catch (Exception ex)
            {
                Trace.TraceError("--- Exception " + ex.Message);
                Console.WriteLine($"Client {DateTime.Now} ---Exception: {ex.Message}");
            }

            return retClient;
        }

        private static bool CommonDisconnect(TcpClient tcpClient)
        {
            bool result = false;
        
            try
            { 
                tcpClient.Close();
                if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                    Console.WriteLine($"Client {DateTime.Now} after tcpclient close");
                tcpClient.Dispose();
                if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                    Console.WriteLine($"Client {DateTime.Now} after tcpclient dispose");

                result = true;
            }
            catch (IOException)
            {
                Trace.TraceError("--- IO Exception");
                Console.WriteLine($"Client {DateTime.Now} ---IO connection");
            }
            catch (SocketException ex)
            {
                Trace.TraceError("--- Soecked Exception: " + ex.Message);
                Console.WriteLine($"Client {DateTime.Now} ---Soecket Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Trace.TraceError("--- Exception " + ex.Message);
                Console.WriteLine($"Client {DateTime.Now} ---Cant connect: {ex.Message}");
            }

            return result;
        }

        public static bool Connect(string hostName, 
                                int port )
        {
            
            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} doing Connect");

            CurrentClient = CommonConnect(hostName, port);
            if (CurrentClient == null)
                return false;
            else
            {
                CurrentStream = CreateCurrentStream(CurrentClient);
                return true;
            }
        }

        public static bool Disconnect()
        {
            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} doing Disconnect");

            bool result = false;
            if (CurrentClient != null)
            {
                if (CurrentStream != null)
                    RemoveStream(CurrentStream);
                CurrentStream = null;
                result = CommonDisconnect(CurrentClient);
                CurrentClient = null;
            }

            return result;
        }

        public static NetworkStream CreateCurrentStream(TcpClient client)
        {
            if(PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} getting StayStream");
            CurrentStream = client.GetStream();
            return CurrentStream;
        }

        public static bool RemoveStream( NetworkStream stream )
        {
           
            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} remove stream");
            stream.Close();

            if (PrivateOptions.DebugOption == Debug_Option.NetworkClientDataConsole)
                Console.WriteLine($"Client {DateTime.Now} after stream close");

            return true;
        }

        public static bool IsConnected()
        {
            if ( ( CurrentClient == null ) || (CurrentStream == null) )
                    return false;
            else
                return true;
        }
     
    }
}