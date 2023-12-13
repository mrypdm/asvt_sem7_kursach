using System;

namespace Executor.Extensions;

/// <summary>
/// Extensions methods of numbers
/// </summary>
public static class NumbersExtensions
{
    /// <summary>
    /// Checks high bit of <see cref="UInt32"/>
    /// </summary>
    public static bool IsNegative(this uint value) => (value & 0x80000000) != 0;

    /// <summary>
    /// Checks high bit of <see cref="UInt16"/>
    /// </summary>
    public static bool IsNegative(this ushort value) => (value & 0x8000) != 0;

    /// <summary>
    /// Checks high bit of <see cref="Byte"/>
    /// </summary>
    public static bool IsNegative(this byte value) => (value & 0x80) != 0;

    /// <summary>
    /// Compares high bits of <paramref name="a"/> and <paramref name="b"/>
    /// </summary>
    public static bool IsSameSignWith(this ushort a, ushort b) => !((ushort)(a ^ b)).IsNegative();

    /// <summary>
    /// Compares high bits of <paramref name="a"/> and <paramref name="b"/>
    /// </summary>
    public static bool IsSameSignWith(this byte a, byte b) => !((byte)(a ^ b)).IsNegative();

    /// <summary>
    /// Represents <see cref="Int32"/> as <see cref="Single"/>
    /// </summary>
    public static float AsFloat(this int n) => BitConverter.Int32BitsToSingle(n);

    /// <summary>
    /// Represents <see cref="Single"/> as <see cref="UInt32"/>
    /// </summary>
    public static uint AsUInt(this float n) => BitConverter.SingleToUInt32Bits(n);
}