﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return ("ERROR (" + tokenList.GetCurrentToken().lineNum + " " + tokenList.GetCurrentToken().lexeme + ") : " + Constants.GeneratorErrors[(int)error]);
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
            if (tokenList.GetCurrentTokenType() == Enums.TokenType.identsym)
            {
                Symbol symbol = Symbol.FindSymbol(symbolTable, currentScope, tokenList.GetCurrentToken().lexeme);
                if(symbol == null)
                {
                    error = Enums.GeneratorError.IDENT_OUTOFSCOPE;
                    return;
                }

                tokenList.NextToken();

                if(symbol.type != Enums.TokenType.varsym)
                {
                    error = Enums.GeneratorError.ASSIGNMENT_CONST_PROC_IMPOSSIBLE;
                    return;
                }

                if (tokenList.GetCurrentTokenType() != Enums.TokenType.becomessym)
                {
                    error = Enums.GeneratorError.ASSIGNMENT_OP_EXPECTED;
                    return;
                }

                tokenList.NextToken();

                Expression();
                if (error != Enums.GeneratorError.NONE) return;

                Emit((int)Enums.OpCode.STO, currentRegister - 1, currentLevel - symbol.level, symbol.address);

                currentRegister--;
            }
            else if (tokenList.GetCurrentTokenType() == Enums.TokenType.callsym)
            {
                tokenList.NextToken();

                if (tokenList.GetCurrentTokenType() != Enums.TokenType.identsym)
                {
                    error = Enums.GeneratorError.CALL_NOT_FOLLOWED_BYIDENT;
                    return;
                }

                Symbol symbol = Symbol.FindSymbol(symbolTable, currentScope, tokenList.GetCurrentToken().lexeme);

                if(symbol == null)
                {
                    error = Enums.GeneratorError.IDENT_OUTOFSCOPE;
                    return;
                }

                if(symbol.type != Enums.TokenType.procsym)
                {
                    error = Enums.GeneratorError.CALL_CONST_VAR_IMPOSSIBLE;
                    return;
                }

                Emit((int)Enums.OpCode.CAL, 0, currentLevel - symbol.level, symbol.address);

                tokenList.NextToken();
            }
            else if (tokenList.GetCurrentTokenType() == Enums.TokenType.beginsym)
            {
                do
                {
                    tokenList.NextToken();

                    Statement();
                    if (error != Enums.GeneratorError.NONE) return;
                } while (tokenList.GetCurrentTokenType() == Enums.TokenType.semicolonsym);

                if(tokenList.GetCurrentTokenType() != Enums.TokenType.endsym)
                {
                    error = Enums.GeneratorError.SEMICOLON_OR_END_EXPECTED;
                    return;
                }

                tokenList.NextToken();
            }
            else if (tokenList.GetCurrentTokenType() == Enums.TokenType.ifsym)
            {
                tokenList.NextToken();

                Condition();
                if (error != Enums.GeneratorError.NONE) return;

                if (tokenList.GetCurrentTokenType() == Enums.TokenType.thensym)
                {
                    error = Enums.GeneratorError.THEN_EXPECTED;
                    return;
                }

                tokenList.NextToken();

                int cTemp = nextCodeIndex;
                Emit((int)Enums.OpCode.JPC, currentRegister - 1, 0, 0);

                Statement();
                if (error != Enums.GeneratorError.NONE) return;

                int cTempElse = nextCodeIndex;
                Emit((int)Enums.OpCode.JMP, 0, 0, 0);

                instructions[cTemp].M = nextCodeIndex;

                if (tokenList.GetCurrentTokenType() == Enums.TokenType.elsesym)
                {
                    tokenList.NextToken();

                    instructions[cTemp].M = nextCodeIndex;
                    cTemp = nextCodeIndex;

                    Statement();
                    if (error != Enums.GeneratorError.NONE) return;
                }

                instructions[cTempElse].M = nextCodeIndex;
            }
            else if (tokenList.GetCurrentTokenType() == Enums.TokenType.whilesym)
            {
                tokenList.NextToken();

                int cTemp1 = nextCodeIndex;

                Condition();
                if (error != Enums.GeneratorError.NONE) return;

                int cTemp2 = nextCodeIndex;

                Emit((int)Enums.OpCode.JPC, currentRegister - 1, 0, 0);

                if (tokenList.GetCurrentTokenType() != Enums.TokenType.dosym)
                {
                    error = Enums.GeneratorError.DO_EXPECTED;
                    return;
                }

                tokenList.NextToken();

                Statement();
                if (error != Enums.GeneratorError.NONE) return;

                Emit((int)Enums.OpCode.JMP, 0, 0, cTemp1);

                instructions[cTemp2].M = nextCodeIndex;
            }
            else if (tokenList.GetCurrentTokenType() == Enums.TokenType.readsym)
            {
                tokenList.NextToken();

                Emit((int)Enums.OpCode.SIO_READ, currentRegister, 0, 1);

                if (tokenList.GetCurrentTokenType() == Enums.TokenType.identsym)
                {
                    error = Enums.GeneratorError.RESERVED_NOT_FOLLOWED_BY_IDENT;
                    return;
                }

                Symbol symbol = Symbol.FindSymbol(symbolTable, currentScope, tokenList.GetCurrentToken().lexeme);

                if(symbol == null)
                {
                    error = Enums.GeneratorError.IDENT_OUTOFSCOPE;
                    return;
                }
                if(symbol.type == Enums.TokenType.varsym)
                {
                    error = Enums.GeneratorError.READ_TO_CONST_PROC_IMPOSSIBLE;
                    return;
                }

                Emit((int)Enums.OpCode.STO, currentRegister, currentLevel - symbol.level, symbol.address);

                tokenList.NextToken();
            }
            else if (tokenList.GetCurrentTokenType() == Enums.TokenType.writesym)
            {
                tokenList.NextToken();
                
                if (tokenList.GetCurrentTokenType() != Enums.TokenType.identsym)
                {
                    error = Enums.GeneratorError.RESERVED_NOT_FOLLOWED_BY_IDENT;
                    return;
                }

                Symbol symbol = Symbol.FindSymbol(symbolTable, currentScope, tokenList.GetCurrentToken().lexeme);

                if (symbol == null)
                {
                    error = Enums.GeneratorError.IDENT_OUTOFSCOPE;
                    return;
                }
                if (symbol.type == Enums.TokenType.procsym)
                {
                    error = Enums.GeneratorError.WRITE_TO_PROC_IMPOSSIBLE;
                    return;
                }


                if (symbol.type == Enums.TokenType.constsym)
                {
                    Emit((int)Enums.OpCode.LIT, currentRegister - 1, 0, symbol.value);
                }
                else if (symbol.type == Enums.TokenType.varsym)
                {
                    Emit((int)Enums.OpCode.LOD, currentRegister - 1, 0, 2);
                }

                Emit((int)Enums.OpCode.SIO_WRITE, currentRegister - 1, 0, 2);

                tokenList.NextToken();
            }
        }

        private void Condition()
        {
            if (tokenList.GetCurrentTokenType() == Enums.TokenType.oddsym)
            {
                tokenList.NextToken();

                Expression();
                if (error != Enums.GeneratorError.NONE) return;
            }
            else
            {
                Expression();
                if (error != Enums.GeneratorError.NONE) return;

                Enums.TokenType currentTokenType = tokenList.GetCurrentTokenType();

                tokenList.NextToken();

                Expression();
                if (error != Enums.GeneratorError.NONE) return;

                if (tokenList.GetCurrentTokenType() == Enums.TokenType.eqsym)
                    Emit((int)Enums.OpCode.EQL, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                else if (tokenList.GetCurrentTokenType() == Enums.TokenType.neqsym)
                    Emit((int)Enums.OpCode.NEQ, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                else if (tokenList.GetCurrentTokenType() == Enums.TokenType.lessym)
                    Emit((int)Enums.OpCode.LSS, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                else if (tokenList.GetCurrentTokenType() == Enums.TokenType.leqsym)
                    Emit((int)Enums.OpCode.LEQ, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                else if (tokenList.GetCurrentTokenType() == Enums.TokenType.gtrsym)
                    Emit((int)Enums.OpCode.GTR, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                else if (tokenList.GetCurrentTokenType() == Enums.TokenType.geqsym)
                    Emit((int)Enums.OpCode.GEQ, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                else
                {
                    error = Enums.GeneratorError.RELATIONAL_OP_EXPECTED;
                    return;
                }

                currentRegister--;
            }
        }

        private void Expression()
        {
            if ((tokenList.GetCurrentTokenType() == Enums.TokenType.plussym) || (tokenList.GetCurrentTokenType() == Enums.TokenType.minussym))
            {
                Enums.TokenType op = tokenList.GetCurrentTokenType();

                tokenList.NextToken();

                Term();
                if (error != Enums.GeneratorError.NONE) return;

                if(op == Enums.TokenType.minussym)
                {
                    Emit((int)Enums.OpCode.NEG, currentRegister - 1, currentRegister - 1, 0);
                }
            }
            else
            {
                Term();
                if (error != Enums.GeneratorError.NONE) return;
            }

            while ((tokenList.GetCurrentTokenType() == Enums.TokenType.plussym) || (tokenList.GetCurrentTokenType() == Enums.TokenType.minussym))
            {
                Enums.TokenType op = tokenList.GetCurrentTokenType();

                tokenList.NextToken();

                Term();
                if (error != Enums.GeneratorError.NONE) return;

                if (op == Enums.TokenType.plussym)
                {
                    Emit((int)Enums.OpCode.ADD, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                }
                else
                {
                    Emit((int)Enums.OpCode.SUB, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                }

                currentRegister--;
            }
        }

        private void Term()
        {
            Factor();
            if (error != Enums.GeneratorError.NONE) return;

            while ((tokenList.GetCurrentTokenType() == Enums.TokenType.multsym) || (tokenList.GetCurrentTokenType() == Enums.TokenType.slashsym))
            {
                Enums.TokenType op = tokenList.GetCurrentTokenType();

                tokenList.NextToken();

                Factor();
                if (error != Enums.GeneratorError.NONE) return;

                if (op == Enums.TokenType.multsym)
                {
                    Emit((int)Enums.OpCode.MUL, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                }
                else
                {
                    Emit((int)Enums.OpCode.DIV, currentRegister - 2, currentRegister - 2, currentRegister - 1);
                }

                currentRegister--;
            }
        }

        private void Factor()
        {
            if (tokenList.GetCurrentTokenType() == Enums.TokenType.identsym)
            {

                Symbol symbol = Symbol.FindSymbol(symbolTable, currentScope, tokenList.GetCurrentToken().lexeme);

                if (symbol == null)
                {
                    error = Enums.GeneratorError.IDENT_OUTOFSCOPE;
                    return;
                }

                if (symbol.type == Enums.TokenType.constsym)
                {
                    Emit((int)Enums.OpCode.LIT, currentRegister, 0, symbol.value);
                }
                else if (symbol.type == Enums.TokenType.varsym)
                {
                    Emit((int)Enums.OpCode.LOD, currentRegister, currentLevel - symbol.level, symbol.address);
                }

                currentRegister++;

                tokenList.NextToken();

                return;
            }
            else if (tokenList.GetCurrentTokenType() == Enums.TokenType.numbersym)
            {
                Emit((int)Enums.OpCode.LIT, currentRegister, 0, int.Parse(tokenList.GetCurrentToken().lexeme));

                currentRegister++;

                tokenList.NextToken();

                return;
            }
            else if (tokenList.GetCurrentTokenType() == Enums.TokenType.lparentsym)
            {
                tokenList.NextToken();

                Expression();
                if (error != Enums.GeneratorError.NONE) return;

                if (tokenList.GetCurrentTokenType() != Enums.TokenType.rparentsym)
                {
                    error = Enums.GeneratorError.RIGHT_PARENTH_MISSING;
                    return;
                }

                tokenList.NextToken();
            }
            else
            {
                error = Enums.GeneratorError.MISSING_SYMBOL;
                return;
            }
        }
    }
}
