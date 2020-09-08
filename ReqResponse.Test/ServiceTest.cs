using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReqResponse.Services;

namespace ReqResponse.Test
{
    [TestClass]
    public class ServiceTest
    {
        #region Blank XML

        [TestMethod]
        public void TestBlankXml()
        {
            IService service = new Service();
            MiscTest.TestBlankXml(service);
        }

        #endregion Blank XML

        #region Invalid XML

        [TestMethod]
        public void TestInvalidXml()
        {
            IService service = new Service();
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
        public void TestAddSuccessXml(string value1, string value2, string result)
        {
            IService service = new Service();
            AddTest.TestSuccessXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("1", "Test", "3")]
        [DataRow("1", "", "612")]
        [DataRow("Test", "1", "3")]
        [DataRow("", "1", "612")]
        public void TestAddFailXml(string value1, string value2, string result)
        {
            IService service = new Service();
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
        public void TestSubtractSuccessXml(string value1, string value2, string result)
        {
            IService service = new Service();
            SubtractTest.TestSuccessXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("1", "Test", "3")]
        [DataRow("1", "", "612")]
        [DataRow("Test", "1", "3")]
        [DataRow("", "1", "612")]
        public void TestSubtractFailXml(string value1, string value2, string result)
        {
            IService service = new Service();
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
        public void TestMultiplySuccessXml(string value1, string value2, string result)
        {
            IService service = new Service();
            MultiplyTest.TestSuccessXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("1", "Test", "3")]
        [DataRow("1", "", "612")]
        [DataRow("Test", "1", "3")]
        [DataRow("", "1", "612")]
        public void TestMultiplyFailXml(string value1, string value2, string result)
        {
            IService service = new Service();
            MultiplyTest.TestFailXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("222222", "-222221", "-49382395062")]
        public void TestMultiplyMathFailXml(string value1, string value2, string result)
        {
            IService service = new Service();
            MultiplyTest.TestFailXml(service, value1, value2, result);
        }

        #endregion Multiply

        #region Divide

        [DataTestMethod]
        [DataRow("22400", "3111", "7")]
        [DataRow("4", "2", "2")]
        [DataRow("400", "25", "16")]
        [DataRow("-5566", "212", "-26")]
        public void TestDivideSuccessXml(string value1, string value2, string result)
        {
            IService service = new Service();
            DivideTest.TestSuccessXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("1", "Test", "3")]
        [DataRow("1", "", "612")]
        [DataRow("Test", "1", "3")]
        [DataRow("", "1", "612")]
        public void TestDivideFailXml(string value1, string value2, string result)
        {
            IService service = new Service();
            MultiplyTest.TestFailXml(service, value1, value2, result);
        }

        [DataTestMethod]
        [DataRow("222222", "0", "0")]
        public void TestDivideByZeroFailXml(string value1, string value2, string result)
        {
            IService service = new Service();
            DivideTest.TestFailXml(service, value1, value2, result);
        }

        #endregion Divide
    }
}