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

            if (relationArgumentType == RelationArgumentType.Integer || relationArgumentType == RelationArgumentType.String)
                Value = name;
        }

        public bool IsStatement()
        {
            bool isDiscard = RelationArgumentType == RelationArgumentType.Discard;
            bool isConstant = RelationArgumentType == RelationArgumentType.Constant;
            //bool isInteger = RelationArgumentType == RelationArgumentType.Integer;
            bool isString = RelationArgumentType == RelationArgumentType.String;
            bool isProgLine = RelationArgumentType == RelationArgumentType.Prog_line;
            bool isVariable = RelationArgumentType == RelationArgumentType.Variable;
            bool isProcedure = RelationArgumentType == RelationArgumentType.Procedure;

            return !(isDiscard || isConstant || isString || isProgLine || isVariable || isProcedure);
        }


    }
}
