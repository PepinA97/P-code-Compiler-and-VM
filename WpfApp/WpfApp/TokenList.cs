using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class TokenList
    {
        List<Token> tokens;
        int iterator;

        public TokenList()
        {
            tokens = new List<Token>();
            iterator = 0;
        }

        public void Add(Token token)
        {
            tokens.Add(token);
        }

        public void NextToken()
        {
            iterator++;
        }

        public Token GetCurrentToken()
        {
            return (Token)tokens[iterator];
        }

        public Enums.TokenType GetCurrentTokenType()
        {
            return GetCurrentToken().id;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach(Token token in tokens)
            {
                sb.Append(token.ToString() + " ");
            }

            return sb.ToString();
        }
    }
}
