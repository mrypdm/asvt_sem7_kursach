using System.Threading.Tasks;

namespace Shared.Converters;

public class NumberStringConverter : IConverter<string, int>
{
    public int Convert(string value)
    {
        if (value.StartsWith("0b"))
        {
            return System.Convert.ToInt32(value[2..], 2);
        }

        if (value.StartsWith("0o"))
        {
            return System.Convert.ToInt32(value[2..], 8);
        }

        if (value.StartsWith("0d"))
        {
            return System.Convert.ToInt32(value[2..], 10);
        }

        if (value.StartsWith("0x"))
        {
            return System.Convert.ToInt32(value[2..], 16);
        }

        return System.Convert.ToInt32(value, 10);
    }

    public Task<int> ConvertAsync(string value) => Task.FromResult(Convert(value));
}