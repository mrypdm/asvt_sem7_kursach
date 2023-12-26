using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.Commands.TwoOperands;
using Executor.States;
using Executor.Storages;

namespace Executor.Tests;

public class TwoOperandTest
{
    [Test]
    [TestCase((ushort)1500, (ushort)1500, false, false)]
    [TestCase((ushort)0, (ushort)2504, true, false)]
    [TestCase((ushort)33088, (ushort)32768, false, true)]
    public void TestBIT(ushort value1, ushort value2, bool expectedZ, bool expectedN)
    {
        // Arrange

        var memory = new Memory();
        var state = new State
        {
            Registers =
            {
                [1] = value1,
                [2] = value2
            }
        };

        var arg = new IArgument[]
        {
            new RegisterWordArgument(memory, state, 0, 1),
            new RegisterWordArgument(memory, state, 0, 2)
        };
        var command = new BIT(memory, state);

        // Act

        command.Execute(arg);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(state.Z, Is.EqualTo(expectedZ));
            Assert.That(state.N, Is.EqualTo(expectedN));
            Assert.That(state.V, Is.EqualTo(false));
            Assert.That(state.C, Is.EqualTo(false));
        });
    }

    [Test]
    [TestCase((ushort)32255, false, false)]
    [TestCase((ushort)128, true, false)]
    [TestCase((ushort)65535, false, true)]
    public void TestBITB(ushort value1, bool expectedZ, bool expectedN)
    {
        // Arrange

        var memory = new Memory();
        memory.SetWord(1002, value1);
        var state = new State
        {
            Registers =
            {
                [1] = 1002,
            }
        };

        var arg = new IArgument[]
        {
            new RegisterByteArgument(memory, state, 2, 1),
            new RegisterByteArgument(memory, state, 2, 1)
        };
        var command = new BITB(memory, state);

        // Act

        command.Execute(arg);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(state.Z, Is.EqualTo(expectedZ));
            Assert.That(state.N, Is.EqualTo(expectedN));
            Assert.That(state.V, Is.EqualTo(false));
            Assert.That(state.C, Is.EqualTo(false));
        });
    }
}