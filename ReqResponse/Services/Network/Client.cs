using ReqResponse.Models;
using ReqResponse.Support;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ReqResponse.Services.Network
{
    public class Client
    {
        public static string XmlResult { get; set; }

        public static bool SendRequest(string xml,
                                       string hostName,
                                        int port)
        {
            bool result = false;
            var encoding = Encoding.ASCII;
            Byte[] bytes = new Byte[4096];
            string data;
            int i;
            Options options = Factory.GetOptions();

            try
            {
                XmlResult = "";
                LingerOption lingerOption = new LingerOption(true, 0);

                using var tcpClient = new TcpClient
                { SendTimeout = 2000, ReceiveTimeout = 2000, LingerState = lingerOption };
                //operations
                tcpClient.Connect(hostName, port);
                NetworkStream stream = tcpClient.GetStream();
                stream.ReadTimeout = 2000;

                if (stream.CanWrite)
                {
                    var bytesToSend = encoding.GetBytes(xml + "\r\n");
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
                }

                if (stream.CanRead)
                {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        if (options.DebugOption == Debug_Option.NetworkClientDataConsole)
                            Console.WriteLine("Received: {0}", data);
                        else if (options.DebugOption == Debug_Option.NetworkClientDataTrace)
                            Trace.TraceInformation("Received: {0}", data);
                        XmlResult = data;
                        result = true;
                    }
                }

                stream.Close();
                stream = null;
                tcpClient.Close();
                tcpClient.Dispose();
            }
            catch (IOException)
            {
                Trace.TraceError("--- Connection lost");
            }
            catch (SocketException ex)
            {
                Trace.TraceError("--- Can't connect: " + ex.Message);
            }

            return result;
        }
    }
}