using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssemblerLib;

public class Assembler
{
    private string _mainFilePath;
    private List<string> _linkedFilesPaths;
    private readonly Parser _parser;
    private readonly TokenBuilder _tokenBuilder;
    private readonly List<CommandLine> _mainFileCommands;
    private readonly List<List<CommandLine>> _linkedFilesCommands;
    private readonly Dictionary<string, int> _marksDict;
    private readonly List<IToken> _tokens;
    private readonly List<string> _machineCode;

    private void _PrintCommands()
    {
        foreach (var i in _mainFileCommands)
        {
            Console.WriteLine($"{string.Join(',', i.Marks)}: {i.InstructionMnemonics} {string.Join(',', i.Arguments)}");
        }

        foreach (var i in _linkedFilesCommands)
        {
            foreach (var j in i)
            {
                Console.WriteLine($"{string.Join(',', j.Marks)}: {j.InstructionMnemonics} {string.Join(',', j.Arguments)}");
            }
        }
    }

    private void _PrintMachineCode()
    {
        Console.WriteLine("\nMACHINE CODE");
        var address = 0;
        foreach (var mCode in _machineCode)
        {
            Console.WriteLine($"{Convert.ToString(address, 8).PadLeft(6, '0')} {mCode}");
            address += 2;
        }

        Console.WriteLine("\nMARKS");
        foreach (var markPair in _marksDict)
        {
            Console.WriteLine($"{markPair.Key}: {Convert.ToString(markPair.Value, 8).PadLeft(6, '0')}");
        }
    }

    public Assembler(string mainFilePath, IEnumerable<string> linkedFilesPaths)
    {
        _mainFilePath = mainFilePath;
        _linkedFilesPaths = linkedFilesPaths.ToList();
        _parser = new Parser();
        _tokenBuilder = new TokenBuilder();

        _mainFileCommands = new List<CommandLine>();
        _linkedFilesCommands = new List<List<CommandLine>>();

        _marksDict = new Dictionary<string, int>();
        _tokens = new List<IToken>();
        _machineCode = new List<string>();
    }

    public async Task Assemble()
    {
        // Parsing (the first assembly cycle)
        _mainFileCommands.AddRange(await _parser.Parse(_mainFilePath));

        for (int i = 0; i < _linkedFilesPaths.Count; i++)
        {
            _linkedFilesCommands.Add(new List<CommandLine>(await _parser.Parse(_linkedFilesPaths[i])));
        }
        _PrintCommands();


        // The second assembly cycle
        int currentAddr = 0;
        List<IToken> newTokens = new List<IToken>();
        foreach (var cmdLine in _mainFileCommands)
        {
            foreach (var mark in cmdLine.Marks)
            {
                if (string.IsNullOrWhiteSpace(mark)) { continue; }
                if (!_marksDict.ContainsKey(mark))
                {
                    _marksDict.Add(mark, currentAddr);
                }
                else
                {
                    throw new Exception($"The mark \"{mark}\" has been used several times.");
                }
            }
            newTokens = _tokenBuilder.Build(cmdLine);
            _tokens.AddRange(newTokens);
            currentAddr += newTokens.Count * 2;
        }


        // The third assembly cycle
        currentAddr = 0;
        foreach (var token in _tokens)
        {
            _machineCode.AddRange(token.Translate(_marksDict, currentAddr));
            currentAddr += 2;
        }


        // Printing of final machine code
        _PrintMachineCode();
    }
}
