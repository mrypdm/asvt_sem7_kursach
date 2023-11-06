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
    private HeadlessUnitTestSession _session;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _session = HeadlessUnitTestSession.StartNew(typeof(TApp));
    }
    
    protected void RunTest(Action testMethod)
    {
        //using var session = HeadlessUnitTestSession.StartNew(typeof(TApp));
        _session.Dispatch(testMethod, default).Wait();
    }
    
    protected void RunAsyncTest(Func<Task> testMethod)
    {
        //using var session = HeadlessUnitTestSession.StartNew(typeof(TApp));
        _session.Dispatch(testMethod, default).Wait();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _session.Dispose();
    }
}