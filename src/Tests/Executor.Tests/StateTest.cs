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

        state.Z = true;
        state.C = true;
        Assert.That(state.V, Is.False);
        Assert.That(state.Z, Is.True);
        Assert.That(state.C, Is.True);

        state.C = false;
        Assert.That(state.C, Is.False);

        state.Z = false;
        Assert.That(state.Z, Is.False);
    }
}