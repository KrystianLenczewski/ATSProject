using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    public class ArgumentNode : Node
    {
        public RelationArgumentType RelationArgumentType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public ArgumentNode(RelationArgumentType relationArgumentType, string name)
        {
            NodeType = NodeType.ARGUMENT;
            RelationArgumentType = relationArgumentType;
            Name = name;

            if(relationArgumentType == RelationArgumentType.Integer || relationArgumentType == RelationArgumentType.String)
                Value = name;
        }
    }
}
