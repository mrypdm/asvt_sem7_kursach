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
    private readonly List<CommandLine> _mainFileCommands;
    private readonly List<List<CommandLine>> _linkedFilesCommands;

    public Assembler(string mainFilePath, IEnumerable<string> linkedFilesPaths)
    {
        _mainFilePath = mainFilePath;
        _linkedFilesPaths = linkedFilesPaths.ToList();
        _parser = new Parser();

        _mainFileCommands = new List<CommandLine>();
        _linkedFilesCommands = new List<List<CommandLine>>();
    }

    public async Task Assemble()
    {
        _mainFileCommands.AddRange(await _parser.Parse(_mainFilePath));

        for (int i = 0; i < _linkedFilesPaths.Count; i++)
        {
            _linkedFilesCommands.Add(new List<CommandLine>(await _parser.Parse(_linkedFilesPaths[i])));
        }

        foreach (var i in _mainFileCommands)
        {
            Console.WriteLine($"{i.Mark}; {i.InstructionMnemonics}; {string.Join(';', i.Arguments)}");
        }

        foreach (var i in _linkedFilesCommands)
        {
            foreach (var j in i)
            {
                Console.WriteLine($"{j.Mark}; {j.InstructionMnemonics}; {string.Join(';', j.Arguments)}");
            }
        }
    }
}
