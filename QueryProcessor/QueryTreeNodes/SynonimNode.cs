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
            SynonimType = synonimType;
            Name = name;
        }
    }
}
