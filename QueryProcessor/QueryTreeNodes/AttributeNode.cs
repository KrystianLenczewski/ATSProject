using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    public class AttributeNode : Node
    {
        public string AttributeType { get; set; }
        public string AttributeValue { get; set; }
        public SynonimNode SynonimNode { get; set; }

        public AttributeNode(string attributeType, string attributeValue, SynonimNode synonimNode)
        {
            NodeType = NodeType.ATTRIBUTE;
            AttributeType = attributeType;
            AttributeValue = attributeValue;
            SynonimNode = synonimNode;
        }
    }
}
