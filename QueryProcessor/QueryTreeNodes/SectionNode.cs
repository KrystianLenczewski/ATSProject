using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    internal class SectionNode : Node
    {
        internal List<Node> Childrens { get; set; } = new List<Node>();
    }
}
