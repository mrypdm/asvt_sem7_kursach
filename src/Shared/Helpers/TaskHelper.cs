using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Helpers;

/// <summary>
/// Helper for <see cref="Task"/>
/// </summary>
public static class TaskHelper
{
    /// <summary>
    /// Waits for condition to become true
    /// </summary>
    /// <param name="condition">Condition</param>
    /// <param name="timeout">Timeout of waiting</param>
    /// <param name="period">Period of checks</param>
    public static async Task WaitForCondition(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? period = null)
    {
        timeout ??= TimeSpan.MaxValue;
        period ??= TimeSpan.FromMilliseconds(500);

        using var cancel = new CancellationTokenSource(timeout.Value);
        
        while (!cancel.IsCancellationRequested && !condition())
        {
            await Task.Delay(period.Value, cancel.Token);
        }
        
        cancel.Token.ThrowIfCancellationRequested();
    }
}