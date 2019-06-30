using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Constants
    {

        public const int MaxIdentSize       = 16;
        public const int MaxNumberSize      = 16;
        public const int ReservedWordsOffset = 21;

        public static readonly string[] ReservedWords = {
            "begin",      "end",    "if",         "then",   "while",
            "do",         "call",   "const",      "var",    "procedure",
            "write",      "read",   "else"
        };

        public static readonly string[] LexerErrors = {
            "No error.",
            "Number starts with letter.",
            "Name too long.",
            "Number too long.",
            "Invalid symbol.",
            "Comment not closed.",
            "No source code."
        };

        public static readonly string[] GeneratorErrors = {
            "No error.",
            "'=' must be followed by a number.",
            "Identifier must be followed by '='.",
            "'const', 'var', 'procedure', 'read', 'write' must be followed by identifier.",
            "Semicolon or comma missing.",
            "Semicolon missing.",
            "Period expected.",
            "Assignment operator expected.",
            "'call' must be followed by an identifier.",
            "'then' expected.",
            "Semicolon or 'end' expected.",
            "'do' expected.",
            "Relational operator expected.",
            "Right parenthesis missing.",
            "The preceding factor cannot begin with this symbol.",
            "Identifier is undeclared or out of scope.",
            "Assignment to constant or procedure is not allowed.",
            "Call of a constant or variable is not allowed.",
            "Write of a procedure is not allowed.",
            "Read to a constant or prodecure is not allowed."
        };
    }
}
