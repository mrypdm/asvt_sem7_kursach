using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal class OPShiftToken : IToken
    {
        // The last character of the _machineCode is bit, not a 8-number!
        private string _machineCode;
        private string _mark;

        public OPShiftToken(string machineCode, string mark)
        {
            _machineCode = machineCode;
            _mark = mark;
        }

        public List<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
        {
            var delta = marksDict[_mark] - currentAddr;
            // 3 77 oct = 255 dec
            if (delta > 255)
            {
                throw new Exception($"The distance to the mark ({_mark}) is too large. {delta}");
            }

            var shiftDec = delta / 2 - 1;
            if (_machineCode.EndsWith('1'))
            {
                shiftDec += 256;
            }
            var shiftOct = Convert.ToString(shiftDec, 8);

            return new List<string>() { _machineCode.Substring(0, _machineCode.Length-1) + shiftOct };
        }
    }
}
