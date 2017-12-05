using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace RichTea.CommandLineParser.Tests
{
    [TestFixture]
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

        [ClCommand("nullable-param-test-method")]
        public static void NullableParamTestMethod(
            [ClArgs("int-param", "ip")]
            int? intParam = 20
        )
        { }

        [Test]
        public void ParseArgsNoParametersTest()
        {
            string verb = "test";

            var invoker = new CommandLineParserInvoker();
            var parsedArgs = invoker.ParseArgs(new[] { verb });

            Assert.AreEqual(verb, parsedArgs.Verb);
            Assert.AreEqual(0, parsedArgs.Count);
        }

        [Test]
        public void ParseArgsSingleParameterTest()
        {
            string verb = "test";

            var invoker = new CommandLineParserInvoker();
            var parsedArgs = invoker.ParseArgs(new[] { verb, "-t", "99" });

            Assert.AreEqual(verb, parsedArgs.Verb);
            Assert.AreEqual(1, parsedArgs.Count);
            Assert.AreEqual("99", parsedArgs["t"].Single());
        }

        [Test]
        public void IntDefaultParameterTest()
        {
            var methodInfo = GetType().GetRuntimeMethods().Single(m => m.IsStatic && m.Name == "TestMethod");

            var intParameter = methodInfo.GetParameters().Single(p => p.ParameterType == typeof(int));
            var intMethodParameter = new IntMethodParameter(intParameter);

            Assert.AreEqual("intParam", intMethodParameter.Name);
            Assert.AreEqual(true, intMethodParameter.Optional);
            Assert.AreEqual(false, intMethodParameter.IsArray);
            Assert.AreEqual(false, intMethodParameter.IsNullable);
            Assert.AreEqual(20, intMethodParameter.DefaultValue);
        }

        [Test]
        public void NullableIntDefaultParameterTest()
        {
            var methodInfo = GetType().GetRuntimeMethods().Single(m => m.IsStatic && m.Name == "NullableParamTestMethod");

            var intParameter = methodInfo.GetParameters().Single(p => p.Name == "intParam");
            var intMethodParameter = new IntMethodParameter(intParameter);

            Assert.AreEqual("intParam", intMethodParameter.Name);
            Assert.AreEqual(true, intMethodParameter.Optional);
            Assert.AreEqual(false, intMethodParameter.IsArray);
            Assert.AreEqual(true, intMethodParameter.IsNullable);
            Assert.AreEqual(20, intMethodParameter.DefaultValue);
        }

        [Test]
        public void StringDefaultParameterTest()
        {
            var methodInfo = GetType().GetRuntimeMethods().Single(m => m.IsStatic && m.Name == "TestMethod");

            var stringParameter = methodInfo.GetParameters().Single(p => p.ParameterType == typeof(string));
            var stringMethodParameter = new StringMethodParameter(stringParameter);

            Assert.AreEqual("stringParam", stringMethodParameter.Name);
            Assert.AreEqual(true, stringMethodParameter.Optional);
            Assert.AreEqual(false, stringMethodParameter.IsArray);
            Assert.AreEqual(false, stringMethodParameter.IsNullable);
            Assert.AreEqual("foo", stringMethodParameter.DefaultValue);
        }
    }
}
