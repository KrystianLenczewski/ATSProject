using QueryProcessor.Enums;
using QueryProcessor.Infrastructure;
using QueryProcessor.QueryTreeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor
{
    public class QueryPreprocessor
    {
        private readonly QueryValidator _queryValidator = new QueryValidator();
        private RelTable.RelTable _relTable = new RelTable.RelTable();
        private SymbolTable _symbolTable;

        public QueryTree ParseQuery(string query)
        {
            _queryValidator.ValidateQuery(query, out List<string> validationErrors);
            _symbolTable = new SymbolTable(query);
            QueryTree queryTree = new QueryTree();
            queryTree.AddResultNode(ExtractResult(query));
            queryTree.AddSuchThatNode(ExtractSuchThatElements(query));

            return queryTree;
        }

        internal SectionNode ExtractSuchThatElements(string query)
        {
            List<string> splitedQuery = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            splitedQuery.ForEach(x => x.Trim().ToLower());
            int suchThatIndex = GetIndexForSuchThat(splitedQuery);

            if(suchThatIndex != -1)
            {
                SectionNode relationSectionNode = new SectionNode() { NodeType = NodeType.SUCH_THAT };
                int endSuchThat = GetSuchThatEndIndex(splitedQuery);
                for (int i = suchThatIndex + 1; i < endSuchThat; i += 2)
                {
                    RelationType relationType = _relTable.GetRelationType(splitedQuery[i]);
                    RelationNode relationNode = new RelationNode() { RelationType = relationType };
                    relationSectionNode.Childrens.Add(relationNode);
                    List<string> relationArguments = splitedQuery[i + 1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                    foreach (string relationArgument in relationArguments)
                    {
                        string relationArgumentName = relationArgument.Replace("(", "").Replace(")", "");
                        RelationArgumentType relationArgumentType = _symbolTable.GetRelationArgumentType(relationArgumentName);
                        ArgumentNode argumentNode = new ArgumentNode(relationArgumentType, relationArgumentName);
                        relationNode.Arguments.Add(argumentNode);
                    }
                }
                return relationSectionNode;
            }

            return null;
        }

        internal SectionNode ExtractResult(string query)
        {
            List<string> splitedQuery = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            splitedQuery.ForEach(x => x.Trim().ToLower());
            int selectIndex = splitedQuery.IndexOf(QueryElement.Select.ToLower());

            List<string> resultSymbols = splitedQuery[selectIndex + 1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            SectionNode resultNode = new SectionNode { NodeType = NodeType.RESULT };
            foreach (var symbolName in resultSymbols)
            {
                SynonimType synonimType = _symbolTable.GetSynonimType(symbolName);
                resultNode.Childrens.Add(new SynonimNode(synonimType, symbolName) { NodeType = NodeType.RESULT });
            }

            return resultNode;
        }

        private int GetIndexForSuchThat(List<string> splittedQuery)
        {
            int suchIndex = splittedQuery.IndexOf("such");
            int thatIndex = splittedQuery.IndexOf("that");

            if (suchIndex + 1 == thatIndex)
                return thatIndex;
            return -1;
        }

        private int GetSuchThatEndIndex(List<string> splittedQuery)
        {
            int withIndex = splittedQuery.IndexOf(QueryElement.With.ToLower());
            int patternIndex = splittedQuery.IndexOf(QueryElement.Pattern.ToLower());

            int endSuchThat = withIndex == -1 ? patternIndex : withIndex;
            endSuchThat = endSuchThat == -1 ? splittedQuery.Count : endSuchThat;
            return endSuchThat;
        }
    }
}
