using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RichTea.CommandLineParser.Tests
{
    [TestClass]
    public class InvokerTest
    {
        private static MethodInvocationInfo methodInvocationInfo;

        [TestInitialize]
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

        [DefaultClCommand]
        public static void DefaultTestMethod(
            [ClArgs("a")]
            int a,
            [ClArgs("b")]
            string b
        )
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "default-test-method"
            }.AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b);
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

        [ClCommand("test-method-context")]
        public static void TestMethodWithContext(
            [ClArgs("a")]
            int a,
            [ClArgs("b")]
            string b,
            ParseContext parseContext
)
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "test-method"
            }.AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b)
            .AddParameter(nameof(parseContext), parseContext.Args);
        }

        [ClCommand("nullable-param-test-method")]
        public static void NullableParamTestMethod(
            [ClArgs("int-param", "ip")]
            int? intParam = 20
        )
        { }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void ContextInvocationParamsTest()
        {
            int a = 909;
            string b = "testB";
            string[] args = { "test-method-context", "-a", a.ToString(), "-b", b.ToString() };
            ParseContext parseContext = new ParseContext(args, new ParsedArgs(), new Method[0]);

            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(InvokerTest), args);
            command.Invoke();

            // expectation
            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "test-method"
            }
            .AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b)
            .AddParameter(nameof(parseContext), parseContext.Args);

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

        [TestMethod]
        public void DefaultBasicInvocationParamsTest()
        {
            int a = 909;
            string b = "testB";
            string[] args = { "-a", a.ToString(), "-b", b.ToString() };
            var invoker = new CommandLineParserInvoker();
            var command = invoker.GetCommand(typeof(InvokerTest), args);
            command.Invoke();

            // expectation
            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "default-test-method"
            }
            .AddParameter(nameof(a), a)
            .AddParameter(nameof(b), b);

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

    }
}
