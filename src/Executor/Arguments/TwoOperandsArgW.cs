namespace Executor;

public class TwoOperandsArgW: IArgument {
    private ushort _Mode;
    private ushort _Register;
    private State _state;
    private Memory _memory;
    public TwoOperandsArgW(ushort Mode, ushort Register, State state, Memory memory) {
      this._memory = memory;
      this._state = state;
      this._Register = Register;
      this._Mode = Mode;
    }

    public ushort GetValue() {
        switch (_Mode) {
            case 0:
                return _state.R[_Register];
            case 1:
                return _memory.GetWord(_state.R[_Register]);
            case 2:
                ushort word = _memory.GetWord(_state.R[_Register]);
                _state.R[_Register] += 2;
                return word;
            case 3:
                ushort addr = _memory.GetWord(_state.R[_Register]);
                ushort word = _memory.GetWord(addr);
                _state.R[_Register] += 2;
                return word;
            case 4:
                _state.R[_Register] -= 2;
                return _memory.GetWord(_state.R[_Register]);
            case 5:
                _state.R[_Register] -= 2;
                ushort addr = _memory.GetWord(_state.R[_Register]);
                return _memory.GetWord(_state[addr]);
            case 6:
                _state.R[7] += 2;
                ushort offset = _memory.GetWord(_state.R[7]);
                return _memory.GetWord(_state.R[_Register] + offset);
            case 7:
                _state.R[7] += 2;
                ushort offset = _memory.GetWord(_state.R[7]);
                ushort addr = _memory.GetWord(_state.R[_Register] + offset);
                return _memory.GetWord(_state.R[addr]);

        }
    }
    public void SetValue(ushort word) {
        switch (_Mode)
        {
            case 0:
                _state.R[_Register] = word;
            case 1:
                _memory.SetWord(_state.R[_Register], word);
            case 2:
                _memory.SetWord(_state.R[_Register], word);
                _state.R[_Register] += 2;
            case 3:
                ushort addr = _memory.GetWord(_state.R[_Register]);
                _memory.SetWord(addr, word);
                _state.R[_Register] += 2;
            case 4:
                _state.R[_Register] -= 2;
                _memory.SetWord(_state.R[_Register], word);
            case 5:
                _state.R[_Register] -= 2;
                ushort addr = _memory.GetWord(_state.R[_Register]);
                _memory.SetWord(_state[addr], word);
            case 6:
                _state.R[7] += 2;
                ushort offset = _memory.GetWord(_state.R[7]);
                _memory.SetWord(_state.R[_Register] + offset, word);
            case 7:
                _state.R[7] += 2;
                ushort offset = _memory.GetWord(_state.R[7]);
                ushort addr = _memory.GetWord(_state.R[_Register] + offset);
                _memory.SetWord(_state.R[addr], word);

        }
    }
}