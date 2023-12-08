namespace Executor.Memories;

public interface IMemory : IReadOnlyMemory
{
    IReadOnlyCollection<byte> Data { get; }

    void SetWord(ushort address, ushort value);

    void SetByte(ushort address, byte value);
}