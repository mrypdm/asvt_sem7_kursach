using System.Reflection;

namespace Executor.Exceptions;

public class ReadOnlyArgumentException : Exception
{
    public ReadOnlyArgumentException(MemberInfo type) : base($"{type.Name} is read only")
    {
    }
}