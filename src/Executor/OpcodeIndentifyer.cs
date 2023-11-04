using System;

namespace Executor{
    public class OpcodeIndentifyer {
        private OpcodeIndentifyer Instance;
        private ushort[] Masks = {65472, 61440, 65024, 65024, 65535, 65520, 65280, 65528, 65528, 65472};
        
        public OpcodeIndentifyer GetInstance(){
            if (Instance == null){
                Instance = new OpcodeIndentifyer();
            }
            return Instance;
        }
        private OpcodeIndentifyer(){
            
        }
        public string GetCommandName(ushort Word){
            throw new InvalidOperationException("Invalid Operation!");
        }
    }

}
