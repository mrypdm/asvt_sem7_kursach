namespace Executor.Extensions;

public static class NumbersExtensions
{
    public static bool IsNegative(this ushort value) => (value & 0x8000) != 0;

    public static bool IsNegative(this byte value) => (value & 0x80) != 0;
}