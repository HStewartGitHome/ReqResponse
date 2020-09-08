namespace ReqResponse.Models
{
    public class MethodInfo
    {
        public MethodInfo()
        {
            Value1Option = Param_Option.IntValue;
            Value2Option = Param_Option.IntValue;
            MethodName = "";
        }

        public Param_Option Value1Option { get; set; }
        public Param_Option Value2Option { get; set; }
        public string MethodName { get; set; }
    }
}