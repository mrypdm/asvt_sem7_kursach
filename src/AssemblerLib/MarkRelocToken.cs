using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal class MarkRelocToken : IToken
    {
        private string _mark;
        private int _addValue;
        // true -> +; false -> -
        private bool _opSign;

        public MarkRelocToken(string mark, int addValue, bool opSign)
        {
            _mark = mark;
            _addValue = addValue;
            _opSign = opSign;
        }

        public List<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
        {
            if (!marksDict.ContainsKey(_mark))
            {
                throw new Exception($"The mark ({_mark}) is not determined.");
            }

            string word;
            if (_addValue != 0)
            {
                if (_opSign)
                {
                    word = Convert.ToString(marksDict[_mark] + _addValue, 8).PadLeft(6, '0') + "\'";
                }
                else
                {
                    word = Convert.ToString(marksDict[_mark] - _addValue, 8).PadLeft(6, '0') + "\'";
                }

                return new List<string>() { word };
            }

            word = Convert.ToString(marksDict[_mark], 8).PadLeft(6, '0') + "\'";
            return new List<string>() { word };
        }
    }
}
