using ReqResponse.Models;
using ReqResponse.Support;
using System;
using System.Net;
using System.Net.Sockets;

namespace ReqResponse.Services.Network
{
    public class Server
    {
        public static void NewServer(int port)
        {
            // from Microsoft source
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?redirectedfrom=MSDN&view=netframework-4.7.2
            TcpListener server = null;
            IService service = new Service();
            Options options = Factory.GetOptions();

            try
            {
                // Set the TcpListener on port 13000.
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[4096];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                        Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                        Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        if (options.ServerDebugOption == Debug_Option.NetworkServerData)
                            Console.WriteLine($"Received: {data}");

                        string output = service.ExecuteXMLRequest(data);
                        if (options.ServerDebugOption == Debug_Option.NetworkServerData)
                            Console.WriteLine($"Response: {output}");

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(output);
                        stream.Write(msg, 0, msg.Length);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }
    }
}