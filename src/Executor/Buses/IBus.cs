namespace Executor.Buses;

public interface IBus
{
    ushort GetWord(ushort address);

    byte GetByte(ushort address);

    void SetWord(ushort address, ushort value);

    void SetByte(ushort address, byte value);
}