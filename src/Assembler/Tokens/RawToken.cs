using System;
using System.Collections.Generic;

namespace Assembler.Tokens;

internal class RawToken : IToken
{
    private int _machineCode;

    public RawToken(int machineCode)
    {
        _machineCode = machineCode;
    }

    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
    {
        return new List<string>() { Convert.ToString(_machineCode, 8).PadLeft(6, '0') };
    }
}