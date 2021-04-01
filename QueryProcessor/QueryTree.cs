using QueryProcessor.Enums;
using QueryProcessor.QueryTreeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor
{
    internal class QueryTree
    {
        private readonly SectionNode _rootNode;

        public QueryTree()
        {

        }

        public List<AttributeNode> GetAttributeNodes()
        {
            SectionNode withNode = GetSectionNode(NodeType.WITH);
            List<Node> attributeNodes = withNode?.Childrens ?? new List<Node>();
            return attributeNodes.Select(s => s as AttributeNode).Where(w => w != null).ToList();
        }

        public List<RelationNode> GetRelationNodes()
        {
            SectionNode suchThatNode = GetSectionNode(NodeType.SUCH_THAT);
            List<Node> relationNodes = suchThatNode?.Childrens ?? new List<Node>();
            return relationNodes.Select(s => s as RelationNode).Where(w => w != null).ToList();
        }

        public List<Node> GetResultNodeChildrens()
        {
            SectionNode resultNode = GetSectionNode(NodeType.RESULT);
            return resultNode?.Childrens ?? new List<Node>();
        }

        private SectionNode GetSectionNode(NodeType nodeType) => _rootNode.Childrens.FirstOrDefault(f => f.NodeType == nodeType) as SectionNode;
    }
}
