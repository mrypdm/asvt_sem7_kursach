using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assembler.Tokens;
using Domain.Models;

namespace Assembler;

public class Compiler
{
    private readonly Parser _parser;
    private readonly TokenBuilder _tokenBuilder;

    private void PrintCommands(IEnumerable<CommandLine> main, IEnumerable<IEnumerable<CommandLine>> linked)
    {
        foreach (var cmd in main)
        {
            Console.WriteLine(
                $"{string.Join(',', cmd.Marks)}: {cmd.InstructionMnemonics} {string.Join(',', cmd.Arguments)}");
        }

        foreach (var linkedFile in linked)
        {
            foreach (var cmd in linkedFile)
            {
                Console.WriteLine(
                    $"{string.Join(',', cmd.Marks)}: {cmd.InstructionMnemonics} {string.Join(',', cmd.Arguments)}");
            }
        }
    }

    private void PrintMachineCode(IEnumerable<string> codes, IDictionary<string, int> marks)
    {
        Console.WriteLine("\nMACHINE CODE");
        var address = 0;
        foreach (var mCode in codes)
        {
            Console.WriteLine($"{Convert.ToString(address, 8).PadLeft(6, '0')} {mCode}");
            address += 2;
        }

        Console.WriteLine("\nMARKS");
        foreach (var markPair in marks)
        {
            Console.WriteLine($"{markPair.Key}: {Convert.ToString(markPair.Value, 8).PadLeft(6, '0')}");
        }
    }

    public Compiler()
    {
        _parser = new Parser();
        _tokenBuilder = new TokenBuilder();
    }

    public async Task Compile(IProject project)
    {
        var mainFile = project.Executable;
        var linkedFiles = project.Files.Where(m => m != mainFile).ToArray();

        // Parsing (the first assembly cycle)
        var mainCommandLines = await _parser.Parse(mainFile);

        var linkedFilesCommands = new List<List<CommandLine>>();
        foreach (var linkedFile in linkedFiles)
        {
            var linkedFileCommands = await _parser.Parse(linkedFile);
            linkedFilesCommands.Add(linkedFileCommands);
        }

        PrintCommands(mainCommandLines, linkedFilesCommands);

        // The second assembly cycle
        var tokens = new List<IToken>();
        var marks = new Dictionary<string, int>();
        var currentAddr = 0;
        foreach (var cmdLine in mainCommandLines)
        {
            foreach (var mark in cmdLine.Marks)
            {
                if (string.IsNullOrWhiteSpace(mark))
                {
                    continue;
                }

                if (!marks.ContainsKey(mark))
                {
                    marks.Add(mark, currentAddr);
                }
                else
                {
                    throw new Exception($"The mark '{mark}' has been used several times");
                }
            }

            var cmdTokens = _tokenBuilder.Build(cmdLine);
            tokens.AddRange(cmdTokens);
            currentAddr += tokens.Count * 2;
        }

        // The third assembly cycle
        currentAddr = 0;
        var codes = new List<string>();
        foreach (var token in tokens)
        {
            codes.AddRange(token.Translate(marks, currentAddr));
            currentAddr += 2;
        }

        // Printing of final machine code
        PrintMachineCode(codes, marks);
    }
}