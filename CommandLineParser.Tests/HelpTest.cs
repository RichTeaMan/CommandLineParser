using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace RichTea.CommandLineParser.Tests
{
    [TestClass]
    public class HelpTest
    {
        private static MethodInvocationInfo methodInvocationInfo;

        [TestInitialize]
        public void Setup()
        {
            methodInvocationInfo = null;
        }

        [ClCommand("test-method", "a test method")]
        public static void TestMethod(
            [ClArgs("a")]
            int[] a,
            [ClArgs("b")]
            string[] b
        )
        {
        }

        [ClCommand("test-method-b", "a b level test method.")]
        public static void TestBMethod(
            [ClArgs("a")]
            int[] a,
            [ClArgs("b")]
            string[] b
        )
        {
        }

        [TestMethod]
        public void HelpInvocationPrint()
        {
            string[] args = { "help" };
            Parser.ParseCommand<HelpTest>(args);
        }

        [TestMethod]
        public void HelpInvocationTest()
        {
            // capture console writeline
            string console;
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                HelpInvocationPrint();

                console = stringWriter.ToString();
            }

            Assert.IsTrue(console.Contains("test-method"), $"Help text must contain test-method. Was actually:\n{console}");
        }

        [TestMethod]
        public void HelpInvocationMethodPrint()
        {
            string[] args = { "help", "test-method" };
            Parser.ParseCommand<HelpTest>(args);
        }

        [TestMethod]
        public void HelpInvocationMethodTest()
        {
            // capture console writeline
            string console;
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                HelpInvocationMethodPrint();

                console = stringWriter.ToString();
            }

            Assert.IsTrue(console.Contains("test-method"), $"Help text must contain test-method. Was actually:\n{console}");
        }

        [TestMethod]
        public void HelpInvocationNoMethodPrint()
        {
            string[] args = { "help", "no-method" };
            Parser.ParseCommand<HelpTest>(args);
        }

        [TestMethod]
        public void HelpInvocationNoMethodTest()
        {
            // capture console writeline
            string console;
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                HelpInvocationNoMethodPrint();

                console = stringWriter.ToString();
            }

            Assert.IsTrue(console.Contains("test-method"), $"Help text must contain test-method. Was actually:\n{console}");
        }

    }
}
