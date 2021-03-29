using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor
{
    class RelTableItem
    {
        private static int GlobalIndex = 0;
        public RelTableItem()
        {

        }
        public RelTableItem(string relationship, int arguments, List<RelationArgumentType> argument1, List<RelationArgumentType> argument2)
        {
            Index = GlobalIndex++;
            Relationship = relationship;
            NumberOfArguments = arguments;
            TypeOfArgument_1 = argument1;
            TypeOfArgument_2 = argument2;

        }
        public int Index { get; set; }
        public string Relationship { get; set; }
        public int NumberOfArguments { get; set; }
        public List<RelationArgumentType> TypeOfArgument_1 { get; set; }
        public List<RelationArgumentType> TypeOfArgument_2 { get; set; }
    }
}
