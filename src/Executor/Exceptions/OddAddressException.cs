using System;

namespace Executor.Exceptions;

public class OddAddressException : Exception
{
    public OddAddressException(ushort address) : base($"Attempt to access to odd address {address}")
    {
    }
}