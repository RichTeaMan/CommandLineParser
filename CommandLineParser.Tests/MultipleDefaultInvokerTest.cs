using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichTea.CommandLineParser.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace RichTea.CommandLineParser.Tests
{
    [TestClass]
    public class MultipleDefaultInvokerTest
    {
        private static MethodInvocationInfo methodInvocationInfo;

        [TestInitialize]
        public void Setup()
        {
            methodInvocationInfo = null;
        }

        [DefaultClCommand]
        public static void DefaultTestMethodA(
            [ClArgs("a")]
            int a,
            [ClArgs("b")]
            string b
        )
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "default-test-method-a"
            }.AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b);
        }

        [DefaultClCommand]
        public static void DefaultTestMethodB(
            [ClArgs("a")]
            int a,
            [ClArgs("b")]
            string b
)
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "default-test-method-b"
            }.AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b);
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousConfigurationException))]
        public void MultipleDefaultBasicInvocationParamsTest()
        {
            int a = 909;
            string b = "testB";
            string[] args = { "-a", a.ToString(), "-b", b.ToString() };
            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(MultipleDefaultInvokerTest), args);
            command.Invoke();
        }

    }
}
