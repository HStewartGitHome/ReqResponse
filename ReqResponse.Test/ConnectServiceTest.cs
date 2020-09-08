using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReqResponse.Models;
using ReqResponse.Services;

namespace ReqResponse.Test
{
    [TestClass]
    public class ConnectServiceTest
    {
        #region Blank XML

        [TestMethod]
        public void TestConnectBlankXml()
        {
            IService service = GetService();
            MiscTest.TestBlankXml(service);
        }

        #endregion Blank XML

        #region Invalid XML

        [TestMethod]
        public void TestInvalidXml()
        {
            IService service = GetService();
            MiscTest.TestInvalidXml(service);
        }

        #endregion Invalid XML

        #region Add

        [DataTestMethod]
        [DataRow("1", "2", "3")]
        [DataRow("400", "212", "612")]
        [DataRow("0", "0", "0")]
        [DataRow("22400", "0", "22400")]
        [DataRow("222222", "-222221", "1")]
        [DataRow("-5566", "212", "-5354")]
        public void TestAddConnectSuccessXml(string value1, string value2, string result)
        {
            IService service = GetService();
            AddTest.TestSuccessXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("1", "Test", "3")]
        [DataRow("1", "", "612")]
        [DataRow("Test", "1", "3")]
        [DataRow("", "1", "612")]
        public void TestAddConnectFailXml(string value1, string value2, string result)
        {
            IService service = GetService();
            AddTest.TestFailXml(service, value1, value2, result);
        }

        #endregion Add

        #region Subtract

        [DataTestMethod]
        [DataRow("1", "2", "-1")]
        [DataRow("400", "212", "188")]
        [DataRow("0", "0", "0")]
        [DataRow("22400", "0", "22400")]
        [DataRow("222222", "-222221", "444443")]
        [DataRow("-5566", "212", "-5778")]
        public void TestSubtractConnectSuccessXml(string value1, string value2, string result)
        {
            IService service = GetService();
            SubtractTest.TestSuccessXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("1", "Test", "3")]
        [DataRow("1", "", "612")]
        [DataRow("Test", "1", "3")]
        [DataRow("", "1", "612")]
        public void TestSubtractConnectFailXml(string value1, string value2, string result)
        {
            IService service = GetService();
            SubtractTest.TestFailXml(service, value1, value2, result);
        }

        #endregion Subtract

        #region Multiply

        [DataTestMethod]
        [DataRow("1", "2", "2")]
        [DataRow("400", "212", "84800")]
        [DataRow("0", "0", "0")]
        [DataRow("22400", "0", "0")]
        [DataRow("22400", "3111", "69686400")]
        [DataRow("-5566", "212", "-1179992")]
        public void TestMultiplyConnectSuccessXml(string value1, string value2, string result)
        {
            IService service = GetService();
            MultiplyTest.TestSuccessXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("1", "Test", "3")]
        [DataRow("1", "", "612")]
        [DataRow("Test", "1", "3")]
        [DataRow("", "1", "612")]
        public void TestMultiplyConnectFailXml(string value1, string value2, string result)
        {
            IService service = GetService();
            MultiplyTest.TestFailXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("222222", "-222221", "-49382395062")]
        public void TestMultiplyConnectMathFailXml(string value1, string value2, string result)
        {
            IService service = GetService();
            MultiplyTest.TestFailXml(service, value1, value2, result);
        }

        #endregion Multiply

        #region Divide

        [DataTestMethod]
        [DataRow("22400", "3111", "7")]
        [DataRow("4", "2", "2")]
        [DataRow("400", "25", "16")]
        [DataRow("-5566", "212", "-26")]
        public void TestDivideConnectSuccessXml(string value1, string value2, string result)
        {
            IService service = GetService();
            DivideTest.TestSuccessXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("1", "Test", "3")]
        [DataRow("1", "", "612")]
        [DataRow("Test", "1", "3")]
        [DataRow("", "1", "612")]
        public void TestDivideConnectFailXml(string value1, string value2, string result)
        {
            IService service = GetService();
            MultiplyTest.TestFailXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("222222", "0", "0")]
        public void TestDivideByZeroConnectFailXml(string value1, string value2, string result)
        {
            IService service = GetService();
            DivideTest.TestFailXml(service, value1, value2, result);
        }

        #endregion Divide

        #region private

        private IService GetService()
        {
            IService service = null;
            Options options = new Options();
            if (options.TestOption == Test_Options.UnitTest_None)
                service = new Service();
            else
                service = new ConnectService();

            return service;
        }

        #endregion private
    }
}