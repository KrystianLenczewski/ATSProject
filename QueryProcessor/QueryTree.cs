using QueryProcessor.Enums;
using QueryProcessor.QueryTreeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor
{
    public class QueryTree
    {
        private readonly SectionNode _rootNode = new SectionNode();

        public QueryTree()
        {

        }

        public void AddSuchThatNode(SectionNode suchThatNode)
        {
            if (suchThatNode == null) return;
            Node existingResultNode = _rootNode.Childrens.FirstOrDefault(f => f.NodeType == NodeType.SUCH_THAT);
            if (existingResultNode != null)
            {
                _rootNode.Childrens.Remove(existingResultNode);
            }

            suchThatNode.NodeType = NodeType.SUCH_THAT;
            _rootNode.Childrens.Add(suchThatNode);
        }

        public void AddResultNode(SectionNode resultNode)
        {
            if (resultNode == null) return;
            Node existingResultNode = _rootNode.Childrens.FirstOrDefault(f => f.NodeType == NodeType.RESULT);
            if (existingResultNode != null)
            {
                _rootNode.Childrens.Remove(existingResultNode);
            }

            resultNode.NodeType = NodeType.RESULT;
            _rootNode.Childrens.Add(resultNode);
        }

        public void AddWithNode(SectionNode withNode)
        {
            if (withNode == null) return;
            Node existingWithNode = _rootNode.Childrens.FirstOrDefault(f => f.NodeType == NodeType.WITH);
            if (existingWithNode != null)
            {
                _rootNode.Childrens.Remove(existingWithNode);
            }

            withNode.NodeType = NodeType.WITH;
            _rootNode.Childrens.Add(withNode);
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

        public List<SynonimNode> GetResultNodeChildrens()
        {
            SectionNode resultNode = GetSectionNode(NodeType.RESULT);
            return resultNode?.Childrens.Select(s=>s as SynonimNode).ToList() ?? new List<SynonimNode>();
        }

        public bool IsResultBoolean()
        {
            List<SynonimNode> resultNodes = GetResultNodeChildrens();
            foreach (var item in resultNodes)
            {
                if (item.SynonimType==SynonimType.BOOLEAN) return true;

            }
            return false;
        }

        private SectionNode GetSectionNode(NodeType nodeType) => _rootNode.Childrens.FirstOrDefault(f => f.NodeType == nodeType) as SectionNode;
   
    }
}
