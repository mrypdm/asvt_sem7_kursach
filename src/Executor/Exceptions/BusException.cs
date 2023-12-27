using System;

namespace Executor.Exceptions;

public class BusException : Exception
{
    public BusException(string message) : base(message)
    {
        
    }
}