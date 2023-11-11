namespace Executor;

public class OneOperandArg: IArgument {

    private ushort _Mode;
    private ushort _Register;
    private State _state;
    private Memory _memory;
    public ushort GetValue() {
      return 0;
    }
    public void SetValue(ushort word) {
      return;
    }
    public OneOperandArg(ushort Mode, ushort Register, State state, Memory memory) {
      this._memory = memory;
      this._state = state;
      this._Register = Register;
      this._Mode = Mode;
    }
}