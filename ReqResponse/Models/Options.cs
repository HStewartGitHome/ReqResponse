using System;

namespace ReqResponse.Models
{
    public class Options
    {
        public Options()
        {
            //DebugOption = Debug_Option.Default;
            DebugOption = Debug_Option.NetworkClientDataConsole;

            ServerDebugOption = Debug_Option.NetworkServerData;
            //ServerDebugOption = Debug_Option.NetworkServerConnection;
           
            TestOption = Test_Options.Default;
            //TestOption = Test_Options.UnitTest_None;


            NetLimit = 2;
            StayNetLimit = 25;
            SendTimeout = 1000;
            ReadTimeout = 1000;
            ReceiveTimeout = 1000;
            BufferSize = 4096;
            OutputConfiguration = true;

            ServerConfig = new()
            {
                PrimaryServer = "localhost",
                PrimaryPort = 11001,
                BackupServer = "localhost",
                BackupPort = 11002
            };
            ServerConfig.NetLimit = NetLimit;
            ServerConfig.StayNetLimit = StayNetLimit;
            ServerConfig.OutputConfiguration = OutputConfiguration;
            ServerConfig.DebugOption = (int)DebugOption;
            ServerConfig.ServerDebugOption = (int)ServerDebugOption;
            ServerConfig.TestOption = (int)TestOption;

            SetServer(ServerConfig,true);

   
        }

        public Debug_Option DebugOption { get; set; }

        public Debug_Option ServerDebugOption { get; set; }
        public Test_Options TestOption { get; set; }
        public int NetLimit { get; set; }
        public int StayNetLimit { get; set; }
        public string HostName { get; set; }
        public int Port { get; set;  }
        public int SendTimeout { get; set; }
        public int ReceiveTimeout { get; set; }
        public int ReadTimeout { get; set; }
        public int BufferSize { get; set; }
        public ServerConfiguration ServerConfig { get; set; }
        public bool UsePrimary { get; set; }
        public bool OutputConfiguration { get; set; }


        public void SetServer( ServerConfiguration serverConfig,
                                bool usePrimary )
        {
            ServerConfig = serverConfig;
            UsePrimary = usePrimary;
            if ( UsePrimary == true )
            {
                HostName = ServerConfig.PrimaryServer;
                Port = ServerConfig.PrimaryPort;
            }
            else
            {
                HostName = ServerConfig.BackupServer;
                Port = ServerConfig.BackupPort;
            }

            NetLimit = ServerConfig.NetLimit;
            StayNetLimit = ServerConfig.StayNetLimit;
            OutputConfiguration = ServerConfig.OutputConfiguration;
            DebugOption = (Debug_Option)ServerConfig.DebugOption;
            ServerDebugOption = (Debug_Option)ServerConfig.ServerDebugOption;
            TestOption = (Test_Options)ServerConfig.TestOption;

            if (OutputConfiguration == true)
            {
                Console.WriteLine($"ServerConfiguration:  UsePrimary = {UsePrimary}");
                Console.WriteLine($"                      HostName    = {HostName}  Port = {Port}");
                Console.WriteLine($"                      NetLimit    = {NetLimit}  StayNetLimit = {StayNetLimit}");
                Console.WriteLine($"                      DebugOption = {DebugOption}  ServerDebugOption = {ServerDebugOption}  TestOption = {TestOption}");
            }
        }
    }
}
