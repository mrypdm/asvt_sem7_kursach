using NUnit.Framework;
using Executor;
namespace Executor.Tests;


public class StateTest
{

    [Test]
    public void TestSetGetFlag()
    {   
        State state = new State();
        state.SetFlag(Flag.Z, 1);
        state.SetFlag(Flag.C, 1);
        Assert.AreEqual(state.GetFlag(Flag.V), 0, "Wrong flag V");
        Assert.AreEqual(state.GetFlag(Flag.Z), 1, "Wrong flag Z");
        Assert.AreEqual(state.GetFlag(Flag.C), 1, "Wrong flag C");
        state.SetFlag(Flag.C, 0);
        Assert.AreEqual(state.GetFlag(Flag.C), 0, "Wrong flag C");
        state.SetFlag(Flag.Z, 0);
        Assert.AreEqual(state.GetFlag(Flag.Z), 0, "Wrong flag Z");
    }
}