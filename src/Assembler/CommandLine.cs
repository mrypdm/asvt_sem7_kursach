using System.Collections.Generic;
using System.Linq;

namespace Assembler;

/// <summary>
/// Line of assembler text
/// </summary>
internal record CommandLine
{
    private const string RegexPatternMarkValidation = @"^\s*[a-zA-Z]+([^:;]\w)*(?=:)";

    /// <summary>
    /// Creates new instance of command line
    /// </summary>
    /// <param name="marks">Symbol marks of line</param>
    /// <param name="instructionMnemonics">Instruction to execute</param>
    /// <param name="args">Arguments of instruction</param>
    public CommandLine(IEnumerable<string> marks, string instructionMnemonics, IEnumerable<string> args)
    {
        Marks = marks.ToHashSet();
        InstructionMnemonics = instructionMnemonics;
        Arguments = args.ToList();
    }

    public void ThrowIfInvalid()
    {
        // Mnemonics validation
        if (string.IsNullOrWhiteSpace(InstructionMnemonics))
        {
            return;
        }

        if (!Instruction.Instructions.ContainsKey(InstructionMnemonics))
        {
            throw new System.Exception($"Unexisting instruction: {InstructionMnemonics}.");
        }

        if (Arguments.Count != Instruction.Instructions[InstructionMnemonics].ArgumentsCount)
        {
            throw new System.Exception(
                $"Incorrect number of arguments: {InstructionMnemonics}. " +
                $"Must be {Instruction.Instructions[InstructionMnemonics].ArgumentsCount}, " +
                $"but was: {Arguments.Count}.");
        }
    }

    public string GetSymbol()
    {
        return $"{string.Join(',', Marks)}: {InstructionMnemonics} {string.Join(',', Arguments)}";
    }

    /// <summary>
    /// Symbol mark for line
    /// </summary>
    public IEnumerable<string> Marks { get; }

    /// <summary>
    /// Instruction to execute
    /// </summary>
    public string InstructionMnemonics { get; }

    /// <summary>
    /// Arguments for instruction
    /// </summary>
    public List<string> Arguments { get; }
}