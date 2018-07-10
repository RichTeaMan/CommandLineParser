using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace RichTea.CommandLineParser.Tests
{
    [TestClass]
    public class AsyncInvokerTest
    {
        private static MethodInvocationInfo methodInvocationInfo;

        [TestInitialize]
        public void Setup()
        {
            methodInvocationInfo = null;
        }

        [ClCommand("async-test-method-no-param")]
        public static async Task TestMethodNoParamAsync()
        {
            await Task.Delay(500);
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "async-test-method-no-param"
            };
        }

        [ClCommand("async-generic-test-method-no-param")]
        public static async Task<int> TestGenericMethodNoParamAsync()
        {
            await Task.Delay(500);
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "async-generic-test-method-no-param"
            };
            return 0;
        }

        [TestMethod]
        public void BasicInvocationAsyncTest()
        {
            string[] args = { "async-test-method-no-param" };
            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(AsyncInvokerTest), args);
            command.Invoke();

            // expectation
            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "async-test-method-no-param"
            };

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

        [TestMethod]
        public void BasicInvocationGenericAsyncTest()
        {
            string[] args = { "async-generic-test-method-no-param" };
            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(AsyncInvokerTest), args);
            command.Invoke();

            // expectation
            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "async-generic-test-method-no-param"
            };

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

    }
}
