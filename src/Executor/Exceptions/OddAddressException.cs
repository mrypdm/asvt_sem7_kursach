namespace Executor.Exceptions;

public class OddAddressException : BusException
{
    public OddAddressException(ushort address) : base($"Attempt to access to odd address {address}")
    {
    }
}