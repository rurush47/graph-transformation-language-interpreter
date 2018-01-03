using System;
using System.Collections.Generic;
using System.Data;

namespace GraphTransformationLanguage
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message)
            : base(message)
        {
     
        }    
    }
    
    struct Config
    {

    }

    struct Rule
    {

    }

    struct Graph
    {

    }

    public class Parser
    {
        private Lexer _lexer;
        private Token _lookahead;
        private Dictionary<string, string> _config = new Dictionary<string, string>();
        private List<Rule> _rules = new List<Rule>();
        private Graph _startGraph;
        private int _numVerticesParsed;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _lookahead = lexer.GetNextToken();
        }

        public void ParseFile()
        {
            //konfiguracja, graf_początkowy, reguły, [produkcja];
            ParseConfig();
            ParseStartGraph();
            ParseRules();
            ParseProduction();
        }

        private void ConsumeToken()
        {
            _lookahead = _lexer.GetNextToken();
        }

        private void Error(string expected)
        {
            throw new InvalidTokenException(
                "Expected " + expected + ", found: " + _lookahead.Type.ToString() +
                " on line: " + _lookahead.Position.Line + " position: " +
                _lookahead.Position.Symbol);
        }

        private Token Match(TokenType tokenType)
        {
            Token token = _lookahead;

            if (_lookahead.Type == tokenType)
            {
                ConsumeToken();
            }
            else
            {
                Error(tokenType.ToString());
            }

            return token;
        }

        private void ParseConfig()
        {
            Match(TokenType.Configuration);
            Match(TokenType.LBrace);
            ParseConfigList();
            Match(TokenType.RBrace);
        }

        private void ParseConfigList()
        {
            // list -> singleConfig ;
            while (_lookahead.Type == TokenType.Identifier)
            {
                ParseSingleConfig();
                Match(TokenType.Semicolon);
            }
        }

        private void ParseSingleConfig()
        {
            // singleConfig -> id = id | number
            string key = Match(TokenType.Identifier).Identifier;
            string val = "";

            Match(TokenType.Equals);
            switch (_lookahead.Type)
            {
                case TokenType.Identifier:
                    val = Match(TokenType.Identifier).ToString();
                    break;
                case TokenType.Number:
                    val = Match(TokenType.Number).ToString();
                    break;
                default:
                    Error("Identifier or number");
                    break;
            }

            _config.Add(key, val);
        }
        
        private void ParseStartGraph()
        {
            Match(TokenType.Start);
            Match(TokenType.LBrace);
            _startGraph = ParseGraph();
            Match(TokenType.Semicolon);
            Match(TokenType.RBrace);
        }
        
        private void ParseRules()
        {
            Match(TokenType.Rules);
            Match(TokenType.LBrace);
            ParseRulesList();
            Match(TokenType.RBrace);
        }

        private void ParseRulesList()
        {
            while (_lookahead.Type == TokenType.VBar)
            {
                ParseSingleRule();
                Match(TokenType.Semicolon);
            }
        }

        private Rule ParseSingleRule()
        {
            Match(TokenType.VBar);
            string name = Match(TokenType.Identifier).Identifier;
            Match(TokenType.VBar);
            Graph leftSide = ParseGraph();
            Match(TokenType.Translate);
            Graph rightSide = ParseGraph();
            
            //TODO create normal rule
            return new Rule();
        }
        
        private void ParseProduction()
        {
            if (_lookahead.Type == TokenType.Production)
            {
                Match(TokenType.Production);
                Match(TokenType.LBrace);
                ParseProductionList();
                Match(TokenType.RBrace);    
            }
            else
            {
                //TODO check for EOF
            }
        }

        private void ParseProductionList()
        {
            while (_lookahead.Type == TokenType.Identifier)
            {
                string name = Match(TokenType.Identifier).Identifier;
                Match(TokenType.Comma);
            }
        }

        private Graph ParseGraph()
        {
            Graph g = new Graph();
            ParseEdgeList(g);
            while (_lookahead.Type == TokenType.Comma)
            {
                Match(TokenType.Comma);
                ParseEdgeList(g);
            }
            
            return new Graph();
        }

        private void ParseEdgeList(Graph g)
        {
            ParseNodeIdentifier(g);

            while (_lookahead.Type == TokenType.Edge)
            {
                Match(TokenType.Edge);
                ParseNodeIdentifier(g);
            }
        }

        private void ParseNodeIdentifier(Graph g)
        {
            switch (_lookahead.Type)
            {
                case TokenType.Identifier:
                    Match(TokenType.Identifier);
                    break;
                case TokenType.Number:
                    Match(TokenType.Number);
                    Match(TokenType.Colon);
                    Match(TokenType.Identifier);
                    break;
            }

            AddNodeToGraph(g);
        }

        private void AddNodeToGraph(Graph graph)
        {
            
        }
    }
}