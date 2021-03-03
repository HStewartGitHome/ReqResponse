namespace ReqResponse.Models
{
    public class Options
    {
        public Options()
        {
            DebugOption = Debug_Option.Default;
            //DebugOption = Debug_Option.NetworkClientDataConsole;

            ServerDebugOption = Debug_Option.NetworkServerData;
            //ServerDebugOption = Debug_Option.NetworkServerConnection;
           
            TestOption = Test_Options.Default;
            //TestOption = Test_Options.UnitTest_None;


            NetLimit = 4;
            StayNetLimit = 10;
            HostName = "localhost";
            Port = 11000;
            SendTimeout = 1000;
            ReadTimeout = 1000;
            ReceiveTimeout = 1000;
            BufferSize = 4096;
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
    }
}