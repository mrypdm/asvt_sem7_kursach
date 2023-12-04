using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal class RawToken : IToken
    {
        private int _machineCode;

        public RawToken(int machineCode)
        {
            _machineCode = machineCode;
        }

        public List<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
        {
            return new List<string>() { Convert.ToString(_machineCode, 8).PadLeft(6, '0') };
        }
    }
}
