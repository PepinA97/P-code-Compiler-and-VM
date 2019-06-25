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

        public Token()
        {
            id = Enums.TokenType.nulsym;
            lexeme = "";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Constants.SymbolNames[(int)id]);

            if (lexeme != "")
                sb.Append(" " + lexeme);
            
            return sb.ToString();
        }
    }
}
