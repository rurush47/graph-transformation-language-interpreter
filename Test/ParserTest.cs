using System.Collections.Generic;
using GraphTransformationLanguage;
using Microsoft.Msagl.Drawing;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class ParserTest
    {
        string _testFilesPath = TestContext.CurrentContext.TestDirectory;
        
        [Test]
        public void TestParser()
        {
            Source source = new Source(_testFilesPath + @"\test_parser.txt");
            Lexer lexer = new Lexer(source);
            Parser parser = new Parser(lexer);
            
            Assert.DoesNotThrow(parser.ParseFile);
            
            source = new Source(_testFilesPath + @"\test_parser2.txt");
            lexer = new Lexer(source);
            parser = new Parser(lexer);
            Assert.DoesNotThrow(parser.ParseFile);
        }
        
        [Test]
        public void TestParserTokenError()
        {
            Source source = new Source(_testFilesPath + @"\test_parser_error.txt");
            Lexer lexer = new Lexer(source);
            Parser parser = new Parser(lexer);
            Assert.Throws<InvalidTokenException>(parser.ParseFile);
        }

        [Test]
        public void TestPareserResults()
        {
            Source source = new Source(_testFilesPath + @"\test_parser.txt");
            Lexer lexer = new Lexer(source);
            Parser parser = new Parser(lexer);

            parser.ParseFile();
            
            List<Rule> rules = parser.Rules;
            List<string> production = parser.Production;
            Dictionary<string, string> config = parser.Config;
            Graph graph = parser.StartGraph;
            
            
            Assert.AreEqual(4, rules.Count);
            Assert.AreEqual(6, production.Count);
            Assert.AreEqual(2, config.Count);
            
            Assert.AreEqual("rule1", production[0]);
            Assert.AreEqual("rule2", production[1]);
            Assert.AreEqual("rule3", production[2]);
            Assert.AreEqual("rule4", production[3]);
            Assert.AreEqual("rule4", production[4]);
            Assert.AreEqual("rule4", production[5]);
        }
    }
}