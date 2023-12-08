using System;
using System.Collections.Generic;

namespace Assembler.Tokens;

internal class OPShiftToken : IToken
{
    private int _machineCode;
    private string _mark;

    public OPShiftToken(int machineCode, string mark)
    {
        _machineCode = machineCode;
        _mark = mark;
    }

    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
    {
        int delta;
        if (marksDict.TryGetValue(_mark, out delta))
        {
            delta = delta - currentAddr;
        }
        else
        {
            throw new Exception($"The mark ({_mark}) is not determined.");
        }

        // 3 77 oct = 255 dec
        if (delta > 255)
        {
            throw new Exception($"The distance to the mark ({_mark}) is too large. {delta}");
        }

        var shiftValue = delta / 2 - 1;

        return new List<string>() { Convert.ToString(_machineCode | shiftValue, 8).PadLeft(6, '0') };
    }
}