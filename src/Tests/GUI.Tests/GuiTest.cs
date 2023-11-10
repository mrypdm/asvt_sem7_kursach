using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Headless;

namespace GUI.Tests;

/// <summary>
/// Base class for testing GUI elements
/// </summary>
/// <typeparam name="TApp">Heir of <see cref="Application"/></typeparam>
public abstract class GuiTest<TApp> where TApp : Application
{
    private HeadlessUnitTestSession _session;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _session = HeadlessUnitTestSession.StartNew(typeof(TApp));
    }

    protected Task RunTest(Action testMethod)
    {
        return _session.Dispatch(testMethod, default);
    }

    protected Task RunAsyncTest(Func<Task> testMethod)
    {
        return _session.Dispatch(testMethod, default);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _session.Dispose();
    }
}