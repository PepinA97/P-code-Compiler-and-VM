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
        ArrayList Tokens;
        int iterator;

        public TokenList()
        {
            Tokens = new ArrayList();
            iterator = 0;
        }

        public void Add(Token token)
        {
            Tokens.Add(token);
        }

        public void NextToken()
        {
            iterator++;
        }

        public Token GetCurrentToken()
        {
            return (Token)Tokens[iterator];
        }

        public int GetCurrentTokenType()
        {
            return GetCurrentToken().id;
        }

        ~TokenList()
        {

        }
    }
}
