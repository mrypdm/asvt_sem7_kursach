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
        private string _numOct;

        public MarkRelocToken(string mark, string numOct)
        {
            _mark = mark;
            _numOct = numOct;
        }

        private int OctToDec(string numOct)
        {
            int result = 0;

            for (int i = 0; i < numOct.Length; i++)
            {
                var n1 = Convert.ToInt32(numOct[i]) - '0';
                var n2 = Convert.ToInt32(Math.Pow(8, numOct.Length - 1 - i));
                result +=  n1*n2 ;
            }

            return result;
        }

        public List<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
        {
            if (!marksDict.ContainsKey(_mark))
            {
                throw new Exception($"The mark ({_mark}) is not determined.");
            }

            int numDec;
            string word;
            if (!String.IsNullOrWhiteSpace(_numOct))
            {
                numDec = OctToDec(_numOct.Substring(1));
                if (_numOct.StartsWith('+'))
                {
                    word = Convert.ToString(marksDict[_mark] + numDec, 8).PadLeft(6, '0') + "\'";
                }
                else if (_numOct.StartsWith('-'))
                {
                     word = Convert.ToString(marksDict[_mark] - numDec, 8).PadLeft(6, '0') + "\'";
                }
                else
                {
                    throw new Exception($"Invalid argument: {_mark}{_numOct}");
                }

                return new List<string>() { word };
            }

            word = Convert.ToString(marksDict[_mark]).PadLeft(6, '0') + "\'";
            return new List<string>() { word };
        }
    }
}
