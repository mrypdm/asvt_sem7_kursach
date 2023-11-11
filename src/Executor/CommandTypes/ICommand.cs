namespace Executor;

public interface ICommand {
    void Execute(IArgument[] arguments);

    IArgument[] GetArguments(ushort word);

    ushort Opcode {
      get;
    }
}