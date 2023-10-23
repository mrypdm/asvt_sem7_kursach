using System;

namespace OpcodeFiller{
    public void OneOperandFill(Dictionary<string, string> OneOperand){
        OneOperand.Add("mask", "65472");  // 0b1111_1111_1100_0000
        OneOperand.Add("1050", "CLRB");
        OneOperand.Add("1051", "COMB");
        OneOperand.Add("1052", "INCB");
        OneOperand.Add("1053", "DECB");
        OneOperand.Add("1054", "NEGB");
        OneOperand.Add("1055", "ADCB");
        OneOperand.Add("1056", "SBCB");
        OneOperand.Add("1057", "TSTB");
        OneOperand.Add("1060", "RORB");
        OneOperand.Add("1061", "ROLB");
        OneOperand.Add("1062", "ASRB");
        OneOperand.Add("1063", "ASLB");
        OneOperand.Add("1064", "MTPS");
        OneOperand.Add("1065", "MFPD");
        OneOperand.Add("1066", "MTPD");
        OneOperand.Add("1067", "MFPS");
        OneOperand.Add("0001", "JMP");
        OneOperand.Add("0003", "SWAB");
        OneOperand.Add("0050", "CLR");
        OneOperand.Add("0051", "COM");
        OneOperand.Add("0052", "INC");
        OneOperand.Add("0053", "DEC");
        OneOperand.Add("0054", "NEG");
        OneOperand.Add("0055", "ADC");
        OneOperand.Add("0056", "SBC");
        OneOperand.Add("0057", "TST");
        OneOperand.Add("0060", "ROR");
        OneOperand.Add("0061", "ROL");
        OneOperand.Add("0062", "ASR");
        OneOperand.Add("0063", "ASL");
        OneOperand.Add("0065", "MFPI");
        OneOperand.Add("0066", "MTPI");
        OneOperand.Add("0067", "SXT");
    }

    public void TwoOperandsFill(Dictionary<string, string> TwoOperands){
        TwoOperands.Add("mask", "61440"); // 0b1111_0000_0000_0000
        TwoOperands.Add("11", "MOVB");
        TwoOperands.Add("12", "CMPB");
        TwoOperands.Add("13", "BITB");
        TwoOperands.Add("14", "BICB");
        TwoOperands.Add("15", "BISB");
        TwoOperands.Add("16", "SUB");
        TwoOperands.Add("01", "MOV");
        TwoOperands.Add("02", "CMP");
        TwoOperands.Add("03", "BIT");
        TwoOperands.Add("04", "BIC");
        TwoOperands.Add("05", "BIS");
        TwoOperands.Add("06", "ADD");
    }

    public void BranchOperationC0Fill(Dictionary<string, string> BranchOperationC0){
        BranchOperationC0.Add("mask", "65024"); // 0b1111_1110_0000_0000
        BranchOperationC0.Add("100", "BPL");
        BranchOperationC0.Add("101", "BHI");
        BranchOperationC0.Add("102", "BVC");
        BranchOperationC0.Add("103", "BCC");
        BranchOperationC0.Add("001", "BNE");
        BranchOperationC0.Add("002", "BGE");
        BranchOperationC0.Add("003", "BGT");
    }

    public void BranchOperationC1Fill(Dictionary<string, string> BranchOperationC1){
        BranchOperationC1.Add("mask", "65024"); // 0b1111_1110_0000_0000
        BranchOperationC1.Add("100", "BMI");
        BranchOperationC1.Add("101", "BLOS");
        BranchOperationC1.Add("102", "BVS");
        BranchOperationC1.Add("103", "BCS");
        BranchOperationC1.Add("000", "BR");
        BranchOperationC1.Add("001", "BEQ");
        BranchOperationC1.Add("002", "BLT");
        BranchOperationC1.Add("003", "BLE");

        BranchOperationC1.Add("077", "SOB"); // Subroutine Instructions
        BranchOperationC1.Add("004", "JSR");
    }

    public void TrapFill(Dictionary<string, string> Trap){
        Trap.Add("mask", "65535"); // 0b1111111111111111
        Trap.Add("000004", "RTI");
        Trap.Add("000003", "BPT");
        Trap.Add("000004", "IOT");  
        Trap.Add("000006", "RTT");
        Trap.Add("000000", "HALT");
        Trap.Add("000001", "WAIT");
        Trap.Add("000005", "RESET");
        Trap.Add("000004", "ILLEGAL_INSTRUCTION"); 
        Trap.Add("000010", "RESERVED"); 
        Trap.Add("000014", "TRACE_TRAP");       
        Trap.Add("000020", "IOT_INSTRUCTION"); 
        Trap.Add("000024", "POWER_FAIL"); 
        Trap.Add("000030", "EMT_INSTRUCTION"); 
        Trap.Add("000034", "TRAP_INSTRUCTION");       
        Trap.Add("000114", "PARITY_ERROR"); 
        Trap.Add("000244", "FLOAT_EXCEPTION"); 
        Trap.Add("000250", "MMU_FAULT"); 
    }

    public void ConditionCodeFill(Dictionary<string, string> ConditionInstruction){
        ConditionInstruction.Add("mask", "65520"); // Надо проверить // 0b1111_1111_1111_0000
        ConditionInstruction.Add("00202", "CCC");
        ConditionInstruction.Add("00203", "SCC");
    }

    public void TrapInstructionsFill(Dictionary<string, string> TrapInstructions){
        TrapInstructions.Add("mask", "65280"); // 0b1111_1111_1000_0000
        TrapInstructions.Add("0210", "EMT");
        TrapInstructions.Add("0211", "TRAP");
    }

    public void FloatingInstructionSetFill(Dictionary<string, string> FloatingInstructionSet){
        FloatingInstructionSet.Add("mask", "65528"); // 0b1111_1111_1111_1000
        FloatingInstructionSet.Add("07500", "FADD");
        FloatingInstructionSet.Add("07501", "FSUB");
        FloatingInstructionSet.Add("07502", "FMUl");
        FloatingInstructionSet.Add("07503", "FDIV");
    }

    public void SubroutineInstruction0Fill(Dictionary<string, string> SubroutineInstruction0){
        SubroutineInstruction0.Add("mask", "65528"); // 0b1111_1111_1111_1000
        SubroutineInstruction0.Add("00020", "RTS");
    }
    public void SubroutineInstruction1Fill(Dictionary<string, string> SubroutineInstruction1){
        SubroutineInstruction1.Add("mask", "65472"); // 0b1111_1111_1100_0000 
        SubroutineInstruction1.Add("0064", "MARK");
    }



}