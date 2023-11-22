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
        ushort word;
        ushort addr;
        ushort offset;
        switch (_Mode) {
            case 0:
                return _state.R[_Register];
            case 1:
                return _memory.GetWord(_state.R[_Register]);
            case 2:
                word = _memory.GetWord(_state.R[_Register]);
                _state.R[_Register] += 2;
                return word;
            case 3:
                addr = _memory.GetWord(_state.R[_Register]);
                word = _memory.GetWord(addr);
                _state.R[_Register] += 2;
                return word;
            case 4:
                _state.R[_Register] -= 2;
                return _memory.GetWord(_state.R[_Register]);
            case 5:
                _state.R[_Register] -= 2;
                addr = _memory.GetWord(_state.R[_Register]);
                return _memory.GetWord(_state.R[addr]);
            case 6:
                _state.R[7] += 2;
                offset = _memory.GetWord(_state.R[7]);
                return _memory.GetWord((ushort)(_state.R[_Register] + offset));
            case 7:
                _state.R[7] += 2;
                offset = _memory.GetWord(_state.R[7]);
                addr = _memory.GetWord((ushort)(_state.R[_Register] + offset));
                return _memory.GetWord(_state.R[addr]);

        }
        throw new InvalidOperationException("Invalid Mode!");
    }
    public void SetValue(ushort word) {
        ushort addr;
        ushort offset;
        switch (_Mode)
        {
            case 0:
                _state.R[_Register] = word;
                return;
            case 1:
                _memory.SetWord(_state.R[_Register], word);
                return;
            case 2:
                _memory.SetWord(_state.R[_Register], word);
                _state.R[_Register] += 2;
                return;
            case 3:
                addr = _memory.GetWord(_state.R[_Register]);
                _memory.SetWord(addr, word);
                _state.R[_Register] += 2;
                return;
            case 4:
                _state.R[_Register] -= 2;
                _memory.SetWord(_state.R[_Register], word);
                return;
            case 5:
                _state.R[_Register] -= 2;
                addr = _memory.GetWord(_state.R[_Register]);
                _memory.SetWord(_state.R[addr], word);
                return;
            case 6:
                _state.R[7] += 2;
                offset = _memory.GetWord(_state.R[7]);
                _memory.SetWord((ushort)(_state.R[_Register] + offset), word);
                return;
            case 7:
                _state.R[7] += 2;
                offset = _memory.GetWord(_state.R[7]);
                addr = _memory.GetWord((ushort)(_state.R[_Register] + offset));
                _memory.SetWord(_state.R[addr], word);
                return;

        }
        throw new InvalidOperationException("Invalid Mode!");
    }
}