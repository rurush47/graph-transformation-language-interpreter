namespace GraphTransformationLanguage
{
    public enum TokenType
    {
        EOF,
        Semicolon,
        Colon,
        Equals,
        Configuration,
        Rules,
        Production,
        LBrace,
        RBrace,
        Edge,
        Translate,
        Number,
        Comma,
        Identifier,
        VBar
    }
    
    public class Token
    {
        public TokenType Type { get; set; }
        public string Identifier { get; set; }

        public Token(TokenType type, string identifier)
        {
            Type = type;
            Identifier = identifier;
        }
    }
}