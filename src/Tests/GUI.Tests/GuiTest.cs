using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Headless;

namespace GUI.Tests;

/// <summary>
/// Base class for testing GUI elements
/// </summary>
/// <typeparam name="TApp">Heir of <see cref="Application"/></typeparam>
public abstract class GuiTest<TApp>
{
    protected async Task RunTest(Action testMethod)
    {
        using var session = HeadlessUnitTestSession.StartNew(typeof(TApp));
        await session.Dispatch(testMethod, default);
    }
    
    protected async Task RunAsyncTest(Func<Task> testMethod)
    {
        using var session = HeadlessUnitTestSession.StartNew(typeof(TApp));
        await session.Dispatch(testMethod, default);
    }
}