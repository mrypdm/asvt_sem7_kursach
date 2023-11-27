using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal class RawToken : IToken
    {
        private string _machineCode;

        public RawToken(string machineCode)
        {
            _machineCode = machineCode;
        }

        public List<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
        {
            return new List<string>() { _machineCode };
        }
    }
}
