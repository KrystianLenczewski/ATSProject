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
                QueryTree queryTree = new QueryTree(_symbolTable.QuerySymbols);
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
            
            if (suchThatIndex != -1)
            {
                SectionNode relationSectionNode = new SectionNode() { NodeType = NodeType.SUCH_THAT };
                int endSuchThat = GetSuchThatEndIndex(splitedQuery);

                // 'i' wskazuje na pierwsza i kolejne czesci SUCH_THAT po AND lub spowoduje wyjscie z petli                
                for (int i = suchThatIndex + 1; i < endSuchThat; i += 3)
                {
                    RelationType relationType = _relTable.GetRelationType(splitedQuery[i]);
                    RelationNode relationNode = new RelationNode() { RelationType = relationType };
                    relationSectionNode.Childrens.Add(relationNode);
                    
                    List<string> relationArguments = splitedQuery[i + 1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    AddArgumentsToRelationNode(relationNode, relationArguments);
                }

                return relationSectionNode;
            }

            return null;
        }

        private void AddArgumentsToRelationNode(RelationNode relationNode, List<string> relationArguments)
        {
            foreach (string relationArgument in relationArguments)
            {
                string relationArgumentName = relationArgument.Replace("(", "").Replace(")", "");
                RelationArgumentType relationArgumentType = _symbolTable.GetRelationArgumentType(relationArgumentName);
                if (relationArgumentType == RelationArgumentType.String)
                    relationArgumentName = relationArgumentName.Replace("\"", "");
                ArgumentNode argumentNode = new ArgumentNode(relationArgumentType, relationArgumentName);
                relationNode.Arguments.Add(argumentNode);
            }
        }

        private SectionNode ExtractResult(string query)
        {
            List<string> splitedQuery = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            splitedQuery.ForEach(x => x.Trim().ToLower());
            int selectIndex = splitedQuery.FindIndex(x => x.Equals(QueryElement.Select, StringComparison.OrdinalIgnoreCase));

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
            int withIndex = splitedQuery.FindIndex(x => x.Equals(QueryElement.With, StringComparison.OrdinalIgnoreCase));
            int patternIndex = splitedQuery.IndexOf(QueryElement.Pattern.ToLower());
            int endWith = patternIndex == -1 ? splitedQuery.Count : patternIndex;

            if (withIndex + 1 != endWith && withIndex != -1)
            {
                SectionNode withNode = new SectionNode { NodeType = NodeType.WITH };

                //dziala z dodawaniem AND, index przechodzi wskazujac na kolejne 'withCondition' (np. p.procName="abc") i wychodzi z petli po wszystkich elementach zlaczonych poprzez AND
                for (int i = withIndex + 1; i < endWith; i += 2)
                {
                    string withCondition = splitedQuery[i];
                    AttributeNode attributeNode = GetAttributeNodeForWithCondition(withCondition);
                    withNode.Childrens.Add(attributeNode);
                }

                return withNode;                

            }
            else return null;
        }



        private AttributeNode GetAttributeNodeForWithCondition(string withCondition)
        {
            List<string> splittedWithCondition = withCondition.Split('=', StringSplitOptions.RemoveEmptyEntries).ToList();
            string leftSide = splittedWithCondition[0];
            string rightSide = splittedWithCondition[1];

            string[] splittedLeftSide = leftSide.Split('.', StringSplitOptions.RemoveEmptyEntries);
            SynonimNode leftSideSynonimNode = new SynonimNode(_symbolTable.GetSynonimType(splittedLeftSide[0]), splittedLeftSide[0]);
            AttributeNode leftSideAttributeNode;
            if (IsConst(rightSide))
                leftSideAttributeNode = new AttributeNode(splittedLeftSide[1], rightSide, leftSideSynonimNode);
            else
            {
                string[] splittedRigtSide = rightSide.Split('.', StringSplitOptions.RemoveEmptyEntries);
                SynonimNode rightSideSynonimNode = new SynonimNode(_symbolTable.GetSynonimType(splittedRigtSide[0]), splittedRigtSide[0]);
                AttributeNode rightSideAttributeNode = new AttributeNode(splittedRigtSide[1], rightSideSynonimNode);
                leftSideAttributeNode = new AttributeNode(splittedLeftSide[1], rightSideAttributeNode, leftSideSynonimNode);
                rightSideAttributeNode.AttributeValue = leftSideAttributeNode;
            }
            return leftSideAttributeNode;
        }

        private int GetIndexForSuchThat(List<string> splittedQuery)
        {
            int suchIndex = splittedQuery.FindIndex(x => x.Equals("such", StringComparison.OrdinalIgnoreCase));
            int thatIndex = splittedQuery.FindIndex(x => x.Equals("that", StringComparison.OrdinalIgnoreCase));

            if (suchIndex + 1 == thatIndex)
                return thatIndex;
            return -1;
        }

        private int GetSuchThatEndIndex(List<string> splittedQuery)
        {
            int withIndex = splittedQuery.FindIndex(x => x.Equals(QueryElement.With, StringComparison.OrdinalIgnoreCase));
            int patternIndex = splittedQuery.IndexOf(QueryElement.Pattern.ToLower());

            int endSuchThat = withIndex == -1 ? patternIndex : withIndex;
            endSuchThat = endSuchThat == -1 ? splittedQuery.Count : endSuchThat;
            return endSuchThat;
        }

        private bool IsConst(string phrase)
        {
            if (int.TryParse(phrase, out _))
                return true;
            phrase = phrase.Replace("\"", "");
            return !phrase.Any(ch => !char.IsLetter(ch));
        }

    }
}
