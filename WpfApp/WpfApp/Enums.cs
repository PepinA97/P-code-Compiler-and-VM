using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Enums
    {
        public enum OpCode
        {
            LIT = 1, RTN = 2, LOD = 3, STO = 4, CAL = 5, INC = 6, JMP = 7, JPC = 8,

            // SIO Opcodes
            SIO_WRITE = 9, SIO_READ = 10, SIO_HALT = 11,

            // stuff Opcodes
            NEG = 12, ADD = 13, SUB = 14, MUL = 15, DIV = 16, ODD = 17, MOD = 18,
            EQL = 19, NEQ = 20, LSS = 21, LEQ = 22, GTR = 23, GEQ = 24
        }

        public enum TokenType
        {
            nulsym = 1,         identsym = 2,   numbersym = 3,

            // Special symbols
            plussym = 4,        minussym = 5,
            multsym = 6,        slashsym = 7,   oddsym = 8,         eqsym = 9,      neqsym = 10,
            lessym = 11,        leqsym = 12,    gtrsym = 13,        geqsym = 14,    lparentsym = 15,
            rparentsym = 16,    commasym = 17,  semicolonsym = 18,  periodsym = 19, becomessym = 20,

            // Reserved words
            beginsym = 21,      endsym = 22,    ifsym = 23,         thensym = 24,   whilesym = 25,
            dosym = 26,         callsym = 27,   constsym = 28,      varsym = 29,    procsym = 30,
            writesym = 31,      readsym = 32,   elsesym = 33
        }

        public enum LexerError
        {
            NONE, NONLETTER_VAR_INITIAL, NAME_TOO_LONG, NUM_TOO_LONG, INV_SYM, COMMENT_NOT_CLOSED, NO_SOURCE_CODE
        }

        public enum GeneratorError
        {
            NONE,
            EQUAL_NOT_FOLLOWED_BY_NUM,          IDENT_NOT_FOLLOWED_BY_NUM,           RESERVED_NOT_FOLLOWED_BY_IDENT,
            SEMICOLON_OR_COMMA_MISSING,         SEMICOLON_MISSING, PERIOD_EXPECTED,  ASSIGNMENT_OP_EXPECTED,
            CALL_NOT_FOLLOWED_BYIDENT,          THEN_EXPECTED,                       SEMICOLON_OR_END_EXPECTED,
            DO_EXPECTED,                        RELATIONAL_OP_EXPECTED,              RIGHT_PARENTH_MISSING,
            MISSING_SYMBOL, IDENT_OUTOFSCOPE,   ASSIGNMENT_CONST_PROC_IMPOSSIBLE,    CALL_CONST_VAR_IMPOSSIBLE,
            WRITE_TO_PROC_IMPOSSIBLE,           READ_TO_CONST_PROC_IMPOSSIBLE
        }

        public enum NonTerminal
        {
            PROGRAM, BLOCK, CONST_DECLARATION, VAR_DECLARATION, PROC_DECLARATION,
            STATEMENT, CONDITION, REL_OP, EXPRESSION, TERM, FACTOR
        }

        public enum CharacterType
        {
            ALPHA,  DIGIT,  SPECIAL,  INVALID
        }
    }
}
