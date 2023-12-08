using System;

namespace Executor.Exceptions;

public class InvalidInstructionException : Exception
{
    public InvalidInstructionException(string message) : base(message)
    {
    }
}