using System;
using System.Collections.Generic;

namespace Assembler.Tokens;

internal class ShiftOperationToken : IToken
{
    protected readonly int _machineCode;
    protected readonly string _mark;
    protected readonly CommandLine _originCmdLine;
    protected readonly int _shiftMask;

    public ShiftOperationToken(CommandLine commandLine, int machineCode, string mark, int shiftMask,
        CommandLine originCmdLine)
    {
        CommandLine = commandLine;
        _machineCode = machineCode;
        _mark = mark;
        _originCmdLine = originCmdLine;
        // Example: br - 0b1111_1111
        // sob - 0b111_111
        _shiftMask = shiftMask;
    }

    public CommandLine CommandLine { get; }

    public virtual IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
    {
        int delta = 0;
        if (marksDict.TryGetValue(_mark, out var markAddress))
        {
            delta = markAddress - currentAddr;
        }
        else
        {
            throw new Exception($"The mark ({_mark}) is not determined.");
        }

        // Distance restrictions for the mark
        if (delta > _shiftMask)
        {
            throw new Exception($"The distance to the mark ({_mark}) is too large. {delta}");
        }

        var shiftValue = (delta / 2 - 1) & _shiftMask;

        return new List<string>
            { Convert.ToString(_machineCode | shiftValue, 8).PadLeft(6, '0') + $";{_originCmdLine.LineText}" };
    }
}