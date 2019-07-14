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

        private bool shouldContinue;

        Machine()
        {
            input = null;
            output = null;



            shouldContinue = true;
        }

        public List<int> Run(List<int> input)
        {
            while (shouldContinue)
            {

            }

            return null;
        }
    }
}
