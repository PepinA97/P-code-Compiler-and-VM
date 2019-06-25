using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Symbol
    {
        Enums.TokenType type;
        String name;
        int value;
        int level;
        int address;
        Symbol scope;

        public Symbol()
        {

        }
    }
}
