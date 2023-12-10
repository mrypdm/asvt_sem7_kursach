using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument referencing a register
/// </summary>
public class RegisterArgument : RegisterWordArgument
{
    public RegisterArgument(IStorage storage, IState state, ushort register)
        : base(storage, state, 0, register)
    {
    }
}