using System;
using System.Collections.Generic;
using System.Text;

namespace PKB.Storing
{
    class VarTableEntry
    {
        public int Index { get; set; }
        public string VariableName { get; set; }
        private static int GlobalIndex = 0;


        public VarTableEntry(string name)
        {
            Index = GlobalIndex++;
            VariableName = name;
        }
    }
}
