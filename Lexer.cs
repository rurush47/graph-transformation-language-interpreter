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
                    return CreateToken(TokenType.Semicolon, ";");
                }
                else if (_c == ':')
                {
                    Consume();
                    return CreateToken(TokenType.Colon, ":");
                }
                else if (_c == ',')
                {
                    Consume();
                    return CreateToken(TokenType.Comma, ",");
                }
                else if (_c == '{')
                {
                    Consume();
                    return CreateToken(TokenType.LBrace, "{");
                }
                else if (_c == '}')
                {
                    Consume();
                    return CreateToken(TokenType.RBrace, "}");
                }
                else if (_c == '|')
                {
                    Consume();
                    return CreateToken(TokenType.VBar, "|");
                }
                else if (_c == '-')
                {
                    Consume();
                    if (_c == '>')
                    {
                        Consume();
                        return CreateToken(TokenType.Edge, "->");
                    }
                    else
                    {
                        RaiseError();
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
                            return CreateToken(TokenType.Translate, "==>");
                        }
                        else
                        {
                            RaiseError();                            
                        }
                    }
                    else
                    {
                        return CreateToken(TokenType.Equals, "=");
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
                    return CreateToken(TokenType.Number, v);
                }
                else if (char.IsLetter(_c) || _c == '_')
                {
                    string v = "";
                    while (!_source.EndOfFile && (char.IsLetter(_c) ||
                                                  _c == '_'  ||
                                                  char.IsDigit(_c)))
                    {
                        v += _c;
                        Consume();
                    }
                    if (v == "config")
                    {
                        return CreateToken(TokenType.Configuration, v);                        
                    }
                    else if (v == "rules")
                    {
                        return CreateToken(TokenType.Rules, v);
                    }
                    else if(v == "production")
                    {
                        return CreateToken(TokenType.Production, v);
                    }      
                    else if(v == "start")
                    {
                        return CreateToken(TokenType.Start, v);
                    } 
                    return CreateToken(TokenType.Identifier, v);
                }
                else
                {
                    RaiseError();
                }
            }
            return CreateToken(TokenType.EOF, "<EOF>");
        }

        private Token CreateToken(TokenType tokenType, string value)
        {
            return new Token(tokenType, value, _source.GetPosition());
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