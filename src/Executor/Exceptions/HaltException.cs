using System;

namespace Executor.Exceptions;

public class HaltException : Exception
{
    public HaltException(bool expected, string message = null) : base(message)
    {
        IsExpected = expected;
    }

    public bool IsExpected { get; }
}