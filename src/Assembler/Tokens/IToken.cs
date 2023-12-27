using System.Collections.Generic;

namespace Assembler.Tokens;

/// <summary>
/// Token
/// </summary>
internal interface IToken
{
    /// <summary>
    /// Command line which generate this token
    /// </summary>
    public CommandLine CommandLine { get; }
    
    /// <summary>
    /// Translate to machine code
    /// </summary>
    public IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr);
}