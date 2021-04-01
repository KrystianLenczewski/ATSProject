using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    internal class SynonimNode : Node
    {
        public SynonimType SynonimType { get; set; }
        public string Name { get; set; }
    }
}
