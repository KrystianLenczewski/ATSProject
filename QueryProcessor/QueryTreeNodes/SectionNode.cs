using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    public class SectionNode : Node
    {
        public List<Node> Childrens { get; set; } = new List<Node>();
    }
}
