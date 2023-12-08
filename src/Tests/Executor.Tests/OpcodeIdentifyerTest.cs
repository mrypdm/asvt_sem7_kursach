using Executor.States;
using Executor.Storages;

namespace Executor.Tests;

public class OpcodeIdentifierTest
{
    [Test]
    public void TestGetCommand()
    {
        // Arrange

        const ushort word1 = 0b1010111000000; // MOV
        var state = new State();
        var memory = new Memory();
        var opcodeIdentifier = new OpcodeIdentifier(state, memory);

        // Act

        var command = opcodeIdentifier.GetCommand(word1);

        // Assert

        Assert.That(command.Opcode, Is.EqualTo(0b1_0000_0000_0000));
    }
}