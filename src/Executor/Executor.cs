using Executor.States;

namespace Executor;

public class Executor
{
    private readonly IState _state;

    public Executor()
    {
        _state = new State();
    }

    public int ExecuteProgram()
    {
        return 0;
    }

    private int ReadCommand(ushort addr)
    {
        return 0;
    }

    public int ExecuteNextInstruction()
    {
        _state.Registers[7] += 2;
        ReadCommand(_state.Registers[7]);
        return 0;
    }

    public int LoadProgram(string filename)
    {
        return 0;
    }
}