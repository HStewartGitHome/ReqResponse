namespace ReqResponse.Models
{
    public enum Result_Options
    {
        Ok = 0,
        FailedConnection = 1,
        InvalidParameterValue1 = 2,
        InvalidParameterValue2 = 3,
        InvalidRequestMethod = 4,
        ExceptionParsingRequest = 5,
        ExceptionParseResponse = 6,
        Exception = 7,
        NullRequest = 8,
        MathError = 9,
        ValueMismatch = 10,
        Unknown = 100
    }

    [System.Flags]
    public enum Param_Option
    {
        IntValue = 1,
        StringValue = 2,
        NoneZeroIntValue = 4,
        None = 0
    }

    [System.Flags]
    public enum Request_Option
    {
        Connected = 1,
        Local = 2,
        StayConnected =4,
        None = 0
    }

    public enum Debug_Option
    {
        None = 0,
        NetworkConsole = 1,
        NetworkTrace = 2,
        NetworkErrorConsole = 3,
        NetworkErrorTrace = 4,
        NetworkServerConnection = 5,
        NetworkServerData = 6,
        NetworkClientDataConsole = 7,
        NetworkClientDataTrace = 8,
        Default = NetworkServerConnection
    }

    public enum Test_Options
    {
        UnitTest_Connect = 1,
        UnitTest_None = 0,
        Default = UnitTest_Connect
    }
}