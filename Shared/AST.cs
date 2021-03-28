using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    class AST
    {

        private List<Node> Nodes { get; set; }
        private Node rootNode { get; set; }
        public static Node CreateEntityType(EntityType entityType)
        {
            throw new NotImplementedException();
        }

        public static void SetRoot(Node node) => node.IsRoot = true;
        //  void setAttribute(Node n, ATTR attr)
        void SetFirstChild(Node p, Node c)
        {
            p.SetFirstChild(c);
        }

        void setRightSibling(Node l, Node r)
        {
            l.setRightSibling(r);
        }
        void setChildOfLink(Node c, Node p)
        {
            c.setChildOfLink(p);
        }
        void setLeftSibling(Node l, Node r)
        {
            l.setLeftSibling(r);
        }

        //gettery
        Node getRoot()
        {
            return rootNode;
        }

        EntityType getType(Node node)
        {
            return node.GetNodeType();
        }
        string getAttribute(Node node)
        {
            return node.getAttribute();
        }

        Node getFirstChild(Node p)
        {
            return p.getFirstChild();
        }

        Node getRightSibling(Node p)
        {
            return p.getRightSibling();
        }

        Node getChildOfLink(Node p)
        {
            return p.getChildOfLink();
        }

        Node getLeftSibling(Node p)
        {
            return p.getLeftSibling();
        }

    }
}
