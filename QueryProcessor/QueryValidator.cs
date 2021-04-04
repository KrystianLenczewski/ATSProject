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

        List<string> validationErrors = new List<string>();
        internal bool ValidateQuery(string query, out List<string> validationErrors)
        {
            symbolTable = new SymbolTable(query);

        }
        private bool ValidateQueryStructure(string query)
        {

            List<string> splitedQuery = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            splitedQuery.ForEach(x => x.Trim().ToLower());

            int selectIndex = splitedQuery.IndexOf(QueryElement.Select.ToLower());
            int suchThatIndex = splitedQuery.IndexOf(QueryElement.SuchThat.ToLower());
            int withIndex = splitedQuery.IndexOf(QueryElement.With.ToLower());
            int patternIndex = splitedQuery.IndexOf(QueryElement.Pattern.ToLower());

            int endSelectIndex = suchThatIndex;
            if (suchThatIndex == -1) endSelectIndex = withIndex == -1 ? patternIndex : withIndex;

            bool selectSectionIsValid = ValidateSelectSection(splitedQuery, selectIndex, endSelectIndex);




            int endSuchThat = withIndex == -1 ? patternIndex : withIndex;

            if (suchThatIndex != -1)
                bool suchThatSectionIsValid = ValidateSuchThatSection(splitedQuery, suchThatIndex, endSuchThat);

            int endWith = patternIndex == -1 ? splitedQuery.Count : patternIndex;
            if(withIndex !=-1)
                bool withSectionIsValid = ValidateSuchThatSection(splitedQuery, withIndex, endWith);

        }
        //Select s1 such that Follows (s1,s2)
        private bool ValidateSuchThatSection(List<string> splitedQuery, int beginIndex, int endIndex)
        {
            List<string> validateErrors = new List<string>();
            List<string> resultSymbols = splitedQuery[beginIndex + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            //Follows
            //(s1,s2)
            for (int i = 0; i < resultSymbols.Count; i++)
            {
                RelationType relationType = _relTable.GetRelationType(resultSymbols[0]);
                List<string> arguments = resultSymbols[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                //(s1
                //s2)
                RelationArgumentType[] relationArgumentTypes = new RelationArgumentType[arguments.Count];
                for (int i = 0; i < arguments.Count; i++)
                {
                    relationArgumentTypes[i] = arguments[i];
                } 

                if (_relTable.ValidateRelation(relationType, arguments.Count))
                {

                }
            }

        }

        private bool ValidateWithSection(List<string> splitedQuery, int beginIndex, int endIndex)
        {
            List<string> validateErrors = new List<string>();


        }

        private bool ValidateSelectSection(List<string> splitedQuery, int beginIndex, int endIndex)
        {
            List<string> validateErrors = new List<string>();

            if (beginIndex == -1) validateErrors.Add("Brak klauzuli select w zapytaniu.");
            if (beginIndex + 1 == endIndex) validateErrors.Add("Brak wartości do zwrócenia(po select).");

            List<string> resultSymbols = splitedQuery[beginIndex + 1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var symbol in resultSymbols)
            {
                if (!symbolTable.IsSymbolExist(symbol)) validateErrors.Add("Symbol po select nie został zadeklarowany.");
            }

            this.validationErrors.AddRange(validateErrors);
            return validateErrors.Count < 0;
        }


        private bool ValidateRelations(string query)
        {

        }
    }
}
