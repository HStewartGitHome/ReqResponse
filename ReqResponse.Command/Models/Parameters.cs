using System.Collections.Generic;

namespace ReqResponse.Command.Models
{
    public static class Parameters
    {
        public static List<Tests> TestToPerform { get; set; }
        public static Tests Test;
        public static bool DoEmail { get; set; }
        public static bool ErrorReportNotExecuted { get; set; }
    }
}