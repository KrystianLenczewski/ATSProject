using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor.ResultGeneration
{
    public class ResultTable
    {
        private readonly List<string> _declaredSynonims;
        private List<ResultTableRow> _resultTableRows = new List<ResultTableRow>();
        private bool _boolResult = true;
        private bool _queryHasRelations = false;

        public ResultTable(List<string> declaredSynonims)
        {
            _declaredSynonims = declaredSynonims.Where(w=>!w.Equals("boolean", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<string> GetResult(Dictionary<string, List<string>> candidates, params string[] synonimNames)
        {
            List<string> result = new List<string>();
            if (_resultTableRows.Count == 0 && !_queryHasRelations)
                _resultTableRows.Add(new ResultTableRow(_declaredSynonims));
            foreach(ResultTableRow resultTableRow in _resultTableRows)
            {
                string resultRow = "";
                foreach (string synonimName in synonimNames)
                {
                    if (!resultTableRow.CellIsEmpty(synonimName))
                    {
                        resultRow += $"{synonimName}:{resultTableRow.GetValueForSynonim(synonimName)} ";
                    }
                    else if(candidates[synonimName].Any())
                    {
                        resultRow += $"{synonimName}:[{string.Join(',', candidates[synonimName].Distinct())}] ";
                    }
                }
                if(!string.IsNullOrEmpty(resultRow))
                    result.Add(resultRow);
            }

            return result.Distinct().ToList();
        }

        public void DisplayTable()
        {
            foreach (var synonimName in _declaredSynonims)
                Console.Write($"{synonimName} ");
            Console.WriteLine();

            foreach(var row in _resultTableRows)
            {
                foreach (string synonimName in _declaredSynonims)
                {
                    if(!string.IsNullOrEmpty(row.GetValueForSynonim(synonimName)))
                        Console.Write($"{row.GetValueForSynonim(synonimName)} ");
                    else
                        Console.Write($"- ");
                }

                Console.WriteLine();
            }
        }

        public void SetQueryHasRelations() => _queryHasRelations = true;

        public void AddRelationResult(string firstArgumentName, string firstArgumentValue, string secondArgumentName = "", string secondArgumentValue = "")
        {
            if(!ExistsResult(firstArgumentName, firstArgumentValue, secondArgumentName, secondArgumentValue))
            {
                if (string.IsNullOrEmpty(secondArgumentName))
                {
                    ResultTableRow resultTableRow = new ResultTableRow(_declaredSynonims);
                    resultTableRow.SetValueForSynonim(firstArgumentName, firstArgumentValue);
                    _resultTableRows.Add(resultTableRow);
                }
                else
                {
                    var rowsWithSameValueForSynonim = GetRowsForValue(firstArgumentName, firstArgumentValue);
                    rowsWithSameValueForSynonim = RemoveDuplicates(rowsWithSameValueForSynonim, secondArgumentName);
                    if (!rowsWithSameValueForSynonim.Any())
                    {
                        ResultTableRow resultTableRow = new ResultTableRow(_declaredSynonims);
                        resultTableRow.SetValueForSynonim(firstArgumentName, firstArgumentValue);
                        resultTableRow.SetValueForSynonim(secondArgumentName, secondArgumentValue);
                        _resultTableRows.Add(resultTableRow);
                    }
                    else
                    {
                        foreach(var row in rowsWithSameValueForSynonim)
                        {
                            if (!string.IsNullOrEmpty(row.GetValueForSynonim(secondArgumentName)))
                            {
                                var clonedRow = row.Clone();
                                clonedRow.SetValueForSynonim(secondArgumentName, secondArgumentValue);
                                _resultTableRows.Add(clonedRow);
                            }
                            else
                                row.SetValueForSynonim(secondArgumentName, secondArgumentValue);
                        }
                    }
                }
            }
        }

        public void RefreshCandidates(string synonimName, List<string> values)
        {
            _resultTableRows = _resultTableRows.Where(w =>values.Contains(w.GetValueForSynonim(synonimName))).ToList();
        }

        public void SetFalseBoolResult() => _boolResult = false;

        public bool GetBooleanResult()
        {
            return _boolResult && _resultTableRows.Any();
        }

        private bool ExistsResult(string firstSynonimName, string firstSynonimValue, string secondSynonimName = "", string secondSynonimValue = "")
        {
            foreach (var resultTableRow in _resultTableRows)
            {
                if (string.IsNullOrEmpty(secondSynonimName))
                {
                    if (resultTableRow.GetValueForSynonim(firstSynonimName) == firstSynonimValue)
                        return true;
                }
                else if (resultTableRow.GetValueForSynonim(firstSynonimName) == firstSynonimValue && resultTableRow.GetValueForSynonim(secondSynonimName) == secondSynonimValue)
                    return true;
            }

            return false;
        }

        private List<ResultTableRow> GetRowsForValue(string synonimName, string synonimValue)
        {
            List<ResultTableRow> result = new List<ResultTableRow>();
            foreach(var resultTableRow in _resultTableRows)
            {
                if (resultTableRow.GetValueForSynonim(synonimName) == synonimValue)
                    result.Add(resultTableRow);
            }
            return result;
        }

        private List<ResultTableRow> RemoveDuplicates(List<ResultTableRow> resultTableRows, string ignoredSynonim = "")
        {
            List<ResultTableRow> result = new List<ResultTableRow>();
            foreach(var row in resultTableRows)
            {
                bool existsInResult = false;
                foreach(var resultRow in result)
                {
                    bool isDifferenceInAnySynonim = false;
                    foreach(var synonimName in _declaredSynonims.Where(w => w != ignoredSynonim))
                    {
                        if (row.GetValueForSynonim(synonimName) != resultRow.GetValueForSynonim(synonimName))
                            isDifferenceInAnySynonim = true;
                    }

                    if (!isDifferenceInAnySynonim)
                        existsInResult = true;
                }
                if (!existsInResult)
                    result.Add(row);
            }

            return result;
        }
    }
}
