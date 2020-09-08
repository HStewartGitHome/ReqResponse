using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReqResponse.Models;
using ReqResponse.Services;
using ReqResponse.Support;

namespace ReqResponse.Test
{
    public class MiscTest
    {
        public static void TestBlankXml(IService service)
        {
            string inputXML = "";
            string outputXML;
            Response response;

            outputXML = service.ExecuteXMLRequest(inputXML);
            response = XmlHelper.DeserializeObject<Response>(outputXML);
            Assert.AreNotEqual(Result_Options.Ok, response.Result, "Result should not be Ok result = " + response.Result);
            Assert.IsTrue((service.LastResult == Models.Result_Options.NullRequest), "Not empty request");
        }

        public static void TestInvalidXml(IService service)
        {
            string inputXML = XmlHelper.CreateRequestString("Mult", "1", "2");
            string outputXML;
            Response response;

            outputXML = service.ExecuteXMLRequest(inputXML);
            response = XmlHelper.DeserializeObject<Response>(outputXML);

            if (service.IsConnectedService)
                Assert.AreNotEqual(Result_Options.FailedConnection, response.Result, "Result should not be Fail Connection");
            Assert.AreEqual(Result_Options.InvalidRequestMethod, response.Result, "Result should be Invalid Request Method result = " + response.Result);
            Assert.AreNotEqual(Result_Options.Ok, response.Result, "Result should not be Ok result = " + response.Result);
        }
    }
}