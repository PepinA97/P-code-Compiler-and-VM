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

        ArrayList instructions;
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

        public ArrayList Run(TokenList inputTokenList)
        {
            tokenList = inputTokenList;

            symbolTable = new SymbolTable();

            instructions = new ArrayList();

            error = Program();

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

        private Enums.GeneratorError Program()
        {
            currentRegister++;

            Enums.GeneratorError error = Block();

            if (error != Enums.GeneratorError.NONE)
                return error;
            
            if(tokenList.GetCurrentTokenType() == Enums.TokenType.periodsym)
            {
                tokenList.NextToken();

                Emit((int)Enums.OpCode.SIO_HALT, 0, 0, 3);

                return Enums.GeneratorError.NONE;
            }

            return Enums.GeneratorError.PERIOD_EXPECTED;
        }


        private Enums.GeneratorError Block()
        {
            return Enums.GeneratorError.NONE;
        }

        private Enums.GeneratorError ConstantsDeclaration()
        {
            return Enums.GeneratorError.NONE;
        }

        private Enums.GeneratorError VariablesDeclaration()
        {
            return Enums.GeneratorError.NONE;
        }

        private Enums.GeneratorError ProceduresDeclaration(int JumpIndex)
        {
            return Enums.GeneratorError.NONE;
        }

        private Enums.GeneratorError Statement()
        {
            return Enums.GeneratorError.NONE;
        }

        private Enums.GeneratorError Condition()
        {
            return Enums.GeneratorError.NONE;
        }

        private Enums.GeneratorError Expression()
        {
            return Enums.GeneratorError.NONE;
        }

        private Enums.GeneratorError Term()
        {
            return Enums.GeneratorError.NONE;
        }

        private Enums.GeneratorError Factor()
        {
            return Enums.GeneratorError.NONE;
        }
    }
}
