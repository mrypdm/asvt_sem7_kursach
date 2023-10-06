using System.Threading.Tasks;

namespace Shared.Converters;

/// <summary>
/// Type converter
/// </summary>
/// <typeparam name="TIn">Input type</typeparam>
/// <typeparam name="TOut">Output type</typeparam>
public interface IConverter<in TIn, TOut>
{
    /// <summary>
    /// Converts value
    /// </summary>
    /// <param name="value">Input value</param>
    /// <returns>Converted value</returns>
    TOut Convert(TIn value);

    /// <summary>
    /// Converts value asynchronously
    /// </summary>
    /// <param name="value">Input value</param>
    /// <returns>Converted value</returns>
    Task<TOut> ConvertAsync(TIn value);
}