using System;
using System.Collections.Generic;
using System.Text;

namespace PKB.Storing
{
    class ProcTableEntry
    {
        public int Index { get; set; }
        public string VariableName { get; set; }
        private static int GlobalIndex = 0;


        public ProcTableEntry(string name)
        {
            Index = GlobalIndex++;
            VariableName = name;
        }
    }
}
