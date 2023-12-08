namespace Executor.Arguments.Abstraction;

public interface IByteRegisterArgument : IArgument
{
    ushort Register { get; }

    ushort Mode { get; }

    byte GetByte();

    void SetByte(byte value);
}