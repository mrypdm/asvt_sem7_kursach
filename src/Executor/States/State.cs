namespace Executor.States;

public class State : IState
{
    private ushort _processorStateWord;

    public void SetFlag(Flag flag, bool val)
    {
        var value = val ? 1 : 0;

        switch (flag)
        {
            case Flag.Z:
                _processorStateWord &= 0b1111_1111_1111_1011;
                _processorStateWord |= (ushort)(value << 2);
                break;
            case Flag.N:
                _processorStateWord &= 0b1111_1111_1111_0111;
                _processorStateWord |= (ushort)(value << 3);
                break;
            case Flag.V:
                _processorStateWord &= 0b1111_1111_1111_1101;
                _processorStateWord |= (ushort)(value << 1);
                break;
            case Flag.C:
                _processorStateWord &= 0b1111_1111_1111_1110;
                _processorStateWord |= (ushort)value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(flag), flag, null);
        }

        Console.WriteLine($"PSW {_processorStateWord}.");
    }

    public bool GetFlag(Flag flag)
    {
        return flag switch
        {
            Flag.Z => (_processorStateWord & 0b100) >> 2 == 1,
            Flag.N => (_processorStateWord & 0b1000) >> 3 == 1,
            Flag.V => (_processorStateWord & 0b10) >> 1 == 1,
            Flag.C => (_processorStateWord & 0b1) == 1,
            _ => throw new ArgumentOutOfRangeException(nameof(flag), flag, null)
        };
    }

    public ushort[] Registers { get; } = new ushort[8];
}