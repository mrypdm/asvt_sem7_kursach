using System;
using Executor.Commands;
using Executor.Commands.BranchOperations;
using Executor.Commands.MiscellaneousInstructions;
using Executor.Commands.OneOperands;
using Executor.Commands.Traps;
using Executor.Commands.TwoOperands;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Tests;

public class CommandParserTests
{
    private static readonly IState State = new State();
    private static readonly IStorage Memory = new Memory();

    [Test]
    [TestCaseSource(nameof(Commands))]
    public void TestGetCommand(ICommand command, ushort code)
    {
        // Arrange

        var parser = new CommandParser(Memory, State);

        // Act

        var parsedCommand = parser.GetCommand(code);

        // Assert

        Assert.That(parsedCommand.GetType(), Is.EqualTo(command.GetType()));
    }

    private static readonly object[] Commands =
    {
        new object[] { new CLR(Memory, State), Convert.ToUInt16("005077", 8) },
        new object[] { new CLRB(Memory, State), Convert.ToUInt16("105077", 8) },
        new object[] { new COM(Memory, State), Convert.ToUInt16("005177", 8) },
        new object[] { new COMB(Memory, State), Convert.ToUInt16("105177", 8) },
        new object[] { new INC(Memory, State), Convert.ToUInt16("005277", 8) },
        new object[] { new INCB(Memory, State), Convert.ToUInt16("105277", 8) },
        new object[] { new DEC(Memory, State), Convert.ToUInt16("005377", 8) },
        new object[] { new DECB(Memory, State), Convert.ToUInt16("105377", 8) },
        new object[] { new NEG(Memory, State), Convert.ToUInt16("005477", 8) },
        new object[] { new NEGB(Memory, State), Convert.ToUInt16("105477", 8) },
        new object[] { new TST(Memory, State), Convert.ToUInt16("005777", 8) },
        new object[] { new TSTB(Memory, State), Convert.ToUInt16("105777", 8) },
        new object[] { new ASR(Memory, State), Convert.ToUInt16("006277", 8) },
        new object[] { new ASRB(Memory, State), Convert.ToUInt16("106277", 8) },
        new object[] { new ASL(Memory, State), Convert.ToUInt16("006377", 8) },
        new object[] { new ASLB(Memory, State), Convert.ToUInt16("106377", 8) },
        new object[] { new ROR(Memory, State), Convert.ToUInt16("006077", 8) },
        new object[] { new RORB(Memory, State), Convert.ToUInt16("106077", 8) },
        new object[] { new ROL(Memory, State), Convert.ToUInt16("006177", 8) },
        new object[] { new ROLB(Memory, State), Convert.ToUInt16("106177", 8) },
        new object[] { new SWAB(Memory, State), Convert.ToUInt16("000377", 8) },
        new object[] { new ADC(Memory, State), Convert.ToUInt16("005577", 8) },
        new object[] { new ADCB(Memory, State), Convert.ToUInt16("105577", 8) },
        new object[] { new SBC(Memory, State), Convert.ToUInt16("005677", 8) },
        new object[] { new SBCB(Memory, State), Convert.ToUInt16("105677", 8) },
        new object[] { new SXT(Memory, State), Convert.ToUInt16("006777", 8) },
        new object[] { new MFPS(Memory, State), Convert.ToUInt16("106777", 8) },
        new object[] { new MTPS(Memory, State), Convert.ToUInt16("106477", 8) },
        new object[] { new MOV(Memory, State), Convert.ToUInt16("017777", 8) },
        new object[] { new MOVB(Memory, State), Convert.ToUInt16("117777", 8) },
        new object[] { new CMP(Memory, State), Convert.ToUInt16("027777", 8) },
        new object[] { new CMPB(Memory, State), Convert.ToUInt16("127777", 8) },
        new object[] { new ADD(Memory, State), Convert.ToUInt16("067777", 8) },
        new object[] { new SUB(Memory, State), Convert.ToUInt16("167777", 8) },
        new object[] { new BIT(Memory, State), Convert.ToUInt16("037777", 8) },
        new object[] { new BITB(Memory, State), Convert.ToUInt16("137777", 8) },
        new object[] { new BIC(Memory, State), Convert.ToUInt16("047777", 8) },
        new object[] { new BICB(Memory, State), Convert.ToUInt16("147777", 8) },
        new object[] { new BIS(Memory, State), Convert.ToUInt16("057777", 8) },
        new object[] { new BISB(Memory, State), Convert.ToUInt16("157777", 8) },
        new object[] { new XOR(Memory, State), Convert.ToUInt16("074777", 8) },
        new object[] { new BR(Memory, State), Convert.ToUInt16("000777", 8) },
        new object[] { new BNE(Memory, State), Convert.ToUInt16("001377", 8) },
        new object[] { new BEQ(Memory, State), Convert.ToUInt16("001777", 8) },
        new object[] { new BPL(Memory, State), Convert.ToUInt16("100377", 8) },
        new object[] { new BMI(Memory, State), Convert.ToUInt16("100777", 8) },
        new object[] { new BVC(Memory, State), Convert.ToUInt16("102377", 8) },
        new object[] { new BVS(Memory, State), Convert.ToUInt16("102777", 8) },
        new object[] { new BCC(Memory, State), Convert.ToUInt16("103377", 8) },
        new object[] { new BCS(Memory, State), Convert.ToUInt16("103777", 8) },
        new object[] { new BGE(Memory, State), Convert.ToUInt16("002377", 8) },
        new object[] { new BLT(Memory, State), Convert.ToUInt16("002777", 8) },
        new object[] { new BGT(Memory, State), Convert.ToUInt16("003377", 8) },
        new object[] { new BLE(Memory, State), Convert.ToUInt16("003777", 8) },
        new object[] { new BHI(Memory, State), Convert.ToUInt16("101377", 8) },
        new object[] { new BLOS(Memory, State), Convert.ToUInt16("101777", 8) },
        new object[] { new JMP(Memory, State), Convert.ToUInt16("000177", 8) },
        new object[] { new JSR(Memory, State), Convert.ToUInt16("004777", 8) },
        new object[] { new RTS(Memory, State), Convert.ToUInt16("000207", 8) },
        new object[] { new MARK(Memory, State), Convert.ToUInt16("006477", 8) },
        new object[] { new SOB(Memory, State), Convert.ToUInt16("077777", 8) },
        new object[] { new EMT(Memory, State), Convert.ToUInt16("104377", 8) },
        new object[] { new TRAP(Memory, State), Convert.ToUInt16("104777", 8) },
        new object[] { new BPT(Memory, State), Convert.ToUInt16("000003", 8) },
        new object[] { new IOT(Memory, State), Convert.ToUInt16("000004", 8) },
        new object[] { new RTI(Memory, State), Convert.ToUInt16("000002", 8) },
        new object[] { new RTT(Memory, State), Convert.ToUInt16("000006", 8) },
        new object[] { new HALT(Memory, State), Convert.ToUInt16("000000", 8) },
        new object[] { new WAIT(Memory, State), Convert.ToUInt16("000001", 8) },
        new object[] { new RESET(Memory, State), Convert.ToUInt16("000005", 8) },
        new object[] { new FlagCommand(Memory, State), Convert.ToUInt16("000277", 8) },
        new object[] { new MUL(Memory, State), Convert.ToUInt16("070777", 8) },
        new object[] { new DIV(Memory, State), Convert.ToUInt16("071777", 8) },
    };
}