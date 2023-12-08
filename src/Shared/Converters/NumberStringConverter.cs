using System.Threading.Tasks;

namespace Shared.Converters;

/// <summary>
/// Converts a <see cref="string"/> to a <see cref="ushort"/> using the number system
/// </summary>
public class NumberStringConverter : IConverter<string, ushort>
{
    /// <inheritdoc />
    public ushort Convert(string value)
    {
        if (value.StartsWith("0b"))
        {
            return System.Convert.ToUInt16(value[2..], 2);
        }

        if (value.StartsWith("0o"))
        {
            return System.Convert.ToUInt16(value[2..], 8);
        }

        if (value.StartsWith("0d"))
        {
            return System.Convert.ToUInt16(value[2..], 10);
        }

        if (value.StartsWith("0x"))
        {
            return System.Convert.ToUInt16(value[2..], 16);
        }

        return System.Convert.ToUInt16(value, 10);
    }

    /// <inheritdoc />
    public Task<ushort> ConvertAsync(string value) => Task.FromResult(Convert(value));
}