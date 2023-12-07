using System;

namespace Shared.Exceptions;

/// <summary>
/// Validation exception
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(string message = null, Exception innerException = null) : base(message, innerException)
    {
    }
}