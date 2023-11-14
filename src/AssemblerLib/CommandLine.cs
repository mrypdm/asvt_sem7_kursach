using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AssemblerLib;

/// <summary>
/// Line of assembler text
/// </summary>
internal record CommandLine
{
    private const string RegexPatternMarkValidation = @"^\s*[a-zA-Z]+([^:;]\w)*(?=:)";
    
    private HashSet<string> _marks;
    private string _instructionMnemonics;
    private List<string> _args;

    private bool CommandLineValidation()
    {
        // Mnemonics validation
        if (!string.IsNullOrWhiteSpace(_instructionMnemonics))
        {
            if (!Instruction.instructions.ContainsKey(_instructionMnemonics))
            {
                throw new System.Exception($"Unexisting instruction: {_instructionMnemonics}.");
            }

            if (_args.Count() != Instruction.instructions[_instructionMnemonics].ArgumentsCount)
            {
                throw new System.Exception(
                    $"Incorrect number of argumants: {_instructionMnemonics}. " +
                    $"Must be {Instruction.instructions[_instructionMnemonics].ArgumentsCount}. " +
                    $"Given: {_args.Count()}.");
            }
        }

        // Mark validation

        return true;
    }

    /// <summary>
    /// Creates new instance of command line
    /// </summary>
    /// <param name="mark">Symbol mark of line</param>
    /// <param name="instruction">Instruction to execute</param>
    /// <param name="args">Arguments of instruction</param>
    public CommandLine(IEnumerable<string> marks, string instructionMnemonics, IEnumerable<string> args)
    {
        _marks = marks.ToHashSet();
        _instructionMnemonics = instructionMnemonics;
        _args = args.ToList();

        CommandLineValidation();
    }

    /// <summary>
    /// Symbol mark for line
    /// </summary>
    public HashSet<string> Marks => _marks;

    /// <summary>
    /// Instruction to execute
    /// </summary>
    public string InstructionMnemonics => _instructionMnemonics;

    /// <summary>
    /// Arguments for instruction
    /// </summary>
    public IEnumerable<string> Arguments => _args;
}
