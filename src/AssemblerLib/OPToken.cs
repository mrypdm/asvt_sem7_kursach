using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal class OPToken : IToken
    {
        private string _machineCode;

        public OPToken(string machineCode)
        {
            _machineCode = machineCode;
        }

        public List<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
        {
            return new List<string>() { _machineCode };
        }
    }
}
