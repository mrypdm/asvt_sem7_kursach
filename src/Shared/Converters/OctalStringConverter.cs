using System.Threading.Tasks;

namespace Shared.Converters;

/// <summary>
/// Converts a number to a string in octal notation
/// </summary>
public class OctalStringConverter : IConverter<int, string>
{
    /// <inheritdoc />
    public string Convert(int value)
    {
        return $"0o{System.Convert.ToString(value, 8)}";
    }

    /// <inheritdoc />
    public Task<string> ConvertAsync(int value) => Task.FromResult(Convert(value));
}