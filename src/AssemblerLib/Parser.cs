﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssemblerLib;

internal class Parser
{
    private const string RegexPatternCommandLine = @"^\s*([^\s,:]+:\s*)?(\S+)?\s*([^\s,]+\s*,?\s*){0,}$";
    private const string RegexPatternRemovingComment = @"^[^;.]+(?=;?)";
    private const string RegexPatternMarkExistence = @"^\s*[^;]*:";
    private const string RegexPatternMarkValidation = @"^\s*[a-zA-Z]+[a-zA-Z0-9_]*([^:;]\w)*(?=:)"; //^\s*[a-zA-Z]+([^:;]\w)*(?=:)
    private readonly char[] BadSymbols = { ' ', '\t', ',', ':' };

    private readonly Regex _regexMaskCommandLine;
    private readonly Regex _regexMaskRemovingComment;
    private readonly Regex _regexMaskMarkExistence;
    private readonly Regex _regexMaskMarkValidation;

    public Parser()
    {
        _regexMaskCommandLine = new Regex(RegexPatternCommandLine, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskRemovingComment = new Regex(RegexPatternRemovingComment, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskMarkExistence = new Regex(RegexPatternMarkExistence, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskMarkValidation = new Regex(RegexPatternMarkValidation, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    public async Task<List<CommandLine>> Parse(string filePath)
    {
        var res = new List<CommandLine>();
        string line;

        using var reader = new StreamReader(filePath);
        var marksSet = new HashSet<string>();

        while ((line = await reader.ReadLineAsync()) != null)
        {
            // Removing comment
            line = line.Split(';', StringSplitOptions.TrimEntries)[0];
            if (string.IsNullOrWhiteSpace(line)) { continue; }

            // Marks validation
            var markExistence = _regexMaskMarkExistence.Match(line).Groups[0].Value;
            if (markExistence != "")
            {
                var markValid = _regexMaskMarkValidation.Match(line).Groups[0].Value;
                if ( markValid == "")
                {
                    throw new System.Exception($"Invalid mark: {markExistence}.");
                }
            }

            // Splitting command line
            var match = _regexMaskCommandLine.Match(line);

            var mark = match.Groups[1].Value.Trim().Trim(BadSymbols).ToLower();
            marksSet.Add(mark);
            var instruction = match.Groups[2].Value.Trim(BadSymbols).ToLower();
            if (string.IsNullOrWhiteSpace(instruction)) { continue; }
            var arguments = match.Groups[3].Captures.Select(c => c.Value.Trim(BadSymbols).ToLower());

            var command = new CommandLine(marksSet, instruction, arguments);
            command.ThrowIfInvalid();
            res.Add(command);
            marksSet.Clear();
        }

        return res;
    }
}
