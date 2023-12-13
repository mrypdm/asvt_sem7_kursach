namespace Executor.Extensions;

public static class NumbersExtensions
{
    public static bool IsNegative(this uint value) => (value & 0x80000000) != 0;

    public static bool IsNegative(this ushort value) => (value & 0x8000) != 0;

    public static bool IsSameSignWith(this ushort a, ushort b) => !((ushort)(a ^ b)).IsNegative();

    public static bool IsNegative(this byte value) => (value & 0x80) != 0;

    public static bool IsSameSignWith(this byte a, byte b) => !((byte)(a ^ b)).IsNegative();
}