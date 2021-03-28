using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKB.Storing
{
    class VarTable
    {

        private List<VarTableEntry> varTableEntries = new List<VarTableEntry>();

        int insertVar(string varName)
        {
            var entry = varTableEntries.FirstOrDefault(f => f.VariableName == varName);
            if(entry==null)
            {
                varTableEntries.Add(new VarTableEntry(varName));
            }
            return entry == null ? varTableEntries.Last().Index : entry.Index;
        }
        string getVarName(int index)
        {
            var entry= varTableEntries.FirstOrDefault(f => f.Index==index);
            if (entry is null) throw new IndexOutOfRangeException();
            else return entry.VariableName;
        }
        int getVarIndex(string name)
        {
            var entry = varTableEntries.FirstOrDefault(f => f.VariableName == name);
            if (entry is null) throw new IndexOutOfRangeException();
            else return entry.Index;
        }
        int getSize()
        {
            return varTableEntries.Count;
        }
        bool isIn(string varName)
        {
            return varTableEntries.Any(a => a.VariableName == varName);
        }
    }
}
