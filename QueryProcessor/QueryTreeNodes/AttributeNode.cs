using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    public class AttributeNode : Node
    {
        public string AttributeType { get; private set; }
        public object AttributeValue { get; set; }
        public SynonimNode SynonimNode { get; set; }

        public AttributeNode(string attributeType, string attributeValue, SynonimNode synonimNode) : this(attributeType, synonimNode)
        {
            AttributeValue = attributeValue;
        }

        public AttributeNode(string attributeType, SynonimNode synonimNode)
        {
            InitializeNodeState(attributeType, synonimNode);
        }
        public AttributeNode(string attributeType, AttributeNode attributeValue, SynonimNode synonimNode) : this(attributeType, synonimNode)
        {
            AttributeValue = attributeValue;
        }

        private void InitializeNodeState(string attributeType, SynonimNode synonimNode)
        {
            NodeType = NodeType.ATTRIBUTE;
            AttributeType = attributeType;
            SynonimNode = synonimNode;
        }
    }
}
