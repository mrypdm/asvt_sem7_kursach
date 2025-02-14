﻿using System.Collections.Generic;
using System.Linq;
using Shared.Exceptions;

namespace Assembler;

/// <summary>
/// Line of assembler text
/// </summary>
internal record CommandLine
{
    /// <summary>
    /// Creates new instance of command line
    /// </summary>
    /// <param name="line">Number of line</param>
    /// <param name="marks">Symbol marks of line</param>
    /// <param name="instructionMnemonics">Instruction to execute</param>
    /// <param name="args">Arguments of instruction</param>
    public CommandLine(int line, IEnumerable<string> marks, string instructionMnemonics, IEnumerable<string> args)
    {
        LineNumber = line;
        Marks = marks.Where(m => !string.IsNullOrWhiteSpace(m)).ToHashSet();
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
            throw new ValidationException($"Unknown instruction: {InstructionMnemonics}");
        }

        if ((Arguments.Count != Instruction.Instructions[InstructionMnemonics].ArgumentsCount) &
            (Instruction.Instructions[InstructionMnemonics].ArgumentsCount != -1))
        {
            throw new ValidationException(
                $"Incorrect number of arguments: {InstructionMnemonics}. " +
                $"Must be {Instruction.Instructions[InstructionMnemonics].ArgumentsCount}, " +
                $"but was: {Arguments.Count}");
        }
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

    /// <summary>
    /// Line number
    /// </summary>
    public int LineNumber { get; }

    /// <summary>
    /// Line text
    /// </summary>
    public string LineText
    {
        get
        {
            var mark = Marks.Any() ? $"{string.Join(',', Marks)}: " : string.Empty;
            var arguments = Arguments.Any() ? $" {string.Join(", ", Arguments)}" : string.Empty;
            return $"{mark}{InstructionMnemonics}{arguments}";
        }
    }
}