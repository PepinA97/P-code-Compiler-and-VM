using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Token
    {
        public Enums.TokenType id;
        public string lexeme;
        public int lineNum;

        public Token()
        {
            id = Enums.TokenType.nulsym;
            lexeme = "";
            lineNum = 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(id);

            if (lexeme != "")
                sb.Append(" " + lexeme);
            
            return sb.ToString();
        }
    }
}
