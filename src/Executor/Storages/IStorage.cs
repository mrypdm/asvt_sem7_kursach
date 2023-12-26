namespace Executor.Storages;

public interface IStorage : IReadOnlyStorage
{
    void SetWord(ushort address, ushort value);

    void SetByte(ushort address, byte value);

    void Init();
}