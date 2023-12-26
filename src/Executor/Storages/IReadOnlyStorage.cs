using System.Collections.Generic;

namespace Executor.Storages;

public interface IReadOnlyStorage
{
    IReadOnlyCollection<byte> Data { get; }

    ushort GetWord(ushort address);

    byte GetByte(ushort address);
}