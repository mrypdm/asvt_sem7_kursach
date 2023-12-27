using System;
using System.Collections.Generic;

namespace Assembler.Tokens;

internal class MarkRelatedToken : IToken
{
    private readonly string _mark;

    public MarkRelatedToken(CommandLine commandLine, string mark)
    {
        CommandLine = commandLine;
        _mark = mark;
    }

    public CommandLine CommandLine { get; }

    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
    {
        if (!marksDict.ContainsKey(_mark))
        {
            throw new Exception($"The mark ({_mark}) is not determined.");
        }

        var delta = marksDict[_mark] - currentAddr;
        // 17 77 77 oct = 65535 dec
        if (Math.Abs(delta) > 65535)
        {
            throw new Exception($"The distance to the mark ({_mark}) is too large. {delta}");
        }

        var relDist = Convert.ToString(Convert.ToInt16(delta - 2), 8).PadLeft(6, '0');
        return new[] { relDist };
    }
}