using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    internal class AttributeNode : Node
    {
        internal AttributeType AttributeType { get; set; }
        internal object AttributeValue { get; set; }
        internal SynonimNode SynonimNode { get; set; }
    }
}
