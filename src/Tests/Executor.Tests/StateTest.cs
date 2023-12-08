using Executor.States;

namespace Executor.Tests;

public class StateTest
{
    [Test]
    public void TestSetGetFlag()
    {
        // Arrange
        var state = new State();

        // Act & Assert

        state.SetFlag(Flag.Z, true);
        state.SetFlag(Flag.C, true);
        Assert.That(state.GetFlag(Flag.V), Is.False);
        Assert.That(state.GetFlag(Flag.Z), Is.True);
        Assert.That(state.GetFlag(Flag.C), Is.True);

        state.SetFlag(Flag.C, false);
        Assert.That(state.GetFlag(Flag.C), Is.False);

        state.SetFlag(Flag.Z, false);
        Assert.That(state.GetFlag(Flag.Z), Is.False);
    }
}