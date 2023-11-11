namespace Executor;

class SOBArg: IArgument {

    private ushort _Offset;
    private ushort _Register;
    private State _state;
    private Memory _memory;
    public ushort GetValue() {
      return 0;
    }
    public void SetValue(ushort word) {
      return;
    }
    public SOBArg(ushort Register, ushort Offset, State state, Memory memory) {
      this._memory = memory;
      this._state = state;
      this._Register = Register;
      this._Offset = Offset;
    }
  }
