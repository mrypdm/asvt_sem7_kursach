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
    private readonly List<CommandLine> _commands;

    public Assembler(string mainFilePath, IEnumerable<string> linkedFilesPaths)
    {
        _mainFilePath = mainFilePath;
        _linkedFilesPaths = linkedFilesPaths.ToList();
        _parser = new Parser();
        _commands = new List<CommandLine>();
    }

    public async Task Assemble()
    {
        _commands.AddRange(await _parser.Parse(_mainFilePath));

        foreach (var i in _commands)
        {
            Console.WriteLine($"{i.Mark}; {i.Instruction}; {string.Join(';', i.Arguments)}");
        }
    }
}
