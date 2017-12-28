using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RichTea.CommandLineParser.Tests
{
    [TestClass]
    public class ParseArgsTest
    {

        [TestMethod]
        public void ParseArgsBasicTest()
        {
            int[] a = new[] { 909 };
            string[] b = new[] { "testB" };
            string[] args = { "array-test-method", "-a", string.Join(" ", a), "-b", string.Join(" ", b) };
            var invoker = new CommandLineParserInvoker();
            var acutalParsedArgs = invoker.ParseArgs(args);

            ParsedArgs expectedParsedArgs = new ParsedArgs
            {
                Verb = "array-test-method"
            };
            expectedParsedArgs.Add("a", new List<string> { "909" });
            expectedParsedArgs.Add("b", new List<string> { "testB" });

            Assert.AreEqual(expectedParsedArgs.Verb, acutalParsedArgs.Verb);
            CollectionAssert.AreEquivalent(expectedParsedArgs["a"], acutalParsedArgs["a"]);
            CollectionAssert.AreEquivalent(expectedParsedArgs["b"], acutalParsedArgs["b"]);
        }

        [TestMethod]
        public void ParseArgsColonTest()
        {
            string[] args = { "array-test-method", "-a:909", "-b:testB" };
            var invoker = new CommandLineParserInvoker();
            var acutalParsedArgs = invoker.ParseArgs(args);

            ParsedArgs expectedParsedArgs = new ParsedArgs
            {
                Verb = "array-test-method"
            };
            expectedParsedArgs.Add("a", new List<string> { "909" });
            expectedParsedArgs.Add("b", new List<string> { "testB" });

            Assert.AreEqual(expectedParsedArgs.Verb, acutalParsedArgs.Verb);
            CollectionAssert.AreEquivalent(expectedParsedArgs["a"], acutalParsedArgs["a"]);
            CollectionAssert.AreEquivalent(expectedParsedArgs["b"], acutalParsedArgs["b"]);
        }

        [TestMethod]
        public void ParseArgsArrayTest()
        {
            string[] args = { "array-test-method", "-a", "909", "818", "-b", "testB", "testBB" };
            var invoker = new CommandLineParserInvoker();
            var acutalParsedArgs = invoker.ParseArgs(args);

            ParsedArgs expectedParsedArgs = new ParsedArgs
            {
                Verb = "array-test-method"
            };
            expectedParsedArgs.Add("a", new List<string> { "909", "818" });
            expectedParsedArgs.Add("b", new List<string> { "testB", "testBB" });

            Assert.AreEqual(expectedParsedArgs.Verb, acutalParsedArgs.Verb);
            CollectionAssert.AreEquivalent(expectedParsedArgs["a"], acutalParsedArgs["a"]);
            CollectionAssert.AreEquivalent(expectedParsedArgs["b"], acutalParsedArgs["b"]);
        }
    }
}
