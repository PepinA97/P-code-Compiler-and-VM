using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Lexer
    {
        public Enums.LexerError error;

        TokenList tokenList;
        string code;
        char currentSymbol;
        int charIndex;
        int lineNum;

        public Lexer()
        {
            error = Enums.LexerError.NONE;

            tokenList = new TokenList();
            code = "";
            currentSymbol = '\0';
            charIndex = 0;
            lineNum = 1;
        }

        public TokenList Run(string inputCode)
        {
            if(inputCode == "")
            {
                error = Enums.LexerError.NO_SOURCE_CODE;
                return null;
            }

            code = inputCode + '\0';
            currentSymbol = code[0];

            while (true)
            {
                // Terminate loop is an error has been found
                if (HasError())
                    break;

                // Remove whitespace (also increments line number)
                RemoveWhitespace();

                // Terminate loop if end of string is reached
                if (currentSymbol == '\0')
                    break;

                // Handle symbol based on its type
                switch (GetSymbolType(currentSymbol))
                {
                    case (int)Enums.CharacterType.ALPHA:
                        HandleAlpha();
                        break;
                    case (int)Enums.CharacterType.DIGIT:
                        HandleDigit();
                        break;
                    case (int)Enums.CharacterType.SPECIAL:
                        HandleSpecial();
                        break;
                    case (int)Enums.CharacterType.INVALID:
                        error = Enums.LexerError.INV_SYM;
                        break;
                }
            }

            return tokenList;
        }

        public bool HasError()
        {
            return (error != Enums.LexerError.NONE ? true : false);
        }

        public string GetError()
        {
            return ("ERROR (" + (int)error + ") : " + Constants.LexerErrors[(int)error]);
        }
        
        private Enums.TokenType CheckReservedWords(string lexeme)
        {
            for(int i = 0; i < Constants.ReservedWords.Length; i++)
            {
                if(Constants.ReservedWords[i] == lexeme)
                {
                    return (Enums.TokenType)(i + Constants.ReservedWordsOffset);
                }
            }

            return Enums.TokenType.identsym;
        }

        private void RemoveWhitespace()
        {
            while (currentSymbol == ' ' || currentSymbol == '\n')
            {
                // Increment line number if a newline char is found
                if (currentSymbol == '\n')
                    lineNum++;

                NextSymbol();
            }
        }

        private void NextSymbol()
        {
            currentSymbol = code[++charIndex];
        }

        private int GetSymbolType(char c)
        {
            if (Char.IsLetter(c))
                return (int)Enums.CharacterType.ALPHA;
            else if (Char.IsDigit(c))
                return (int)Enums.CharacterType.DIGIT;
            else if (IsSpecialSymbol(c))
                return (int)Enums.CharacterType.SPECIAL;
            return (int)Enums.CharacterType.INVALID;
        }

        private bool IsSpecialSymbol(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' ||
                   c == '(' || c == ')' || c == '=' || c == ',' ||
                   c == '.' || c == '<' || c == '>' || c == ';' ||
                   c == ':';
        }

        private void HandleAlpha()
        {
            int numAlphanumerics = 0;
            StringBuilder sb = new StringBuilder();
            bool continueLooping = true;

            while (continueLooping)
            {
                switch (GetSymbolType(currentSymbol))
                {
                    case (int)Enums.CharacterType.ALPHA:
                    case (int)Enums.CharacterType.DIGIT:
                        if(numAlphanumerics < Constants.MaxIdentSize)
                        {
                            sb.Append(currentSymbol);
                            numAlphanumerics++;

                            NextSymbol();
                        }
                        else
                        {
                            error = Enums.LexerError.NAME_TOO_LONG;
                            return;
                        }
                        break;
                    case (int)Enums.CharacterType.SPECIAL:
                    case (int)Enums.CharacterType.INVALID:
                        continueLooping = false;
                        break;
                }
            }

            Token newToken = new Token();

            newToken.id = CheckReservedWords(sb.ToString());
            if (newToken.id == Enums.TokenType.identsym)
                newToken.lexeme = sb.ToString();
            newToken.lineNum = lineNum;

            tokenList.Add(newToken);
        }

        private void HandleDigit()
        {
            int numDigits = 0;
            StringBuilder sb = new StringBuilder();
            bool continueLooping = true;

            while (continueLooping)
            {
                switch (GetSymbolType(currentSymbol))
                {
                    case (int)Enums.CharacterType.ALPHA:
                        error = Enums.LexerError.NONLETTER_VAR_INITIAL;
                        // error
                        return;
                    case (int)Enums.CharacterType.DIGIT:
                        if (numDigits < Constants.MaxNumberSize)
                        {
                            sb.Append(currentSymbol);
                            numDigits++;

                            NextSymbol();
                        }
                        else
                        {
                            error = Enums.LexerError.NUM_TOO_LONG;
                            return;
                        }
                        break;
                    case (int)Enums.CharacterType.SPECIAL:
                    case (int)Enums.CharacterType.INVALID:
                        continueLooping = false;
                        break;
                }
            }

            Token newToken = new Token
            {
                lexeme = sb.ToString(),
                id = Enums.TokenType.numbersym,
                lineNum = lineNum
            };

            tokenList.Add(newToken);
        }

        private void HandleSpecial()
        {
            Enums.TokenType id = Enums.TokenType.nulsym;

            char lookaheadSymbol = code[charIndex + 1];
            if((currentSymbol == '/') && (lookaheadSymbol == '*'))
            {
                while (true)
                {
                    NextSymbol();

                    if (currentSymbol == '\0')
                    {
                        error = Enums.LexerError.COMMENT_NOT_CLOSED;
                        return;
                    }

                    if (currentSymbol == '\n')
                        lineNum++;

                    lookaheadSymbol = code[charIndex + 1];

                    // Stop ignoring symbols when we reach the end of comment symbols */
                    if((currentSymbol == '*') && (lookaheadSymbol == '/'))
                    {
                        break;
                    }
                }

                // Skip the end of comment symbols */
                NextSymbol();
                NextSymbol();

                return;
            }
            else if((currentSymbol == '<') && (lookaheadSymbol == '>'))
            {
                id = Enums.TokenType.neqsym;
                NextSymbol();
            }
            else if ((currentSymbol == '<') && (lookaheadSymbol == '='))
            {
                id = Enums.TokenType.leqsym;
                NextSymbol();
            }
            else if ((currentSymbol == '>') && (lookaheadSymbol == '='))
            {
                id = Enums.TokenType.geqsym;
                NextSymbol();
            }
            else if ((currentSymbol == ':') && (lookaheadSymbol == '='))
            {
                id = Enums.TokenType.becomessym;
                NextSymbol();
            }
            else
            {
                switch (currentSymbol)
                {
                    case '+':
                        id = Enums.TokenType.plussym;
                        break;
                    case '-':
                        id = Enums.TokenType.minussym;
                        break;
                    case '*':
                        id = Enums.TokenType.multsym;
                        break;
                    case '/':
                        id = Enums.TokenType.slashsym;
                        break;
                    case '(':
                        id = Enums.TokenType.lparentsym;
                        break;
                    case ')':
                        id = Enums.TokenType.rparentsym;
                        break;
                    case '<':
                        id = Enums.TokenType.lessym;
                        break;
                    case '>':
                        id = Enums.TokenType.gtrsym;
                        break;
                    case '.':
                        id = Enums.TokenType.periodsym;
                        break;
                    case ';':
                        id = Enums.TokenType.semicolonsym;
                        break;
                    case ',':
                        id = Enums.TokenType.commasym;
                        break;
                    case '=':
                        id = Enums.TokenType.eqsym;
                        break;
                }
            }

            NextSymbol();

            Token newToken = new Token
            {
                id = id,
                lineNum = lineNum
            };

            tokenList.Add(newToken);
        }
    }
}
