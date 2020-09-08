using System;

namespace TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Process.TestInvaldXml();
            Process.TestBlankXML();
            Process.TestXml("1", "2", "3");
            Process.TestXml("100", "200", "300");
            Process.TestXml("1", "Test", "3");
            Process.TestXml("1", "", "612");
            Process.TestDivideByZeroXml();
        }
    }
}