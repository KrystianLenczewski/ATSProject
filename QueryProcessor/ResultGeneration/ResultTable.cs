using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.ResultGeneration
{
    public class ResultTable
    {
        private readonly Dictionary<string, RelationArgumentType> _declaredSymbols;
        private readonly List<ResultTableRow> _resultTableRows = new List<ResultTableRow>();

        public ResultTable(Dictionary<string, RelationArgumentType> declaredSymbols)
        {
            _declaredSymbols = declaredSymbols;
        }

        public void AddRelationResult(string firstArgumentName, string firstArgumentValue, string secondArgumentName, string secondArgumentValue)
        {

        }
    }
}
