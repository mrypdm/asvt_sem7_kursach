namespace Executor.Memories;

public interface IMemory : IReadOnlyMemory
{
    void SetWord(ushort address, ushort value);

    void SetByte(ushort address, byte value);
}