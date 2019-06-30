using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// pass in token list

namespace WpfApp
{
    class Generator
    {
        Enums.GeneratorError error;

        SymbolTable symbolTable;
        Symbol currentScope;

        List<Instruction> instructions;
        TokenList tokenList;

        int currentLevel;
        int nextCodeIndex;
        int currentRegister;

        public Generator()
        {
            error = Enums.GeneratorError.NONE;

            symbolTable = null;
            currentScope = null;

            instructions = null;
            tokenList = null;

            currentLevel = 0;
            nextCodeIndex = 0;
            currentRegister = 0;
        }

        public List<Instruction> Run(TokenList inputTokenList)
        {
            tokenList = inputTokenList;

            symbolTable = new SymbolTable();

            instructions = new List<Instruction>();

            Program();

            return instructions;
        }

        public bool HasError()
        {
            return (error != Enums.GeneratorError.NONE ? true : false);
        }

        public string GetError()
        {
            return ("ERROR (" + (int)error + ") : " + Constants.GeneratorErrors[(int)error]);
        }

        private int Emit(int OP, int R, int L, int M)
        {
            Instruction instruction = new Instruction
            {
                OP = OP,
                R = R,
                L = L,
                M = M
            };

            instructions.Add(instruction);
            return nextCodeIndex++;
        }

        private void Program()
        {
            currentRegister++;

            Block();

            if (error != Enums.GeneratorError.NONE) return;
            
            if(tokenList.GetCurrentTokenType() == Enums.TokenType.periodsym)
            {
                tokenList.NextToken();

                Emit((int)Enums.OpCode.SIO_HALT, 0, 0, 3);
                
                return;
            }

            error = Enums.GeneratorError.PERIOD_EXPECTED;
            return;
        }


        private void Block()
        {
            currentLevel++;

            ConstantsDeclaration();
            if (error != Enums.GeneratorError.NONE) return;

            VariablesDeclaration();
            if (error != Enums.GeneratorError.NONE) return;

            int jumpIndex = Emit((int)Enums.OpCode.JMP, 0, 0, 0);

            ProceduresDeclaration(jumpIndex);
            if (error != Enums.GeneratorError.NONE) return;

            instructions[jumpIndex].M = nextCodeIndex;

            Statement();
            if (error != Enums.GeneratorError.NONE) return;

            currentLevel--;
        }

        private void ConstantsDeclaration()
        {
            if(tokenList.GetCurrentTokenType() == Enums.TokenType.constsym)
            {
                do
                {
                    tokenList.NextToken();

                    Symbol newSymbol = new Symbol();
                    newSymbol.type = Enums.TokenType.constsym;
                    newSymbol.level = currentLevel;

                    if (tokenList.GetCurrentTokenType() != Enums.TokenType.identsym)
                    {
                        error = Enums.GeneratorError.RESERVED_NOT_FOLLOWED_BY_IDENT;
                        return;
                    }

                    newSymbol.name = tokenList.GetCurrentToken().lexeme;
                    tokenList.NextToken();

                    if (tokenList.GetCurrentTokenType() != Enums.TokenType.eqsym)
                    {
                        error = Enums.GeneratorError.IDENT_NOT_FOLLOWED_BY_NUM;
                        return;
                    }

                    tokenList.NextToken();

                    if (tokenList.GetCurrentTokenType() != Enums.TokenType.numbersym)
                    {
                        error = Enums.GeneratorError.EQUAL_NOT_FOLLOWED_BY_NUM;
                        return;
                    }

                    newSymbol.value = int.Parse(tokenList.GetCurrentToken().lexeme);
                    newSymbol.scope = currentScope;

                    symbolTable.Add(newSymbol);

                    tokenList.NextToken();
                } while (tokenList.GetCurrentTokenType() == Enums.TokenType.commasym);

                if (tokenList.GetCurrentTokenType() != Enums.TokenType.semicolonsym)
                {
                    error = Enums.GeneratorError.SEMICOLON_MISSING;
                    return;
                }

                tokenList.NextToken();
            }
        }

        private void VariablesDeclaration()
        {
            if (tokenList.GetCurrentTokenType() == Enums.TokenType.varsym)
            {
                int numVariables = 1;
                do
                {
                    tokenList.NextToken();

                    Symbol newSymbol = new Symbol();
                    newSymbol.type = Enums.TokenType.varsym;
                    newSymbol.level = currentLevel;

                    if(currentLevel == 1)
                        newSymbol.address = numVariables;
                    else
                        newSymbol.address = numVariables + 4;
                    numVariables++;


                    if (tokenList.GetCurrentTokenType() != Enums.TokenType.identsym)
                    {
                        error = Enums.GeneratorError.RESERVED_NOT_FOLLOWED_BY_IDENT;
                        return;
                    }

                    newSymbol.name = tokenList.GetCurrentToken().lexeme;
                    newSymbol.scope = currentScope;

                    symbolTable.Add(newSymbol);

                    tokenList.NextToken();
                } while (tokenList.GetCurrentTokenType() == Enums.TokenType.commasym);

                Emit((int)Enums.OpCode.INC, 0, 0, numVariables);

                if (tokenList.GetCurrentTokenType() != Enums.TokenType.semicolonsym)
                {
                    error = Enums.GeneratorError.SEMICOLON_MISSING;
                    return;
                }

                tokenList.NextToken();
            }
        }

        private void ProceduresDeclaration(int jumpIndex)
        {
            while (tokenList.GetCurrentTokenType() == Enums.TokenType.procsym)
            {
                tokenList.NextToken();

                Symbol newSymbol = new Symbol();
                newSymbol.type = Enums.TokenType.procsym;
                newSymbol.level = currentLevel;
                newSymbol.address = jumpIndex + 1;
                
                if (tokenList.GetCurrentTokenType() != Enums.TokenType.identsym)
                {
                    error = Enums.GeneratorError.RESERVED_NOT_FOLLOWED_BY_IDENT;
                    return;
                }

                newSymbol.name = tokenList.GetCurrentToken().lexeme;
                newSymbol.scope = currentScope;

                symbolTable.Add(newSymbol);

                Symbol oldScope = currentScope;
                Symbol newScope = newSymbol;

                tokenList.NextToken();

                if (tokenList.GetCurrentTokenType() != Enums.TokenType.semicolonsym)
                {
                    error = Enums.GeneratorError.SEMICOLON_MISSING;
                    return;
                }

                tokenList.NextToken();

                currentScope = newScope;

                Emit((int)Enums.OpCode.INC, 0, 0, 0);
                
                Block();
                if (error != Enums.GeneratorError.NONE) return;

                currentScope = oldScope;

                jumpIndex = nextCodeIndex;

                Emit((int)Enums.OpCode.RTN, 0, 0, 0);

                if (tokenList.GetCurrentTokenType() != Enums.TokenType.semicolonsym)
                {
                    error = Enums.GeneratorError.SEMICOLON_OR_END_EXPECTED;
                    return;
                }

                tokenList.NextToken();
            } ;
        }

        private void Statement()
        {
        }

        private void Condition()
        {
        }

        private void Expression()
        {
        }

        private void Term()
        {
        }

        private void Factor()
        {
        }
    }
}
