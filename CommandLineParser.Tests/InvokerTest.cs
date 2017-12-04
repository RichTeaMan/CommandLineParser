using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace CommandLineParser.Tests
{
    [TestFixture]
    public class InvokerTest
    {
        private static MethodInvocationInfo methodInvocationInfo;

        [SetUp]
        public void Setup()
        {
            methodInvocationInfo = null;
        }

        [ClCommand("test-method-no-param")]
        public static void TestMethodNoParam()
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "test-method-no-param"
            };
        }

        [ClCommand("test-method")]
        public static void TestMethod(
            [ClArgs("a")]
            int a,
            [ClArgs("b")]
            string b
        )
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "test-method"
            }.AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b);
        }

        [ClCommand("nullable-param-test-method")]
        public static void NullableParamTestMethod(
            [ClArgs("int-param", "ip")]
            int? intParam = 20
        )
        { }

        [Test]
        public void BasicInvocationNoParamsTest()
        {
            string[] args = { "test-method-no-param" };
            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(InvokerTest), args);
            command.Invoke();

            // expectation
            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "test-method-no-param"
            };

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

        [Test]
        public void BasicInvocationParamsTest()
        {
            int a = 909;
            string b = "testB";
            string[] args = { "test-method", "-a", a.ToString(), "-b", b.ToString() };
            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(InvokerTest), args);
            command.Invoke();

            // expectation
            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "test-method"
            }
            .AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b);

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

    }
}
