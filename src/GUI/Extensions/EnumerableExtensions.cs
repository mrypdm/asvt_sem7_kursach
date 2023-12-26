using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GUI.Extensions;

/// <summary>
/// Extensions for <see cref="IEnumerable{T}"/>
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Coverts <see cref="IEnumerable{T}"/> to <see cref="ObservableCollection{T}"/>
    /// </summary>
    public static ObservableCollection<TItem> ToObservableCollection<TItem>(this IEnumerable<TItem> source) =>
        new(source);
}