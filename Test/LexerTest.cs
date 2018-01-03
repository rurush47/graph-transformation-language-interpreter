using System.Collections.Generic;
using GraphTransformationLanguage;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class LexerTest
    {
        string _testFilesPath = TestContext.CurrentContext.TestDirectory;
        
        [Test]
        public void TestLexer()
        {
            List<Token> tokens = new List<Token>();            
            Source source = new Source(_testFilesPath + @"\test_lexer.txt");
            Lexer lexer = new Lexer(source);

            Token t = lexer.GetNextToken(); 
            while (t.Type != TokenType.EOF)
            {
                tokens.Add(t);
                t = lexer.GetNextToken();
            }
            
            Assert.True(tokens.Count == 15);
            Assert.AreEqual(tokens[0].Type, TokenType.Semicolon);
            Assert.AreEqual(tokens[1].Type, TokenType.Colon);
            Assert.AreEqual(tokens[2].Type, TokenType.Equals);
            Assert.AreEqual(tokens[3].Type, TokenType.Configuration);
            Assert.AreEqual(tokens[4].Type, TokenType.Rules);
            Assert.AreEqual(tokens[5].Type, TokenType.Production);
            Assert.AreEqual(tokens[6].Type, TokenType.LBrace);
            Assert.AreEqual(tokens[7].Type, TokenType.RBrace);
            Assert.AreEqual(tokens[8].Type, TokenType.Edge);
            Assert.AreEqual(tokens[9].Type, TokenType.Number);
            Assert.AreEqual(tokens[10].Type, TokenType.Colon);
            Assert.AreEqual(tokens[11].Type, TokenType.Identifier);
            Assert.AreEqual(tokens[12].Type, TokenType.VBar);
            Assert.AreEqual(tokens[13].Type, TokenType.Translate);
            Assert.AreEqual(tokens[14].Type, TokenType.Start);
        }

        [Test]
        public void TestInvalidCharacter()
        {          
            Source source = new Source(_testFilesPath + @"\test_invalid_char.txt");
            Lexer lexer = new Lexer(source);
            Assert.Throws<InvalidCharacterException>(() => lexer.GetNextToken());
        }
    }
}