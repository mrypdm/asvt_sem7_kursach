using DeviceSdk;

namespace Executor.Storages;

public interface IBus
{
    IDevice GetInterrupt(int currentPriority);
}