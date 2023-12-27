using System;

namespace Executor.Exceptions;

public class ReservedInstructionException : Exception
{
    public ReservedInstructionException(ushort code)
        : base($"Reserved instruction '{Convert.ToString(code, 8).PadLeft(6, '0')}'")
    {
    }
}