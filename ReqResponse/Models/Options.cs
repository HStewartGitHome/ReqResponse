namespace ReqResponse.Models
{
    public class Options
    {
        public Options()
        {
            //DebugOption = Debug_Option.Default;
            DebugOption = Debug_Option.NetworkServerData;
            //TestOption = Test_Options.Default;
            TestOption = Test_Options.UnitTest_None;
            NetLimit = 4;
        }

        public Debug_Option DebugOption { get; set; }
        public Test_Options TestOption { get; set; }
        public int NetLimit { get; set; }
    }
}