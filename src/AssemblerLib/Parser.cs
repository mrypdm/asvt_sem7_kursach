using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssemblerLib;

internal class Parser
{
    private const string _RegexPatternCommandLine = @"^\s*([^\s,:]+:\s*)?(\S+)?\s*([^\s,]+\s*,?\s*){0,}$";
    private const string _RegexPatternRemovingComment = @"^[^;.]+(?=;?)";
    private readonly char[] BadSymbols = { ' ', '\t', ',', ':' };

    private readonly Regex _regexMaskCommandLine;
    private readonly Regex _regexMaskRemovingComment;

    public Parser()
    {
        _regexMaskCommandLine = new Regex(_RegexPatternCommandLine, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskRemovingComment = new Regex(_RegexPatternRemovingComment, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    public async Task<List<CommandLine>> Parse(string filePath)
    {
        var res = new List<CommandLine>();
        string line;

        using var reader = new StreamReader(filePath);

        while ((line = await reader.ReadLineAsync()) != null)
        {
            // Removing comment
            line = _regexMaskRemovingComment.Match(line).Groups[0].Value;
            if (line == "") { continue; }

            // Splitting command line
            var match = _regexMaskCommandLine.Match(line);

            var mark = match.Groups[1].Value.Trim().Trim(BadSymbols).ToLower();
            var instruction = match.Groups[2].Value.Trim(BadSymbols).ToLower();
            var arguments = match.Groups[3].Captures.Select(c => c.Value.Trim(BadSymbols).ToLower());

            var command = new CommandLine(mark, instruction, arguments);

            res.Add(command);
        }

        return res;
    }
}
