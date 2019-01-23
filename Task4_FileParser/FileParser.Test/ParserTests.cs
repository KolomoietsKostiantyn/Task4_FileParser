using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task4_FileParser.Logick;

namespace FileParser.Test
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ValidationArr_Lenth2_OK()
        {
            string[] arr = { "5", "6" };

            Parser parser = new Parser();

            bool result = parser.ValidationArr(arr);

            Assert.IsTrue(result);
        }

    }
}
