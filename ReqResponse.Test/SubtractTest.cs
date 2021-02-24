using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReqResponse.Models;
using ReqResponse.Services;
using ReqResponse.Support;

namespace ReqResponse.Test
{
    public class SubtractTest
    {
        public static void TestSuccessXml(IService service,
                                          string value1,
                                          string value2,
                                          string result)
        {
            string inputXML = XmlHelper.CreateRequestString("Subtract", value1, value2);
            string outputXML;
            Response response;

            outputXML = service.ExecuteXMLRequest(inputXML);
            response = XmlHelper.DeserializeObject<Response>(outputXML);

            if (service.IsConnectedService)
                Assert.AreNotEqual(Result_Options.FailedConnection, response.Result, "Result should not be Fail Connection");
            Assert.AreEqual(Result_Options.Ok, response.Result, "Result should be Ok");
            Assert.AreEqual(result, response.ResultValue, "Result value should be " + result + " and got " + response.ResultValue);
        }

        public static void TestFailXml(IService service,
                                      string value1,
                                      string value2,
                                      string result)
        {
            string inputXML = XmlHelper.CreateRequestString("Subtract", value1, value2);
            string outputXML;
            Response response;

            outputXML = service.ExecuteXMLRequest(inputXML);
            response = XmlHelper.DeserializeObject<Response>(outputXML);

            if (service.IsConnectedService)
                Assert.AreNotEqual(Result_Options.FailedConnection, response.Result, "Result should not be Fail Connection");
            Assert.AreNotEqual(Result_Options.Ok, response.Result, $"Result should not be Ok with Result={response.Result} and Expected {result}");
        }
    }
}