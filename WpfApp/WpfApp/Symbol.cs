using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Symbol
    {
        public Enums.TokenType type;

        public Symbol scope;

        public String name;

        public int value;
        public int level;
        public int address;

        public Symbol()
        {
            type = Enums.TokenType.nulsym;

            scope = null;

            name = "";

            value = 0;
            level = 0;
            address = 0;
        }

        public Symbol FindSymbol(SymbolTable symbolTable, Symbol scope, string symbolName)
        {
            while (true)
            {
                foreach (Symbol symbol in symbolTable.GetSymbols())
                {
                    if((symbol.scope == scope) && (symbol.name == symbolName))
                    {
                        return symbol;
                    }
                }
                if (scope == null)
                {
                    return null;
                }
                else
                {
                    scope = scope.scope;
                }
            }
        }
    }
}
