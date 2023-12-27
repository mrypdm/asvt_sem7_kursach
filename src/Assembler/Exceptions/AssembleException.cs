using System;

namespace Assembler.Exceptions;

/// <summary>
/// Assemble exception
/// </summary>
public class AssembleException : Exception
{
    internal AssembleException(CommandLine commandLine, string message) : base(message)
    {
        LineNumber = commandLine.LineNumber;
        LineText = commandLine.LineText;
    }

    /// <summary>
    /// Line number which fail assemble
    /// </summary>
    public int LineNumber { get; }

    /// <summary>
    /// Line text which fail assemble
    /// </summary>
    public string LineText { get; }
}