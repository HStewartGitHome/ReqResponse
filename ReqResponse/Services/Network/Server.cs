using ReqResponse.Models;
using ReqResponse.Support;
using System;
using System.Net;
using System.Net.Sockets;

namespace ReqResponse.Services.Network
{
    public class Server
    {
        public static Options options = null;
        public static void NewServer(int port)
        {
            // from Microsoft source
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?redirectedfrom=MSDN&view=netframework-4.7.2
            TcpListener server = null;

            options = ConfigFactory.GetOptions();

            try
            {
                server = StartListener("127.0.0.1", port);

               

                // Enter the listening loop.
                while (true)
                {
                    if (RunListener(server) == true)
                    {
                        if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                            Console.Write("Succesfull Start Listener");
                    }
                    else
                    {
                        if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                            Console.Write("Listener failed to Start!!!!");

                       server = StartListener("127.0.0.1", port);
                    }
   
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                // Stop listening for new clients.
                StopListener(server);
            }
        }

        public static bool RunListener( TcpListener server)
        {
            bool result = false;
            IService service = new Service();
            TcpClient client = null;

            try
            {
           

                // Buffer for reading data
                Byte[] bytes = new Byte[4096];
                String data = null;


                    if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                        Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    client = server.AcceptTcpClient();
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
                result = true;
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                // Stop client
                if ( (result == false) && ( client != null ))
                    client.Close();
            }
            return result;
        }

        public static TcpListener StartAListener( string name, 
                                                 int port )
        {
            TcpListener server = null;


            try
            {
                if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                    Console.Write("Starting Listener");

                // Set the TcpListener on port 13000.
                IPAddress localAddr = IPAddress.Parse(name);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
                server = null;
            }
            finally
            {
            }

            return server;
        }

        public static TcpListener StartListener(string name,
                                               int port)
        {
            TcpListener server = null;
            int index = 0;
            int count = 4;

            try
            {
                while ((server == null) && (index < count))
                {
                    if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                        Console.Write("Starting Listener {index} of {count}");

                    server = StartAListener(name, port);

                    if (server == null)
                    {
                        if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                            Console.Write("Failed Start Listener {index} of {count}");
                    }
                    else
                    {
                        if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                            Console.Write("Successfully Started Listener {index} of {count}");

                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
                server = null;
            }
            finally
            {
            }

            return server;
        }

            public static void StopListener( TcpListener server )
                                                 
            {
            Options options = ConfigFactory.GetOptions();

    
            try
            {
                if (options.ServerDebugOption == Debug_Option.NetworkServerConnection)
                    Console.Write("Stopping Listener");

                server.Stop();

             

            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
            }
        }

    }

}
