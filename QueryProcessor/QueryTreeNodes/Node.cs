using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.QueryTreeNodes
{
    internal abstract class Node
    {
        protected NodeType _nodeType;
        public NodeType NodeType { get => _nodeType; protected set { } }
    }
}
