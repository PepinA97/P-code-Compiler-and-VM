using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Constants
    {

        public const int MaxIdentSize   = 16;
        public const int MaxNumberSize  = 16;

        public static readonly string[] SymbolNames = {
            "",              "nulsym",         "identsym",   "numbersym",

            // Special symbols
            "plussym",       "minussym",
            "multsym",       "slashsym",  "oddsym",        "eqsym",     "neqsym",
            "lessym",        "leqsym",    "gtrsym",        "geqsym",    "lparentsym",
            "rparentsym",    "commasym",  "semicolonsym",  "periodsym", "becomessym",

            // Reserved words
            "beginsym",      "endsym",    "ifsym",         "thensym",   "whilesym",
            "dosym",         "callsym",   "constsym",      "varsym",    "procsym",
            "writesym",      "readsym",   "elsesym"
        };

        public static readonly string[] LexerErrors = {

        };
    }
}
