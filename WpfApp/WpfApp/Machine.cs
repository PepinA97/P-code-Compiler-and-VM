using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Machine
    {
        private List<int> input;
        private List<int> output;

        private int basePointer;
        private int stackPointer;
        private int programCounter;
        private int instructionRegister;

        private int[] registerFile;
        private int[] stack;

        private bool shouldContinue;

        public Machine()
        {
            input = null;
            output = null;
            
            basePointer = 1;
            stackPointer = 0;
            programCounter = 0;
            instructionRegister = 0;

            registerFile = new int[Constants.RegisterFileSize];
            stack = new int[Constants.StackSize];

            shouldContinue = true;
        }
        
        public List<int> Run(List<Instruction> machineCode, List<int> input)
        {
            while (shouldContinue)
            {
                instructionRegister = programCounter;

                programCounter++;

                ExecuteInstruction(machineCode[instructionRegister]);
            }

            return output;
        }

        private void ExecuteInstruction(Instruction instruction)
        {
            switch ((Enums.OpCode)instruction.OP)
            {
                case Enums.OpCode.LIT:
                    break;
                case Enums.OpCode.RTN:
                    break;
                case Enums.OpCode.LOD:
                    break;
                case Enums.OpCode.STO:
                    break;
                case Enums.OpCode.CAL:
                    break;
                case Enums.OpCode.INC:
                    break;
                case Enums.OpCode.JMP:
                    break;
                case Enums.OpCode.JPC:
                    break;
                case Enums.OpCode.SIO_WRITE:
                    break;
                case Enums.OpCode.SIO_READ:
                    break;
                case Enums.OpCode.SIO_HALT:
                    break;
                case Enums.OpCode.NEG:
                    break;
                case Enums.OpCode.ADD:
                    break;
                case Enums.OpCode.SUB:
                    break;
                case Enums.OpCode.MUL:
                    break;
                case Enums.OpCode.DIV:
                    break;
                case Enums.OpCode.ODD:
                    break;
                case Enums.OpCode.MOD:
                    break;
                case Enums.OpCode.EQL:
                    break;
                case Enums.OpCode.NEQ:
                    break;
                case Enums.OpCode.LEQ:
                    break;
                case Enums.OpCode.LSS:
                    break;
                case Enums.OpCode.GEQ:
                    break;
                case Enums.OpCode.GTR:
                    break;
                default:
                    shouldContinue = false;
                    break;
            }
        }

        private int GetBasePointer()
        {
            return 0;
        }
    }
}
