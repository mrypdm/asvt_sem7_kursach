namespace Executor.Arguments.Abstraction;

public interface IWordRegisterArgument : IArgument
{
    ushort Register { get; }
    
    ushort Mode { get; }
    
    ushort GetWord();

    void SetWord(ushort value);
}