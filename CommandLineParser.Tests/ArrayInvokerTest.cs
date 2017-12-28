using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RichTea.CommandLineParser.Tests
{
    [TestClass]
    public class ArrayInvokerTest
    {
        private static MethodInvocationInfo methodInvocationInfo;

        [TestInitialize]
        public void Setup()
        {
            methodInvocationInfo = null;
        }

        [ClCommand("array-test-method")]
        public static void ArrayTestMethod(
            [ClArgs("a")]
            int[] a,
            [ClArgs("b")]
            string[] b
        )
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "array-test-method"
            }.AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b);
        }

        [TestMethod]
        public void ArraySingleInvocationParamsTest()
        {
            int[] a = new[] { 909 };
            string[] b = new[] { "testB" };
            string[] args = { "array-test-method", "-a", string.Join(" ", a), "-b", string.Join(" ", b) };
            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(ArrayInvokerTest), args);
            command.Invoke();

            // expectation
            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "array-test-method"
            }
            .AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b);

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

        [TestMethod]
        public void ArrayMultiInvocationParamsTest()
        {
            string[] args = { "array-test-method", "-a", "909", "818", "-b", "testB", "testBB" };
            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(ArrayInvokerTest), args);
            command.Invoke();

            // expectation
            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "array-test-method"
            }
            .AddParameter("a", new [] { "909", "818" })
            .AddParameter("b", new[] { "testB", "testBB" });

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

    }
}
