using System;
using Executor.Arguments.Abstraction;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument referencing a byte
/// </summary>
public class RegisterByteArgument : BaseRegisterArgument<byte>
{
    public RegisterByteArgument(IStorage storage, IState state, ushort mode, ushort register)
        : base(storage, state, mode, register)
    {
    }

    public override byte Value
    {
        get => !Address.HasValue ? (byte)(State.Registers[Register] & 0xFF) : Storage.GetByte(Address.Value);
        set
        {
            if (!Address.HasValue)
            {
                State.Registers[Register] = (ushort)((State.Registers[Register] & 0xFF00) | value);
                return;
            }
            
            Storage.SetByte(Address!.Value, value);
        }
    }
    protected override ushort Delta => (ushort)(Register < 6 ? 1 : 2);
}