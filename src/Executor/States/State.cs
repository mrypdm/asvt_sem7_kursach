namespace Executor.States;

public class State : IState
{
    public ushort ProcessorStateWord { get; set; }

    public bool C
    {
        get => (ProcessorStateWord & 1) != 0;
        set => ProcessorStateWord = (ushort)((ProcessorStateWord & 0b1111_1111_1111_1110) | (value ? 1 : 0));
    }

    public bool V
    {
        get => (ProcessorStateWord & 2) != 0;
        set => ProcessorStateWord = (ushort)((ProcessorStateWord & 0b1111_1111_1111_1101) | ((value ? 1 : 0) << 1));
    }

    public bool Z
    {
        get => (ProcessorStateWord & 4) != 0;
        set => ProcessorStateWord = (ushort)((ProcessorStateWord & 0b1111_1111_1111_1011) | ((value ? 1 : 0) << 2));
    }

    public bool N
    {
        get => (ProcessorStateWord & 8) != 0;
        set => ProcessorStateWord = (ushort)((ProcessorStateWord & 0b1111_1111_1111_0111) | ((value ? 1 : 0) << 3));
    }

    public bool T => (ProcessorStateWord & 16) != 0;

    public int Priority
    {
        get => (ProcessorStateWord & 0xE0) >> 5;
        set => ProcessorStateWord = (ushort)((ProcessorStateWord & 0b1111_1111_0001_1111) | (value << 5));
    }

    public ushort[] Registers { get; set; } = new ushort[8];
}