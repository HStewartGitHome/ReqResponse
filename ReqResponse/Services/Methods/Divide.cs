using ReqResponse.Models;
using System;

namespace ReqResponse.Services.Methods
{
    public class Divide : BaseMethod
    {
        public override Response ExecuteRequest(Request request)
        {
            Response response = base.ExecuteRequest(request);
            if (response.Result == Result_Options.Ok)
            {
                int value1 = Convert.ToInt32(request.Value1);
                int value2 = Convert.ToInt32(request.Value2);
                try
                {
                    // The following line raises an exception because it is checked.
                    int result = checked(value1 / value2);
                    response.ResultValue = result.ToString();
                }
                catch (System.OverflowException)
                {
                    response.Result = Result_Options.MathError;
                }
            }

            return response;
        }

        public override void SetMethodInfo(MethodInfo info)
        {
            info.MethodName = "Divide";
            info.Value2Option = Param_Option.NoneZeroIntValue;
            base.SetMethodInfo(info);
        }
    }
}