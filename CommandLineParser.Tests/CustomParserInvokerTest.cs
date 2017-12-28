using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichTea.CommandLineParser.Tests.ParameterObjects;
using RichTea.CommandLineParser.Tests.ParameterParsers;

namespace RichTea.CommandLineParser.Tests
{
    [TestClass]
    public class CustomParserInvokerTest
    {
        private static MethodInvocationInfo methodInvocationInfo;

        [TestInitialize]
        public void Setup()
        {
            methodInvocationInfo = null;
        }

        [ClCommand("single-custom-type-test-method")]
        public static void SingleCustomTypeTestMethod(
            [ClArgs("person")]
            Person person
        )
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "single-custom-type-test-method"
            }.AddParameter(nameof(person), person);
        }

        [ClCommand("multi-custom-type-test-method")]
        public static void MultiCustomTypeTestMethod(
            [ClArgs("person-a")]
            Person personA,
            [ClArgs("person-b")]
            Person personB
        )
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "multi-custom-type-test-method"
            }.AddParameter(nameof(personA), personA)
            .AddParameter(nameof(personB), personB);
        }

        [ClCommand("mixed-custom-type-test-method")]
        public static void MixedfCustomTypeTestMethod(
            [ClArgs("person")]
            Person person,
            [ClArgs("number")]
            int number
        )
        {
            methodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "mixed-custom-type-test-method"
            }.AddParameter(nameof(person), person)
            .AddParameter(nameof(number), number);
        }

        [TestMethod]
        public void SingleCustomTypeTest()
        {
            string forename = "Tommy";
            string surname = "Biggles";
            string[] args = { "single-custom-type-test-method", "-person", forename, surname };
            var invoker = new CommandLineParserInvoker().AddParameterParser(new PersonParameterParser());
            var command = invoker.GetCommand(typeof(CustomParserInvokerTest), args);
            command.Invoke();

            // expectation
            var person = new Person
            {
                Forename = forename,
                Surname = surname
            };

            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "single-custom-type-test-method"
            }.AddParameter(nameof(person), person);

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

        [TestMethod]
        public void MultiCustomTypeTest()
        {
            string forenameA = "Tommy";
            string surnameA = "Biggles";

            string forenameB = "Timmy";
            string surnameB = "Boggles";

            string[] args = { "multi-custom-type-test-method", "-person-a", forenameA, surnameA, "-person-b", forenameB, surnameB };
            var invoker = new CommandLineParserInvoker().AddParameterParser(new PersonParameterParser());
            var command = invoker.GetCommand(typeof(CustomParserInvokerTest), args);
            command.Invoke();

            // expectation
            var personA = new Person
            {
                Forename = forenameA,
                Surname = surnameA
            };

            var personB = new Person
            {
                Forename = forenameB,
                Surname = surnameB
            };

            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "multi-custom-type-test-method"
            }.AddParameter(nameof(personA), personA)
            .AddParameter(nameof(personB), personB);

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

        [TestMethod]
        public void MixedCustomTypeTest()
        {
            string forename = "Tommy";
            string surname = "Biggles";

            int number = 606;

            string[] args = { "mixed-custom-type-test-method", "-person", forename, surname, "-number", number.ToString() };
            var invoker = new CommandLineParserInvoker().AddParameterParser(new PersonParameterParser());
            var command = invoker.GetCommand(typeof(CustomParserInvokerTest), args);
            command.Invoke();

            // expectation
            var person = new Person
            {
                Forename = forename,
                Surname = surname
            };

            var expectedMethodInvocationInfo = new MethodInvocationInfo
            {
                MethodName = "mixed-custom-type-test-method"
            }.AddParameter(nameof(person), person)
            .AddParameter(nameof(number), number);

            Assert.AreEqual(expectedMethodInvocationInfo, methodInvocationInfo);
        }

    }
}
