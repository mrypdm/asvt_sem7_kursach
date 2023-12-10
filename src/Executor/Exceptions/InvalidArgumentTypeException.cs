using System;
using System.Reflection;

namespace Executor.Exceptions;

public class InvalidArgumentTypeException : Exception
{
    public InvalidArgumentTypeException(MemberInfo expectedType, MemberInfo actualType)
        : base($"Invalid argument type. Expected: {expectedType.Name}. But was: {actualType.Name}")
    {
    }
}