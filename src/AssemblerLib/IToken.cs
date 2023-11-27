using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal interface IToken
    {
        public List<string> Translate(Dictionary<string, int> marksDict, int currentAddr);
    }
}
