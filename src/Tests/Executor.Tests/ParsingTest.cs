using System.Linq;
using Executor.Arguments;
using Executor.Commands.TwoOperands;
using Executor.States;
using Executor.Storages;

namespace Executor.Tests;

public class ParsingTests
{
    [Test]
    [TestCase((ushort)0b001_010_111_000_000, 2, 7, 0, 0)] // 0o012700 MOV #5, R0
    [TestCase((ushort)0b001_110_010_110_101, 6, 2, 6, 5)] // 0o066265 MOV 300(R2), 200(R5)
    [TestCase((ushort)0b001_110_111_000_001, 6, 7, 0, 1)] // 0o016701 MOV MARK, R1
    public void TestTwoWordOperands(ushort word, int mode1, int reg1, int mode2, int reg2)
    {
        // Arrange
        var memory = new Memory();
        var state = new State();
        var command = new MOV(memory, state);

        // Act & Assert

        var args = command.GetArguments(word);
        Assert.That(args, Has.Length.EqualTo(2));
        Assert.That(args, Has.All.TypeOf<RegisterWordArgument>());

        var castedArgs = args.Select(m => m as RegisterWordArgument).ToArray();
        Assert.That(castedArgs[0].Mode, Is.EqualTo(mode1));
        Assert.That(castedArgs[0].Register, Is.EqualTo(reg1));
        Assert.That(castedArgs[1].Mode, Is.EqualTo(mode2));
        Assert.That(castedArgs[1].Register, Is.EqualTo(reg2));
    }
}