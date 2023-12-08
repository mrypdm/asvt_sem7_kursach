using System.Collections.Generic;

namespace Executor.Memories;

public interface IReadOnlyMemory
{
    IReadOnlyCollection<byte> Data { get; }

    ushort GetWord(ushort address);

    byte GetByte(ushort address);
}