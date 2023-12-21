using System;
using System.Collections.Generic;

namespace Assembler.Tokens;

internal class OperationToken : IToken
{
    private readonly int _machineCode;
    private readonly CommandLine _originCmdLine;

    public OperationToken(int machineCode, CommandLine originCmdLine)
    {
        _machineCode = machineCode;
        _originCmdLine = originCmdLine;
    }

    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
    {
        return new[] { Convert.ToString(_machineCode, 8).PadLeft(6, '0') + $";{_originCmdLine.GetSymbol()}" };
    }
}
