using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Instruction
    {
        public int OP;
        public int R;
        public int L;
        public int M;

        public Instruction()
        {
            OP = 0;
            R = 0;
            L = 0;
            M = 0;
        }
    }
}
