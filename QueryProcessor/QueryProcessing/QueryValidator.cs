using QueryProcessor.Enums;
using QueryProcessor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor.QueryProcessing
{
    internal class QueryValidator
    {
        private readonly RelTable.RelTable _relTable = new RelTable.RelTable();
        private SymbolTable symbolTable;

        private readonly Dictionary<RelationArgumentType, string> _attributeToSynonim = new Dictionary<RelationArgumentType, string>
        {
            [RelationArgumentType.Procedure] = "procName",
            [RelationArgumentType.Variable] = "varName",
            [RelationArgumentType.Constant] = "value",
            [RelationArgumentType.Statement] = "stmt#",
            [RelationArgumentType.Assign] = "stmt#"
        };

        private readonly Dictionary<string, List<string>> _availableAttributeComparison = new Dictionary<string, List<string>>
        {
            ["procName"] = new List<string> { "procName", "varName" },
            ["varName"] = new List<string> { "procName", "varName" },
            ["stmt#"] = new List<string> { "stmt#", "value" }
        };

        private readonly Dictionary<string, RelationArgumentType> attributeToType = new Dictionary<string, RelationArgumentType>
        {
            ["procname"] = RelationArgumentType.String,
            ["varname"] = RelationArgumentType.String,
            ["value"] = RelationArgumentType.Integer,
            ["stmt#"] = RelationArgumentType.Integer
        };
        private readonly List<string> validationErrors = new List<string>();
        internal bool ValidateQuery(string query, out List<string> validationErrors)
        {
            try
            {
                symbolTable = new SymbolTable(query);
                bool result = ValidateQueryStructure(query);
                validationErrors = this.validationErrors.Distinct().ToList();
                return result;
            }
            catch (Exception)
            {
                validationErrors = new List<string>();
                validationErrors.Add("Zapytanie jest nieprawidłowe - nieprawidłowa składnia");
                return false;
            }

        }
        private bool ValidateQueryStructure(string query)
        {

            //select p with p.procName=\"Second\" with ..... such that Calls(p,q) such that Modifies() with 


            List<string> splitedQuery = GetSplitedQuery(query);

            bool validQuery = true;
        
            for(int i=0; i<splitedQuery.Count; i++)
            {
                string keyword="";
                string nextKeyword;
                int endIndex=-1;
                bool validQueryElement = true;

                keyword = QueryElement.GetKeyword(splitedQuery[i]);

                if(!String.IsNullOrEmpty(keyword))
                {
                    for (int p =i+1; p < splitedQuery.Count; p++)
                    {
                         nextKeyword = QueryElement.GetKeyword(splitedQuery[p]);
                        if (!String.IsNullOrEmpty(nextKeyword)) { endIndex = p; break; }
                    }
                    if (endIndex == -1) endIndex = splitedQuery.Count;

                    switch (keyword)
                    {
                        case QueryElement.Select:
                            validQueryElement = ValidateSelectSection(splitedQuery, i, endIndex);
                            break;
                        case QueryElement.SuchThat:
                            validQueryElement = ValidateSuchThatSection(splitedQuery, i, endIndex);
                            break;
                        case QueryElement.With:
                            validQueryElement = ValidateWithSection(splitedQuery, i, endIndex);
                            break;
                        default:
                            break;
                    }
                }

               
                validQuery = validQuery && validQueryElement;
            }

            return validQuery;

            //int selectIndex = splitedQuery.IndexOf(QueryElement.Select.ToLower());
            //int suchThatIndex = GetIndexForSuchThat(splitedQuery);
            //int withIndex = splitedQuery.IndexOf(QueryElement.With.ToLower());
            //int patternIndex = splitedQuery.IndexOf(QueryElement.Pattern.ToLower());

            //int endSelectIndex = suchThatIndex - 1;
            //if (suchThatIndex == -1) endSelectIndex = withIndex == -1 ? patternIndex : withIndex;
            //endSelectIndex = endSelectIndex == -1 ? splitedQuery.Count : endSelectIndex;

            //bool selectSectionIsValid = ValidateSelectSection(splitedQuery, selectIndex, endSelectIndex);

            //int endSuchThat = withIndex == -1 ? patternIndex : withIndex;
            //endSuchThat = endSuchThat == -1 ? splitedQuery.Count : endSuchThat;
            //bool suchThatSectionIsValid = true;
            //if (suchThatIndex != -1)
            //    suchThatSectionIsValid = ValidateSuchThatSection(splitedQuery, suchThatIndex, endSuchThat);

            //bool withSectionIsValid = true;
            //int endWith = patternIndex == -1 ? splitedQuery.Count : patternIndex;
            //if (withIndex != -1)
            //    withSectionIsValid = ValidateWithSection(splitedQuery, withIndex, endWith);

            //return selectSectionIsValid && suchThatSectionIsValid && withSectionIsValid;
        }
        private bool ValidateSuchThatSection(List<string> splitedQuery, int beginIndex, int endIndex)
        {

            List<string> validateErrors = new List<string>();
            try
            {   
                if (beginIndex + 1 != endIndex)
                {                    
                    for (int i = beginIndex + 1; i < endIndex; i += 3)
                    {
                        RelationType relationType = _relTable.GetRelationType(splitedQuery[i]);
                        List<string> relationArguments = splitedQuery[i + 1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                        List<RelationArgumentType> relationArgumentTypes = new List<RelationArgumentType>();
                        foreach (string relationArgument in relationArguments)
                        {
                            string relationArgumentName = relationArgument.Replace("(", "").Replace(")", "");
                            relationArgumentTypes.Add(symbolTable.GetRelationArgumentType(relationArgumentName));
                        }

                        if (!_relTable.ValidateRelation(relationType, relationArguments.Count, relationArgumentTypes.ToArray()))
                            validateErrors.Add($"Relacja: {splitedQuery[i]} jest niepoprawna.");

                        ValidateAndInSuchThatSection(splitedQuery, validateErrors, i);
                    }
                }
                else
                    validateErrors.Add("Po such that nie ma żadnych relacji.");
            }
            catch (ArgumentException e)
            {
                validateErrors.Add(e.Message);
            }

            this.validationErrors.AddRange(validateErrors);
            return validateErrors.Count == 0;
        }

        private void ValidateAndInSuchThatSection(List<string> splitedQuery, List<string> validateErrors, int i)
        {
            // walidacja, czy po relacji i jej argumentach, jesli nie ma "with" lub "pattern", to znajduje sie "and"
            if (splitedQuery.Count > i + 2 || splitedQuery.Count==i+2)
            {
                if ((splitedQuery[i + 2] != QueryElement.With && splitedQuery[i + 2] != QueryElement.Pattern) || splitedQuery.Count!=i+2)
                {
                    if (splitedQuery[i + 2].ToLower() != QueryElement.And)
                        validateErrors.Add($"Oczekiwano AND po relacji w Such that zamiast: {splitedQuery[i + 2]}");
                }
            }

            // walidacja czy po "and" znajduje sie relacja
            if (splitedQuery.Count > i + 2)
            {
                if (splitedQuery[i + 2].ToLower() == QueryElement.And)
                {
                    try
                    {
                        _relTable.GetRelationType(splitedQuery[i + 3]);

                    }
                    catch (Exception ex)
                    {
                        validateErrors.Add($"Po AND oczekiwano relacji zamiast: {splitedQuery[i + 3]}, {ex}");
                    }
                }
            }
        }

        private bool ValidateWithSection(List<string> splitedQuery, int beginIndex, int endIndex)
        {
            List<string> validateErrors = new List<string>();
            try
            {
                if (beginIndex + 1 != endIndex)
                {
                    for (int i = beginIndex + 1; i < endIndex; i += 2)
                    {
                        string withCondition = splitedQuery[beginIndex + 1];
                        string[] splittedWithCondition = withCondition.Split('=', StringSplitOptions.RemoveEmptyEntries);
                        
                        if (splittedWithCondition.Length == 2)
                        {
                            string leftSide = splittedWithCondition.ElementAtOrDefault(0);
                            string rightSide = splittedWithCondition.ElementAtOrDefault(1);
                            if (ValidateAttributePhrase(leftSide))
                            {
                                if (IsConst(rightSide) || ValidateAttributePhrase(rightSide))
                                {
                                    if (!ValidateComparison(leftSide, rightSide))
                                        validateErrors.Add("Błędne przyrównanie w klauzuli with.");
                                }
                            }
                        }
                        else
                            validateErrors.Add("Atrybuty po klauzuli With są nieprawidłowe.");

                        //if (!ValidateAndInWithSection(splitedQuery, validateErrors, i))
                        //{
                        //    validateErrors.Add("Wystąpił błąd przy walidacji AND w klauzuli With.");
                        //}
                    }
                }
            }
            catch (ArgumentException e)
            {
                validateErrors.Add(e.Message);
            }

            validationErrors.AddRange(validateErrors);
            return validateErrors.Count == 0;
        }

        private bool ValidateAndInWithSection(List<string> splitedQuery, List<string> validateErrors, int index)
        {   
            int splitedQuerySize = splitedQuery.Count;

            if (splitedQuerySize > index + 1)
            {
                String splitedQueryElement = splitedQuery[index + 1];

                // walidacja, czy jesli nie ma "pattern" to znajduje sie "and"
                if (splitedQueryElement != QueryElement.Pattern)
                {                    
                    if (splitedQueryElement.ToLower() != QueryElement.And)
                        validateErrors.Add($"Oczekiwano AND po relacji w With zamiast: {splitedQueryElement}");
                }
                
                if (splitedQueryElement.ToLower() == QueryElement.And)
                {
                    // walidacja, czy "and" nie jest ostatnim miejscu zapytania
                    if (splitedQuerySize == index + 2)
                    {
                        validateErrors.Add("AND nie może być na koncu zapytania.");
                    }

                    // walidacja czy po "and" znajduje sie odpowiednio zadany warunek
                    if (splitedQuerySize > index + 2)
                    {
                        string nextWithCondition = splitedQuery[index + 2];
                        string[] splittedNextWithCondition = nextWithCondition.Split('=', StringSplitOptions.RemoveEmptyEntries);

                        if (splitedQueryElement == QueryElement.And)
                        {
                            if (splittedNextWithCondition.Length == 2)
                            {
                                string nextLeftSide = splittedNextWithCondition.ElementAtOrDefault(0);
                                string nextRightSide = splittedNextWithCondition.ElementAtOrDefault(1);
                                if (ValidateAttributePhrase(nextLeftSide))
                                {
                                    if (IsConst(nextRightSide) || ValidateAttributePhrase(nextRightSide))
                                    {
                                        if (!ValidateComparison(nextLeftSide, nextRightSide))
                                            validateErrors.Add("Błędne przyrównanie w klauzuli with po AND.");
                                    }
                                }
                            }
                            else
                                validateErrors.Add("Atrybuty po AND w klauzuli With są nieprawidłowe.");
                        }
                    }
                }
            }
            this.validationErrors.AddRange(validateErrors);
            return validateErrors.Count == 0;
        }

        private bool ValidateSelectSection(List<string> splitedQuery, int beginIndex, int endIndex)
        {
            List<string> validateErrors = new List<string>();

            if (beginIndex == -1) validateErrors.Add("Brak klauzuli select w zapytaniu.");
            if (beginIndex + 1 == endIndex) validateErrors.Add("Brak wartości do zwrócenia(po select).");

            List<string> resultSymbols = splitedQuery[beginIndex + 1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            if (beginIndex + 2 < endIndex) validateErrors.Add("Nieprawidłowe dane w klauzuli select.");

            foreach (var symbol in resultSymbols)
            {
                if (!symbolTable.IsSymbolExist(symbol)) validateErrors.Add("Symbol po select nie został zadeklarowany.");
            }

            this.validationErrors.AddRange(validateErrors);
            return validateErrors.Count == 0;
        }


        private bool ValidateAttributeValueType(RelationArgumentType relationArgumentType, string attributeValue)
        {
            if (relationArgumentType == RelationArgumentType.Integer)
                return int.TryParse(attributeValue, out _);
            else
                return attributeValue.StartsWith("\"") && attributeValue.EndsWith("\"");
        }

        private bool ValidateAttributePhrase(string attributePhrase)
        {
            string[] splittedAttributePhrase = attributePhrase.Split('.', StringSplitOptions.RemoveEmptyEntries);
            string synonimName = splittedAttributePhrase?.ElementAtOrDefault(0);
            string attribute = splittedAttributePhrase?.ElementAtOrDefault(1);
            if (string.IsNullOrEmpty(synonimName) || string.IsNullOrEmpty(attribute))
                throw new Exception("Atrybuty po klauzuli With są nieprawidłowe.");

            RelationArgumentType synonimType = symbolTable.GetRelationArgumentType(synonimName);

            if (_attributeToSynonim.TryGetValue(synonimType, out string attributeName))
            {
                if (!attributeName.Equals(attribute, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Błędy atrybut dla synonimu w klauzuli With.");
            }
            else
                throw new Exception("Dany synonim nie ma zdefiniowanych atrybutów po klauzuli With.");

            return true;
        }

        private bool IsConst(string phrase)
        {
            if (int.TryParse(phrase, out _))
                return true;
            phrase = phrase.Replace("\"", "");
            return !phrase.Any(ch => !char.IsLetter(ch));
        }

        private bool ValidateComparison(string leftSide, string rightSide)
        {
            string[] splittedLeftSide = leftSide.Split('.', StringSplitOptions.RemoveEmptyEntries);
            string leftSideAttributeName = splittedLeftSide.ElementAtOrDefault(1);

            if (IsConst(rightSide))
            {
                return ValidateAttributeValueType(attributeToType[leftSideAttributeName.ToLower()], rightSide);
            }
            else if (ValidateAttributePhrase(rightSide))
            {
                string[] splittedRightSide = rightSide.Split('.', StringSplitOptions.RemoveEmptyEntries);
                string rightSideAttributeName = splittedRightSide.ElementAtOrDefault(1);

                List<string> availableComparisonAttributes = _availableAttributeComparison[leftSideAttributeName];
                return availableComparisonAttributes.Contains(rightSideAttributeName);
            }
            return false;
        }
        private List<string> GetSplitedQuery(string query)
        {
            List<string> splitedQuery = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> splitedQueryResult = new List<string>();
            splitedQuery.ForEach(x => x.Trim().ToLower());
          
            for(int i=0;i<splitedQuery.Count;i++)
            {
                if(splitedQuery[i]=="such" && splitedQuery[i+1]=="that" && i+1<splitedQuery.Count)
                {
                    splitedQueryResult.Add(QueryElement.SuchThat);
                    i++;
                }
                else
                {
                    splitedQueryResult.Add(splitedQuery[i]);
                }
            }

            return splitedQueryResult;
        }

       
    }
}
