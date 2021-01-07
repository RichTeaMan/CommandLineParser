using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichTea.CommandLineParser.ParameterParsers;
using System.Linq;
using System.Reflection;

namespace RichTea.CommandLineParser.Tests
{
    [TestClass]
    public class ClCommandTest
    {
        [ClCommand("test-method")]
        public static void TestMethod(
            [ClArgs("int-param", "ip")]
            int intParam = 20,
            [ClArgs("string-param")]
            string stringParam = "foo"
        )
        { }

        public static void TestMethodWithoutAttr(
            [ClArgs("int-param", "ip")]
            int intParam = 20,
            [ClArgs("string-param")]
            string stringParam = "foo"
 )
        { }

        [ClCommand("nullable-param-test-method")]
        public static void NullableParamTestMethod(
            [ClArgs("int-param", "ip")]
            int? intParam = 20
        )
        { }

        [TestMethod]
        public void ParseArgsNoParametersTest()
        {
            string verb = "test";

            var invoker = new CommandLineParserInvoker();
            var parsedArgs = invoker.ParseArgs(new[] { verb });

            Assert.AreEqual(verb, parsedArgs.Verb);
            Assert.AreEqual(0, parsedArgs.Count);
        }

        [TestMethod]
        public void ParseArgsSingleParameterTest()
        {
            string verb = "test";

            var invoker = new CommandLineParserInvoker();
            var parsedArgs = invoker.ParseArgs(new[] { verb, "-t", "99" });

            Assert.AreEqual(verb, parsedArgs.Verb);
            Assert.AreEqual(1, parsedArgs.Count);
            Assert.AreEqual("99", parsedArgs["t"].Single());
        }

        [TestMethod]
        public void IntParameterParserTest()
        {
            var intParameterParser = new IntParameterParser();
            var parseResult = intParameterParser.ParseParameter("test", new string[]{ "20" });

            Assert.AreEqual(20, parseResult.Parameter);
        }

        [TestMethod]
        public void StringParameterParserTest()
        {
            var stringMethodParameter = new StringParameterParser();
            var parseResult = stringMethodParameter.ParseParameter("test", new string[] { "foo" });

            Assert.AreEqual("foo", parseResult.Parameter);
        }
    }
}
