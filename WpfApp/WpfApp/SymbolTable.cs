using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class SymbolTable
    {
        ArrayList symbols;

        public SymbolTable()
        {
            symbols = null;
        }

        public void Add(Symbol symbol)
        {
            symbols.Add(symbol);
        }

        public ArrayList GetSymbols()
        {
            return symbols;
        }
    }
}
