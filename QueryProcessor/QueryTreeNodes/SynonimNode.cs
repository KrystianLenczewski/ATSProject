using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    public class SynonimNode : Node
    {
        public SynonimType SynonimType { get; set; }
        public string Name { get; set; }

        public SynonimNode(SynonimType synonimType, string name)
        {
            NodeType = NodeType.ATTRIBUTE;
            SynonimType = synonimType;
            Name = name;
        }

        internal bool IsStamement()
        {
            bool isConstant = SynonimType == SynonimType.Constant;
            bool isVariable = SynonimType == SynonimType.Variable;
            bool isProcedure = SynonimType == SynonimType.Procedure;

            return !(isConstant || isVariable || isProcedure);
        }
    }
}
