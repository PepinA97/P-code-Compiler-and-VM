using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// pass in input string

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
            error = Enums.LexerError.None;

            tokenList = new TokenList();
            code = "";
            currentSymbol = '\0';
            charIndex = 0;
            lineNum = 0;
        }

        public TokenList Run(string inputCode)
        {
            code = inputCode + '\0';
            currentSymbol = code[0];

            while (true)
            {
                // Remove whitespace (also increments line number)
                RemoveWhitespace();
                
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
                        // make error "invalid symbol"
                        return null;
                }

                // Terminate loop is an error has been found
                if (HasError())
                    break;

                // Terminate loop if end of string is reached
                if (currentSymbol == '\0')
                    break;

                NextSymbol();
            }

            return tokenList;
        }

        public bool HasError()
        {
            return (error != Enums.LexerError.None ? true : false);
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
                    case 0:
                    case 1:
                        if(numAlphanumerics < Constants.MaxIdentSize)
                        {
                            sb.Append(currentSymbol);
                            numAlphanumerics++;

                            NextSymbol();
                        }
                        else
                        {
                            // error
                            return;
                        }
                        break;
                    case 2:
                    case 3:
                        continueLooping = false;
                        break;
                }
            }

            Token newToken = new Token();
            newToken.lexeme = sb.ToString();
            newToken.id = Enums.TokenType.identsym;

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
                    case 0:
                        error = Enums.LexerError.Bad;
                        // error
                        return;
                    case 1:
                        if (numDigits < Constants.MaxNumberSize)
                        {
                            sb.Append(currentSymbol);
                            numDigits++;
                            NextSymbol();
                        }
                        else
                        {

                            return;
                        }
                        break;
                    case 2:
                    case 3:
                        continueLooping = false;
                        break;
                }
            }

            Token newToken = new Token();
            newToken.lexeme = sb.ToString();
            newToken.id = Enums.TokenType.numbersym;
            
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
                        // error
                        return;
                    }

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
                id = id
            };

            tokenList.Add(newToken);
        }
    }
}
