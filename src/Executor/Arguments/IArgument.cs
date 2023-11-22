namespace Executor.Arguments;

public interface IArgument
{
    ushort GetValue();

    void SetValue(ushort word);
}