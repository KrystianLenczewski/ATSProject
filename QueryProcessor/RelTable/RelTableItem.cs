using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.RelTable
{
    internal class RelTableItem
    {
        private static int GlobalIndex = 0;

        public int Index { get; set; }
        public RelationType RelationType { get; set; }
        public int ArgumentsCount { get; set; }
        public List<RelationArgumentType> Argument1Types { get; set; }
        public List<RelationArgumentType> Argument2Types { get; set; }

        public RelTableItem(RelationType relationType, int argumentsCount, List<RelationArgumentType> argument1Types, List<RelationArgumentType> argument2Types)
        {
            Index = GlobalIndex++;
            RelationType = relationType;
            ArgumentsCount = argumentsCount;
            Argument1Types = argument1Types;
            Argument2Types = argument2Types;
        }
    }
}
