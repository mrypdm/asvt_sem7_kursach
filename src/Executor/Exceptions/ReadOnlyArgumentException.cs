using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Executor.Exceptions;

public class ReadOnlyArgumentException : Exception
{
    public ReadOnlyArgumentException(MemberInfo type) : base($"{type.Name} is read only")
    {
    }
}