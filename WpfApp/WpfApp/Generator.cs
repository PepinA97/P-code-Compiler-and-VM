using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// pass in token list

namespace WpfApp
{
    class Generator
    {
        int error;

        public Generator()
        {

        }

        public bool HasError()
        {
            return (this.error != 0 ? true : false);
        }
    }
}
