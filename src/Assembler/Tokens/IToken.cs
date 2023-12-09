using System.Collections.Generic;

namespace Assembler.Tokens;

internal interface IToken
{
    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr);
}