﻿using GraphTransformationLanguage;
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
            
            Assert.AreEqual(true, true);
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
    }
}