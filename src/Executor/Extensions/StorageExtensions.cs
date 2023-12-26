using Executor.States;
using Executor.Storages;

namespace Executor.Extensions;

public static class StorageExtensions
{
    public static void PushToStack(this IStorage storage, IState state, ushort value)
    {
        state.Registers[6] -= 2;
        storage.SetWord(state.Registers[6], value);
    }

    public static ushort PopFromStack(this IStorage storage, IState state)
    {
        var value = storage.GetWord(state.Registers[6]);
        state.Registers[6] += 2;
        return value;
    }
}