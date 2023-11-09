using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssemblerLib;

internal class Parser
{
    private const string RegexPattern = @"^\s*([^\s,:]+:\s*)?(\S+)?\s*([^\s,]+\s*,?\s*){0,}$";
    private readonly char[] BadSymbols = { ' ', '\t', ',', ':' };

    private readonly Regex _regexMask;
    
    public Parser()
    {
        _regexMask = new Regex(RegexPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    public async Task<List<CommandLine>> Parse(string filePath)
    {
        var res = new List<CommandLine>();
        string line;

        using var reader = new StreamReader(filePath);

        while ((line = await reader.ReadLineAsync()) != null)
        {
            var match = _regexMask.Match(line);

            var mark = match.Groups[1].Value.Trim().Trim(BadSymbols).ToLower();
            var instruction = match.Groups[2].Value.Trim(BadSymbols).ToLower();
            var arguments = match.Groups[3].Captures.Select(c => c.Value.Trim(BadSymbols).ToLower());

            var command = new CommandLine(mark, instruction, arguments);

            res.Add(command);
        }

        return res;
    }
}
