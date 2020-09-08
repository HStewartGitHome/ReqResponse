using ReqResponse.Models;
using ReqResponse.Services;
using ReqResponse.Support;
using System;

namespace TestApp
{
    public class Process
    {
        public static void TestXml(string value1,
                                   string value2,
                                   string result)
        {
            string inputXML = XmlHelper.CreateRequestString("Add", value1, value2);
            string outputXML;
            Response response;

            IService service = new Service();
            outputXML = service.ExecuteXMLRequest(inputXML);
            response = XmlHelper.DeserializeObject<Response>(outputXML);
            Console.WriteLine("TestXml:   Response Result Value=" + response.ResultValue + " Result=" + response.Result.ToString());
        }

        public static void TestBlankXML()
        {
            string inputXML = "";
            string outputXML;
            Response response;

            IService service = new ConnectService();
            outputXML = service.ExecuteXMLRequest(inputXML);

            response = XmlHelper.DeserializeObject<Response>(outputXML);
            Console.WriteLine("TestBlankXML:  Response Result Value=" + response.ResultValue + " Result=" + response.Result.ToString());
        }

        public static void TestInvaldXml()
        {
            string inputXML = XmlHelper.CreateRequestString("Mult", "1", "2");
            string outputXML;
            Response response;

            IService service = new Service();
            outputXML = service.ExecuteXMLRequest(inputXML);
            response = XmlHelper.DeserializeObject<Response>(outputXML);
            Console.WriteLine("TestInvaldXml: Response Result Value=" + response.ResultValue + " Result=" + response.Result.ToString());
        }

        public static void TestDivideByZeroXml()
        {
            string inputXML = XmlHelper.CreateRequestString("Divide", "0", "0");
            string outputXML;
            Response response;

            IService service = new Service();
            outputXML = service.ExecuteXMLRequest(inputXML);
            response = XmlHelper.DeserializeObject<Response>(outputXML);
            Console.WriteLine("TestDivideByZeroXml: Response Result Value=" + response.ResultValue + " Result=" + response.Result.ToString());
        }
    }
}