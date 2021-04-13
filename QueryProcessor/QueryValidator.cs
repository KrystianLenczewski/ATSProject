using QueryProcessor.Enums;
using QueryProcessor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor
{
    internal class QueryValidator
    {
        private RelTable.RelTable _relTable = new RelTable.RelTable();
        private SymbolTable symbolTable;

        private readonly Dictionary<RelationArgumentType, string> attributeToSynonim = new Dictionary<RelationArgumentType, string>
        {
            [RelationArgumentType.Procedure] = "procName",
            [RelationArgumentType.Variable] = "varName",
            [RelationArgumentType.Constant] = "value",
            [RelationArgumentType.Statement] = "stmt#",
            [RelationArgumentType.Assign] = "stmt#"
        };

        private readonly Dictionary<string, RelationArgumentType> attributeToType = new Dictionary<string, RelationArgumentType>
        {
            ["procname"] = RelationArgumentType.String,
            ["varname"] = RelationArgumentType.String,
            ["value"] = RelationArgumentType.Integer,
            ["stmt#"] = RelationArgumentType.Integer
        };

        List<string> validationErrors = new List<string>();
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
            List<string> splitedQuery = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            splitedQuery.ForEach(x => x.Trim().ToLower());

            int selectIndex = splitedQuery.IndexOf(QueryElement.Select.ToLower());
            int suchThatIndex = GetIndexForSuchThat(splitedQuery);
            int withIndex = splitedQuery.IndexOf(QueryElement.With.ToLower());
            int patternIndex = splitedQuery.IndexOf(QueryElement.Pattern.ToLower());

            int endSelectIndex = suchThatIndex - 1;
            if (suchThatIndex == -1) endSelectIndex = withIndex == -1 ? patternIndex : withIndex;
            endSelectIndex = endSelectIndex == -1 ? splitedQuery.Count : endSelectIndex;

            bool selectSectionIsValid = ValidateSelectSection(splitedQuery, selectIndex, endSelectIndex);

            int endSuchThat = withIndex == -1 ? patternIndex : withIndex;
            endSuchThat = endSuchThat == -1 ? splitedQuery.Count : endSuchThat;
            bool suchThatSectionIsValid = true;
            if (suchThatIndex != -1)
                suchThatSectionIsValid = ValidateSuchThatSection(splitedQuery, suchThatIndex, endSuchThat);

            bool withSectionIsValid = true;
            int endWith = patternIndex == -1 ? splitedQuery.Count : patternIndex;
            if (withIndex != -1)
                withSectionIsValid = ValidateWithSection(splitedQuery, withIndex, endWith);

            return selectSectionIsValid && suchThatSectionIsValid && withSectionIsValid;
        }
        //Select s1 such that Follows (s1,s2)
        private bool ValidateSuchThatSection(List<string> splitedQuery, int beginIndex, int endIndex)
        {
            List<string> validateErrors = new List<string>();
            try
            {
                if (beginIndex + 1 != endIndex)
                {
                    //Follows
                    //(s1,s2)
                    for (int i = beginIndex + 1; i < endIndex; i += 2)
                    {
                        RelationType relationType = _relTable.GetRelationType(splitedQuery[i]);
                        List<string> relationArguments = splitedQuery[i + 1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                        //(s1
                        //s2)
                        List<RelationArgumentType> relationArgumentTypes = new List<RelationArgumentType>();
                        foreach (string relationArgument in relationArguments)
                        {
                            string relationArgumentName = relationArgument.Replace("(", "").Replace(")", "");
                            relationArgumentTypes.Add(symbolTable.GetRelationArgumentType(relationArgumentName));
                        }

                        if (!_relTable.ValidateRelation(relationType, relationArguments.Count, relationArgumentTypes.ToArray()))
                            validateErrors.Add($"Relacja: {splitedQuery[i]} jest niepoprawna.");
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

        private bool ValidateWithSection(List<string> splitedQuery, int beginIndex, int endIndex)
        {
            List<string> validateErrors = new List<string>();
            try
            {
                if (beginIndex + 1 != endIndex)
                {
                    string withCondition = splitedQuery[beginIndex + 1];
                    List<string> splittedWithCondition = withCondition.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
                    string synonimName = splittedWithCondition?.ElementAtOrDefault(0);
                    string[] attributeWithValue = splittedWithCondition?.ElementAtOrDefault(1)?.Split('=', StringSplitOptions.RemoveEmptyEntries);

                    if (string.IsNullOrEmpty(synonimName) || attributeWithValue?.Length != 2)
                        validateErrors.Add("Atrybuty po klauzuli With są nieprawidłowe.");
                    else
                    {
                        RelationArgumentType synonimType = symbolTable.GetRelationArgumentType(synonimName);
                        if (attributeToSynonim.TryGetValue(synonimType, out string attributeName))
                        {
                            if (attributeName.Equals(attributeWithValue[0], StringComparison.OrdinalIgnoreCase))
                            {
                                if (!ValidateAttributeValueType(attributeToType[attributeName.ToLower()], attributeWithValue[1]))
                                    validateErrors.Add("Wartość atrybutu ma nieprawidłowy typ.");
                            }
                            else
                                validateErrors.Add("Błędy atrybut dla synonimu w klauzuli With.");
                        }
                        else
                            validateErrors.Add("Dany synonim nie ma zdefiniowanych atrybutów po klauzuli With.");
                    }
                }
            }
            catch (ArgumentException e)
            {
                validateErrors.Add(e.Message);
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

        private int GetIndexForSuchThat(List<string> splittedQuery)
        {
            int suchIndex = splittedQuery.IndexOf("such");
            int thatIndex = splittedQuery.IndexOf("that");

            if (suchIndex + 1 == thatIndex)
                return thatIndex;
            return -1;
        }

        private bool ValidateAttributeValueType(RelationArgumentType relationArgumentType, string attributeValue)
        {
            if (relationArgumentType == RelationArgumentType.Integer)
                return int.TryParse(attributeValue, out _);
            else
                return attributeValue.StartsWith("\"") && attributeValue.EndsWith("\"");
        }
    }
}
