using QueryProcessor.Enums;
using QueryProcessor.Infrastructure;
using QueryProcessor.QueryTreeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor.QueryProcessing
{
    public class QueryPreprocessor
    {
        private readonly QueryValidator _queryValidator = new QueryValidator();
        private RelTable.RelTable _relTable = new RelTable.RelTable();
        private SymbolTable _symbolTable;
        private List<string> validationErrors = new List<string>();

        public QueryTree ParseQuery(string query)
        {
            if (_queryValidator.ValidateQuery(query, out List<string> validationErrors))
            {
                _symbolTable = new SymbolTable(query);
                QueryTree queryTree = new QueryTree();
                queryTree.AddResultNode(ExtractResult(query));
                queryTree.AddSuchThatNode(ExtractSuchThatElements(query));
                queryTree.AddWithNode(ExtractWith(query));
                return queryTree;
            }
            else
            {
                this.validationErrors = validationErrors;
                throw new Exception("Zapytanie PQL jest nieprawidłowe.");
            }
                
        }

        public List<string> GetValidationErrors()
        {
            return validationErrors;
        }

        private SectionNode ExtractSuchThatElements(string query)
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

        private SectionNode ExtractResult(string query)
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

        private SectionNode ExtractWith(string query)
        {
            List<string> splitedQuery = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            splitedQuery.ForEach(x => x.Trim().ToLower());
            int withIndex = splitedQuery.IndexOf(QueryElement.With.ToLower());
            int patternIndex = splitedQuery.IndexOf(QueryElement.Pattern.ToLower());
            int endWith = patternIndex == -1 ? splitedQuery.Count : patternIndex;

            if (withIndex + 1 != endWith && withIndex!=-1)
            {
                string withCondition = splitedQuery[withIndex + 1];
                List<string> splittedWithCondition = withCondition.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
                string synonimName = splittedWithCondition?.ElementAtOrDefault(0);//s2
                string[] attributeWithValue = splittedWithCondition?.ElementAtOrDefault(1)?.Split('=', StringSplitOptions.RemoveEmptyEntries);//stmt#, 5
                SectionNode withNode = new SectionNode { NodeType = NodeType.WITH };
                SynonimNode synonimNode = new SynonimNode(_symbolTable.GetSynonimType(synonimName), synonimName);

                AttributeNode attributeNode = new AttributeNode(attributeWithValue[0], attributeWithValue[1], synonimNode);
                withNode.Childrens.Add(attributeNode);
                return withNode;
            }
            else return null;
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
