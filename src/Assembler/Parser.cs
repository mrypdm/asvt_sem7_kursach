using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assembler;

internal class Parser
{
    private static readonly char[] BadSymbols = { ' ', '\t', ',', ':' };

    private readonly Regex _regexMaskCommandLine =
        new(@"^\s*([^\s,:]+:\s*)?(\S+)?\s*([^\s,]+\s*,?\s*){0,}$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    private readonly Regex _regexMaskRemovingComment =
        new(@"^[^;.]+(?=;?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    private readonly Regex _regexMaskMarkExistence =
        new(@"^\s*[^;]*:", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    private readonly Regex _regexMaskMarkValidation =
        new(@"^\s*[a-zA-Z]+[a-zA-Z0-9_]*([^:;]\w)*(?=:)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    public async Task<List<CommandLine>> Parse(string filePath)
    {
        var res = new List<CommandLine>();

        using var reader = new StreamReader(filePath);
        var marksSet = new HashSet<string>();

        var lineNumber = 0;
        while (await reader.ReadLineAsync() is { } line)
        {
            ++lineNumber;
            // Removing comment
            line = line.Split(';', StringSplitOptions.TrimEntries)[0];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // Marks validation
            var markExistence = _regexMaskMarkExistence.Match(line).Groups[0].Value;
            if (markExistence != "")
            {
                var markValid = _regexMaskMarkValidation.Match(line).Groups[0].Value;
                if (markValid == "")
                {
                    throw new Exception($"Invalid mark: {markExistence}.");
                }
            }

            // Splitting command line
            var match = _regexMaskCommandLine.Match(line);

            var mark = match.Groups[1].Value.Trim().Trim(BadSymbols).ToLower();
            marksSet.Add(mark);
            var instruction = match.Groups[2].Value.Trim(BadSymbols).ToLower();
            if (string.IsNullOrWhiteSpace(instruction))
            {
                continue;
            }

            var arguments = match.Groups[3].Captures.Select(c => c.Value.Trim(BadSymbols).ToLower());

            var command = new CommandLine(lineNumber, marksSet, instruction, arguments);
            command.ThrowIfInvalid();
            res.Add(command);
            marksSet.Clear();
        }

        return res;
    }
}