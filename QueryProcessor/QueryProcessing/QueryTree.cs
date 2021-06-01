using QueryProcessor.Enums;
using QueryProcessor.QueryTreeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor.QueryProcessing
{
    public class QueryTree
    {
        private readonly Dictionary<string, RelationArgumentType> _declarations;
        private readonly SectionNode _rootNode = new SectionNode();

        public QueryTree(Dictionary<string, RelationArgumentType> declarations)
        {
            _declarations = declarations;
        }

        public Dictionary<string, RelationArgumentType> GetDeclarations() => _declarations;

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
            return resultNode?.Childrens.Select(s => s as SynonimNode).ToList() ?? new List<SynonimNode>();
        }

        public bool IsResultBoolean()
        {
            foreach (var resultNode in GetResultNodeChildrens())
            {
                if (resultNode.SynonimType == SynonimType.BOOLEAN)
                    return true;
            }
            return false;
        }

        public List<AttributeNode> GetRelatedAttributeNodes()
        {
            SectionNode withNode = GetSectionNode(NodeType.WITH);
            List<AttributeNode> attributeNodes = withNode?.Childrens?.Select(s => s as AttributeNode)?.Where(w => w != null)?.ToList();
            return attributeNodes.Where(w => w.AttributeValue is AttributeNode).ToList();
        }

        public void PrepareTreeForQueryEvaluator()
        {
            MoveWithInfoToRelationArguments();
        }

        private void MoveWithInfoToRelationArguments()
        {
            SectionNode withNode = GetSectionNode(NodeType.WITH);
            if (withNode != null)
            {
                List<AttributeNode> attributeNodes = withNode?.Childrens?.Select(s => s as AttributeNode)?.Where(w => w != null)?.ToList();
                foreach (AttributeNode attributeNode in attributeNodes)
                    AddInfoFromWithToRelationsArguments(attributeNode);
            }
        }

        private void AddInfoFromWithToRelationsArguments(AttributeNode attributeNode)
        {
            if (!(attributeNode.AttributeValue is AttributeNode))
            {
                string attributeSynonimName = attributeNode.SynonimNode.Name;
                foreach (RelationNode relationNode in GetRelationNodes())
                {
                    foreach (ArgumentNode relationArgument in relationNode.Arguments)
                    {
                        if (relationArgument.Name == attributeSynonimName)
                            relationArgument.Value = attributeNode.AttributeValue?.ToString();
                    }
                }
            }
        }

        private SectionNode GetSectionNode(NodeType nodeType) => _rootNode.Childrens.FirstOrDefault(f => f.NodeType == nodeType) as SectionNode;

    }
}
