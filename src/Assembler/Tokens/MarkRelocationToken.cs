using System;
using System.Collections.Generic;

namespace Assembler.Tokens;

internal class MarkRelocationToken : IToken
{
    private readonly string _mark;

    private readonly int _addValue;

    // true -> +; false -> -
    private readonly bool _opSign;

    public MarkRelocationToken(string mark, int addValue, bool opSign)
    {
        _mark = mark;
        _addValue = addValue;
        _opSign = opSign;
    }

    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
    {
        if (!marksDict.ContainsKey(_mark))
        {
            throw new Exception($"The mark ({_mark}) is not determined.");
        }

        var word = Convert.ToString(marksDict[_mark] + (_opSign ? 1 : -1) * _addValue, 8).PadLeft(6, '0') + "'";
        return new[] { word };
    }
}