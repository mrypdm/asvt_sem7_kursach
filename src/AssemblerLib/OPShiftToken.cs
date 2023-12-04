using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal class OPShiftToken : IToken
    {
        private int _machineCode;
        private string _mark;

        public OPShiftToken(int machineCode, string mark)
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

            var shiftValue = delta / 2 - 1;

            return new List<string>() { Convert.ToString(_machineCode | shiftValue, 8).PadLeft(6, '0') };
        }
    }
}
