using System;

namespace GraphTransformationLanguage
{
    public class InvalidCharacterException : Exception
    {
        public InvalidCharacterException(string message)
            : base(message)
        {
     
        }    
    }
    
    public class Lexer
    {
        private Source _source;
        private char _c;

        public Lexer(Source source)
        {
            _source = source;
            _c = source.GetNextChar();
        }
        
        public Token GetNextToken()
        {
            while (!_source.EndOfFile)
            {
                if (_c == ' ' ||
                    _c == '\t' ||
                    _c == '\n' ||
                    _c == '\r')
                {
                    Consume();
                }
                else if (_c == ';')
                {
                    Consume();
                    return new Token(TokenType.Semicolon, ";");
                }
                else if (_c == ',')
                {
                    Consume();
                    return new Token(TokenType.Colon, ",");
                }
                else if (_c == '{')
                {
                    Consume();
                    return new Token(TokenType.LBrace, "{");
                }
                else if (_c == '}')
                {
                    Consume();
                    return new Token(TokenType.RBrace, "}");
                }
                else if (_c == '|')
                {
                    Consume();
                    return new Token(TokenType.VBar, "|");
                }
                else if (_c == '-')
                {
                    Consume();
                    if (_c == '>')
                    {
                        Consume();
                        return new Token(TokenType.Edge, "->");
                    }
                }
                else if (_c == '=')
                {
                    Consume();
                    if (_c == '=')
                    {
                        Consume();
                        if (_c == '>')
                        {
                            Consume();
                            return new Token(TokenType.Translate, "==>");
                        }
                        else
                        {
                            RaiseError();                            
                        }
                    }
                    else
                    {
                        return new Token(TokenType.Equals, "=");
                    }
                }
                else if (char.IsDigit(_c))
                {
                    string v = "";
                    while (!_source.EndOfFile && char.IsDigit(_c))
                    {
                        v += _c;
                        Consume();
                    }
                    return new Token(TokenType.Number, v);
                }
                else if (char.IsLetter(_c))
                {
                    string v = "";
                    while (!_source.EndOfFile && char.IsLetter(_c))
                    {
                        v += _c;
                        Consume();
                    }
                    if (v == "config")
                    {
                        return new Token(TokenType.Configuration, v);                        
                    }
                    else if (v == "rules")
                    {
                        return new Token(TokenType.Rules, v);
                    }
                    else if(v == "production")
                    {
                        return new Token(TokenType.Production, v);
                    }                    
                    return new Token(TokenType.Identifier, v);
                }
                else
                {
                    RaiseError();
                }
            }
            return new Token(TokenType.EOF, "<EOF>");
        }

        private void Consume()
        {
            _c = _source.GetNextChar();
        }

        private void RaiseError()
        {
            string errorPos = _source.GetCurrentPosition();
            throw new InvalidCharacterException("Invalid character \"" + _c  + "\" at " + errorPos);
        }
    }
}