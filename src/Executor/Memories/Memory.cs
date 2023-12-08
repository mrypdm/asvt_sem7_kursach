using Executor.Exceptions;

namespace Executor.Memories;

public class Memory : IMemory
{
    private readonly byte[] _memory = new byte[65026];

    public byte GetByte(ushort address) => _memory[address];

    public IReadOnlyCollection<byte> GetMemory() => _memory;

    public ushort GetWord(ushort address)
    {
        ThrowIfOdd(address);

        var lowByte = _memory[address];
        var highByte = _memory[address + 1];

        return (ushort)((highByte << 8) | lowByte);
    }

    public void SetByte(ushort address, byte value)
    {
        _memory[address] = value;
    }

    public void SetWord(ushort address, ushort value)
    {
        ThrowIfOdd(address);

        _memory[address] = (byte)(value & 255);
        _memory[address + 1] = (byte)(value >> 8);
    }

    private static void ThrowIfOdd(ushort address)
    {
        if (address % 2 == 1)
        {
            throw new OddAddressException(address);
        }
    }
}