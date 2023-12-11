using System;
using Executor.Arguments.Abstraction;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument referencing a word
/// </summary>
public class RegisterWordArgument : BaseRegisterArgument<ushort>
{
    public RegisterWordArgument(IStorage storage, IState state, ushort mode, ushort register)
        : base(storage, state, mode, register)
    {
    }

    public override ushort Value
    {
        get => !Address.HasValue ? State.Registers[Register] : Storage.GetWord(Address.Value);
        set
        {
            if (!Address.HasValue)
            {
                State.Registers[Register] = value;
                return;
            }

            Storage.SetWord(Address!.Value, value);
        }
    }

    protected override ushort Delta => 2;
}