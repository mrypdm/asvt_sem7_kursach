namespace Executor.Arguments.Abstraction;

public interface IArgument
{
    object GetValue();

    void SetValue(object obj);
}