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
        VBar,
        Start
    }
    
    public struct Position
    {   
        public int Symbol;
        public int Line;
    }
    
    public class Token
    {
        public TokenType Type { get; set; }
        public string Identifier { get; set; }
        public Position Position { get; set;}
        
        public Token(TokenType type, string identifier, Position pos)
        {
            Type = type;
            Identifier = identifier;
            Position = pos;
        }
    }
}