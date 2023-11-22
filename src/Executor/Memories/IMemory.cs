namespace Executor.Memories;

public interface IMemory
{
    ushort GetWord(ushort address);

    byte GetByte(ushort address);

    void SetWord(ushort address, ushort value);

    void SetByte(ushort address, byte value);
}