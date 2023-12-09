using System;
using System.Collections.Generic;

namespace Assembler.Tokens;

internal class RawToken : IToken
{
    private readonly int _machineCode;

    public RawToken(int machineCode)
    {
        _machineCode = machineCode;
    }

    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
    {
        return new[] { Convert.ToString(_machineCode, 8).PadLeft(6, '0') };
    }
}