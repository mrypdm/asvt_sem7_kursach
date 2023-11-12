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

    public Assembler(string mainFilePath, IEnumerable<string> linkedFilesPaths)
    {
        _mainFilePath = mainFilePath;
        _linkedFilesPaths = linkedFilesPaths.ToList();
        _parser = new Parser();

        _mainFileCommands = new List<CommandLine>();
        _linkedFilesCommands = new List<List<CommandLine>>();

        _machineCode = new List<string>();
    }

    public async Task Assemble()
    {
        // Parsing
        _mainFileCommands.AddRange(await _parser.Parse(_mainFilePath));

        for (int i = 0; i < _linkedFilesPaths.Count; i++)
        {
            _linkedFilesCommands.Add(new List<CommandLine>(await _parser.Parse(_linkedFilesPaths[i])));
        }
        _PrintCommands();

        // Assembling
        foreach (var cmdLine in _mainFileCommands)
        {

        }
    }
}
