using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    internal class RelationNode : Node
    {
        internal RelationType RelationType { get; set; }
        internal List<Node> Arguments { get; set; } = new List<Node>();
    }
}
