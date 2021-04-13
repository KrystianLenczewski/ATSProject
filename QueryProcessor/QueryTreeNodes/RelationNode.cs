using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    public class RelationNode : Node
    {
        public RelationType RelationType { get; set; }
        public List<ArgumentNode> Arguments { get; set; } = new List<ArgumentNode>();

        public RelationNode()
        {
            NodeType = NodeType.RELATION;
        }
    }
}
