namespace Executor.Memories;

public interface IReadOnlyMemory
{
    IReadOnlyCollection<byte> GetMemory();

    ushort GetWord(ushort address);

    byte GetByte(ushort address);
}