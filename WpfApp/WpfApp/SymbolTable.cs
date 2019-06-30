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
        List<Symbol> symbols;

        public SymbolTable()
        {
            symbols = new List<Symbol>();
        }

        public void Add(Symbol symbol)
        {
            symbols.Add(symbol);
        }

        public List<Symbol> GetSymbols()
        {
            return symbols;
        }
    }
}
