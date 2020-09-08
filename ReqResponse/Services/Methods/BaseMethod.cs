using ReqResponse.Models;

namespace ReqResponse.Services.Methods
{
    public class BaseMethod : IBaseMethod
    {
        public BaseMethod()
        {
            Info = new MethodInfo();
            SetMethodInfo(Info);
        }

        internal MethodInfo Info { get; set; }

        public virtual Response ExecuteRequest(Request request)
        {
            Response response = new Response();
            response.Result = VerifyRequestValues(request);

            return response;
        }

        public virtual void SetMethodInfo(MethodInfo info)
        {
            Info = info;
        }

        internal Result_Options VerifyRequestValues(Request request)
        {
            Result_Options result = Result_Options.Ok;

            if (VerifyValue(request.Value1, Info.Value1Option) == false)
                result = Result_Options.InvalidParameterValue1;
            else if (VerifyValue(request.Value2, Info.Value2Option) == false)
                result = Result_Options.InvalidParameterValue2;

            return result;
        }

        internal bool VerifyValue(string str,
                                 Param_Option option)
        {
            bool result = true;

            if (str == null)
                result = false;
            else if (option == Param_Option.IntValue)
            {
                int value;
                if (int.TryParse(str, out value))
                    result = true;
                else
                    result = false;
            }
            else if (option == Param_Option.NoneZeroIntValue)
            {
                int value;
                if (int.TryParse(str, out value))
                {
                    if (value == 0)
                        result = false;
                    else
                        result = true;
                }
                else
                    result = false;
            }

            return result;
        }

        internal BaseMethod GetMethod(Request request)
        {
            BaseMethod method = null;

            return method;
        }
    }
}