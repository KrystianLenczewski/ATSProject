using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor.ResultGeneration
{
    internal class ResultTableRow
    {
        private readonly Dictionary<string, string> _synonimsWithValue = new Dictionary<string, string>();

        public ResultTableRow(List<string> declaredSynonimsNames)
        {
            foreach (string synonimName in declaredSynonimsNames)
                _synonimsWithValue[synonimName] = string.Empty;
        }

        public bool CellIsEmpty(string synonimName) => _synonimsWithValue[synonimName] == string.Empty;

        public void SetValueForSynonim(string synonimName, string value) => _synonimsWithValue[synonimName] = value;

        public string GetValueForSynonim(string synonimName) => _synonimsWithValue[synonimName];

        public ResultTableRow Clone()
        {
            ResultTableRow resultTableRow = new ResultTableRow(_synonimsWithValue.Keys.ToList());
            foreach (var synonimSithValuePair in _synonimsWithValue)
                resultTableRow.SetValueForSynonim(synonimSithValuePair.Key, synonimSithValuePair.Value);

            return resultTableRow;
        }
    }
}
