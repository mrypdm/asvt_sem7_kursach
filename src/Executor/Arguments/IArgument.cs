namespace Executor.Arguments;

internal interface IArgument
{
    ushort GetValue();

    void SetValue(ushort word);
}