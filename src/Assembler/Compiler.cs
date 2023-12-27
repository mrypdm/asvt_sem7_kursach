using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assembler.Exceptions;
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

        // Parsing (the first assembly cycle)
        var mainCommandLines = await _parser.Parse(mainFile);

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
                    throw new AssembleException(cmdLine, $"The mark '{mark}' has been used several times");
                }
            }

            try
            {
                var cmdTokens = _tokenBuilder.Build(cmdLine).ToArray();
                tokens.AddRange(cmdTokens);

                currentAddr += cmdTokens.Length * 2;
            }
            catch (Exception e)
            {
                throw new AssembleException(cmdLine, e.Message);
            }
        }

        // The third assembly cycle
        currentAddr = 0;
        var codes = new List<string>();
        foreach (var token in tokens)
        {
            try
            {
                var machineCodes = token.Translate(marks, currentAddr);
                codes.AddRange(machineCodes);
                currentAddr += 2;
            }
            catch (Exception e)
            {
                throw new AssembleException(token.CommandLine, e.Message);
            }
        }

        // Printing of final machine code
        PrintMachineCode(codes, marks);
        
        // Writing result machine code in file
        await File.WriteAllLinesAsync(project.ProjectBinary, codes);
    }
}