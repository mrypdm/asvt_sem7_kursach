using System;
using System.Collections.Generic;

namespace Assembler.Tokens;

internal class OPToken : IToken
{
    private int _machineCode;

    public OPToken(int machineCode)
    {
        _machineCode = machineCode;
    }

    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
    {
        return new List<string>() { Convert.ToString(_machineCode, 8).PadLeft(6, '0') };
    }
}