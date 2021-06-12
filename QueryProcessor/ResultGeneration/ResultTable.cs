using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            _declaredSynonims = declaredSynonims.Where(w => !w.Equals("boolean", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public string GetResultPipeTesterFormat(Dictionary<string, List<string>> candidates, params string[] synonimNames)
        {
            List<string> newResult = new List<string>();
            List<string> oldFormatResult = GetResult(candidates, synonimNames);

            foreach (string oldFormatRow in oldFormatResult)
            {
                List<string> currentNewResult = new List<string> { "" };
                var synonimsWithValue = oldFormatRow.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var synonimWithValue in synonimsWithValue)
                {
                    var synonimValue = synonimWithValue.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!synonimValue[1].Contains("["))
                        AddForSimpleValue(synonimValue[1], currentNewResult);
                    else
                        currentNewResult = AddForComplexValue(synonimValue[1], currentNewResult);
                }

                newResult.AddRange(currentNewResult.Distinct());
            }

            return string.Join(", ", newResult.Distinct());
        }

        public List<string> GetResult(Dictionary<string, List<string>> candidates, params string[] synonimNames)
        {
            List<string> result = new List<string>();
            if (_resultTableRows.Count == 0 && !_queryHasRelations)
                _resultTableRows.Add(new ResultTableRow(_declaredSynonims));
            foreach (ResultTableRow resultTableRow in _resultTableRows)
            {
                string resultRow = "";
                foreach (string synonimName in synonimNames)
                {
                    if (!resultTableRow.CellIsEmpty(synonimName))
                    {
                        resultRow += $"{synonimName}:{resultTableRow.GetValueForSynonim(synonimName)} ";
                    }
                    else if (candidates[synonimName].Any())
                    {
                        resultRow += $"{synonimName}:[{string.Join(',', candidates[synonimName].Distinct())}] ";
                    }
                }
                if (!string.IsNullOrEmpty(resultRow))
                    result.Add(resultRow);
            }

            return result.Distinct().ToList();
        }

        public void DisplayTable()
        {
            foreach (var synonimName in _declaredSynonims)
                Console.Write($"{synonimName} ");
            Console.WriteLine();

            foreach (var row in _resultTableRows)
            {
                foreach (string synonimName in _declaredSynonims)
                {
                    if (!string.IsNullOrEmpty(row.GetValueForSynonim(synonimName)))
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
            if (!ExistsResult(firstArgumentName, firstArgumentValue, secondArgumentName, secondArgumentValue))
            {
                if (string.IsNullOrEmpty(secondArgumentName))
                {
                    ResultTableRow resultTableRow = new ResultTableRow(_declaredSynonims);
                    resultTableRow.SetValueForSynonim(firstArgumentName, firstArgumentValue);
                    _resultTableRows.Add(resultTableRow);
                }
                else
                {
                    bool isSecond = true;
                    var rowsWithSameValueForSynonim = GetRowsForValue(firstArgumentName, firstArgumentValue);
                    if (!rowsWithSameValueForSynonim.Any())
                    {
                        rowsWithSameValueForSynonim = GetRowsForValue(secondArgumentName, secondArgumentValue);
                        rowsWithSameValueForSynonim = RemoveDuplicates(rowsWithSameValueForSynonim, firstArgumentName);
                        isSecond = false;
                    }
                    else
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
                        string compareSynonim = isSecond ? secondArgumentName : firstArgumentName;
                        string value = isSecond ? secondArgumentValue : firstArgumentValue;
                        foreach (var row in rowsWithSameValueForSynonim)
                        {
                            if (!string.IsNullOrEmpty(row.GetValueForSynonim(compareSynonim)))
                            {
                                var clonedRow = row.Clone();
                                clonedRow.SetValueForSynonim(compareSynonim, value);
                                _resultTableRows.Add(clonedRow);
                            }
                            else
                                row.SetValueForSynonim(compareSynonim, value);
                        }
                    }
                }
            }
        }

        public void RefreshCandidates(string synonimName, List<string> values)
        {
            _resultTableRows = _resultTableRows.Where(w => values.Contains(w.GetValueForSynonim(synonimName))).ToList();
        }

        public void SetFalseBoolResult() => _boolResult = false;

        public bool GetBooleanResult()
        {
            return _boolResult;
            //_resultTableRows.Any();
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
            foreach (var resultTableRow in _resultTableRows)
            {
                if (resultTableRow.GetValueForSynonim(synonimName) == synonimValue)
                    result.Add(resultTableRow);
            }
            return result;
        }

        private List<ResultTableRow> RemoveDuplicates(List<ResultTableRow> resultTableRows, string ignoredSynonim = "")
        {
            List<ResultTableRow> result = new List<ResultTableRow>();
            foreach (var row in resultTableRows)
            {
                bool existsInResult = false;
                foreach (var resultRow in result)
                {
                    bool isDifferenceInAnySynonim = false;
                    foreach (var synonimName in _declaredSynonims.Where(w => w != ignoredSynonim))
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

        private void AddForSimpleValue(string value, List<string> currentNewResult)
        {
            for (int i = 0; i < currentNewResult.Count; i++)
            {
                if (string.IsNullOrEmpty(currentNewResult[i]))
                    currentNewResult[i] += value.ToString();
                else
                    currentNewResult[i] += $" {value}";
            }
        }

        private List<string> AddForComplexValue(string value, List<string> currentNewResult)
        {
            value = value.Replace("[", "").Replace("]", "");
            var splittedValues = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();

            foreach (var progLine in splittedValues)
            {
                foreach (string currentResultRow in currentNewResult)
                {
                    if (!string.IsNullOrEmpty(currentResultRow))
                        result.Add(progLine);
                    else
                        result.Add($"{currentResultRow} {progLine}");
                }
            }

            return result;
        }
    }
}
