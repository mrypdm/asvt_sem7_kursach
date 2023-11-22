using Executor.States;
using Executor.Memories
using Executor.CommandTypes

namespace Executor;

public class Executor
{
    private IState _state;
    private IMemory _memory;

    public Executor()
    {
        _state = new State();
        _memory = new Memory();
        _OpId = new OpcodeIdentifier(_state, _memory);
    }

    public int ExecuteProgram()
    {
        return 0;
    }

    public int ExecuteNextInstruction()
    {
        var word = _memory.GetWord(_state.Registers[7]);
        _state.Registers += 2;
        var command = _OpId.GetCommand(word);
        command.Execute(command.GetArguments(word)); 
        return 0;
    }

    public int LoadProgram(string filename)
    {
        return 0;
    }
}