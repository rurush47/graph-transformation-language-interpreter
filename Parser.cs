using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Msagl.Drawing;

namespace GraphTransformationLanguage
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message)
            : base(message)
        {
            
        }    
    }

    public class Config
    {
        public string Key;
        public string Value;
    }
    
    public class Parser
    {
        private Lexer _lexer;
        private Token _lookahead;

        public Dictionary<string, string> Config { get; private set; }
        public Graph StartGraph;
        public List<Rule> Rules { get; }
        public List<string> Production { get; set; }
        public bool FixedProduction { get; private set; }

        public Parser(Lexer lexer)
        {
            Config = new Dictionary<string, string>();
            Rules = new List<Rule>();
            Production = new List<string>();
            _lexer = lexer;
            _lookahead = lexer.GetNextToken();
        }

        public void ParseFile()
        {
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
            Config = ParseConfigList();
            Match(TokenType.RBrace);
        }

        private Dictionary<string, string> ParseConfigList()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            Config result = ParseSingleConfig(); 
            while (result != null)
            {
                config.Add(result.Key, result.Value);
                result = ParseSingleConfig();
            }
            
            return config;
        }

        private Config ParseSingleConfig()
        {
            if (_lookahead.Type != TokenType.Identifier)
            {
                return null;
            }
            
            string key = Match(TokenType.Identifier).Identifier;
            string val = "";

            Match(TokenType.Equals);
            switch (_lookahead.Type)
            {
                case TokenType.Identifier:
                    val = Match(TokenType.Identifier).Identifier;
                    break;
                case TokenType.Number:
                    val = Match(TokenType.Number).Identifier;
                    break;
                default:
                    Error("Identifier or number");
                    break;
            }
            
            Match(TokenType.Semicolon);
            return new Config
            {
                Key = key,
                Value = val
            };
        }
        
        private void ParseStartGraph()
        {
            Match(TokenType.Start);
            Match(TokenType.LBrace);
            StartGraph = ParseGraph();
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
                Rule rule = ParseSingleRule();
                Rules.Add(rule);
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
            
            Rule rule = new Rule();
            rule.SetRule(name, leftSide, rightSide);
            
            return rule;
        }
        
        private void ParseProduction()
        {
            if (_lookahead.Type == TokenType.Production)
            {
                Match(TokenType.Production);
                Match(TokenType.LBrace);
                Production = ParseProductionList();
                Match(TokenType.RBrace);

                FixedProduction = true;
                return;
            }

            FixedProduction = false;
        }

        private List<string> ParseProductionList()
        {
            List<string> production = new List<string>();
            
            while (_lookahead.Type == TokenType.Identifier)
            {
                string name = Match(TokenType.Identifier).Identifier;
                production.Add(name);
                Match(TokenType.Comma);
            }

            return production;
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
            
            return g;
        }
        
        private void ParseEdgeList(Graph g)
        {
            Node currentNode = ParseNodeIdentifier(g);
            g.AddNode(currentNode);
            
            while (_lookahead.Type == TokenType.Edge)
            {
                Match(TokenType.Edge);
                Node newNode = ParseNodeIdentifier(g);
                g.AddNode(newNode);
                
                g.AddEdge(currentNode.Id, newNode.Id);
                currentNode = newNode;
            }
        }

        private Node ParseNodeIdentifier(Graph g)
        {
            int ruleID = -1;

            if (_lookahead.Type == TokenType.Number)
            {
                ruleID = int.Parse(Match(TokenType.Number).Identifier);
                Match(TokenType.Colon);
            }
            var symbol = Match(TokenType.Identifier).Identifier;

            Node node = g.Nodes.FirstOrDefault(n => n.NodeSymbol == symbol && n.RuleNodeID == ruleID);
            if (node != null)
            {
                return node;                
            }

            node = new Node(g.GetNewID().ToString())
            {
                NodeSymbol = symbol,
                RuleNodeID = ruleID
            };
            return node;
        }
    }
}