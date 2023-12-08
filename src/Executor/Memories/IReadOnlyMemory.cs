namespace Executor.Memories;

public interface IReadOnlyMemory
{
    ushort GetWord(ushort address);
    byte GetByte(ushort address);
}