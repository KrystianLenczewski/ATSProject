using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    public class AttributeNode : Node
    {
        public AttributeType AttributeType { get; set; }
        public object AttributeValue { get; set; }
        public SynonimNode SynonimNode { get; set; }
    }
}
