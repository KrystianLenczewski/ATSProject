using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public abstract class Node
    {
        public string Name;
        public bool IsRoot { get; set; }
        public abstract void SetFirstChild(Node p);
        public abstract void setRightSibling(Node l);
        public abstract void setChildOfLink(Node c);
        public abstract void setLeftSibling(Node l);
        public abstract EntityType GetNodeType();
        public abstract string getAttribute();
        public abstract Node getFirstChild();
        public abstract Node getRightSibling();
        public abstract Node getChildOfLink();
        public abstract Node getLeftSibling();
    }
}
