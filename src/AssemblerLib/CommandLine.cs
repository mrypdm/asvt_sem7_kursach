using System.Collections.Generic;
using System.Linq;

namespace AssemblerLib;

/// <summary>
/// Line of assembler text
/// </summary>
internal record CommandLine
{
    private string _mark;
    private string _instruction;
    private List<string> _args;

    /// <summary>
    /// Creates new instance of command line
    /// </summary>
    /// <param name="mark">Symbol mark of line</param>
    /// <param name="instruction">Instruction to execute</param>
    /// <param name="args">Arguments of instruction</param>
    public CommandLine(string mark, string instruction, IEnumerable<string> args)
    {
        _mark = mark;
        _instruction = instruction;
        _args = args.ToList();
    }

    /// <summary>
    /// Symbol mark for line
    /// </summary>
    public string Mark => _mark;

    /// <summary>
    /// Instruction to execute
    /// </summary>
    public string Instruction => _instruction;

    /// <summary>
    /// Arguments for instruction
    /// </summary>
    public IEnumerable<string> Arguments => _args;
}
