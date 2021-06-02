using PKB;
using QueryProcessor.Enums;
using QueryProcessor.Infrastructure;
using QueryProcessor.Moqs;
using QueryProcessor.QueryTreeNodes;
using QueryProcessor.ResultGeneration;
using Shared;
using Shared.PQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor.QueryProcessing
{
    public class QueryEvaluator
    {
        private readonly PKBStore _pkbStore;
        private ResultTable _resultTable;
        private readonly Dictionary<string, List<string>> _candidates = new Dictionary<string, List<string>>();


        public QueryEvaluator()
        {
            _pkbStore = PKBInitializer.InitializePKB();
        }

        public List<string> GetQueryResultsRaw(QueryTree queryTree)
        {
            queryTree.PrepareTreeForQueryEvaluator();
            PrepareCandidatesDictionary(queryTree.GetDeclarations() ?? new Dictionary<string, RelationArgumentType>());
            _resultTable = new ResultTable(queryTree.GetDeclarations().Keys.ToList() ?? new List<string>());

            List<RelationNode> relations = queryTree.GetRelationNodes();
            HandleRelations(relations);
            List<string> resultSynonimNames = queryTree.GetResultNodeChildrens().Select(s=>s.Name).ToList();

            return _resultTable.GetResult(_candidates, resultSynonimNames.ToArray());
        }

        private void HandleRelations(List<RelationNode> relationNodes)
        {
            foreach (RelationNode relationNode in relationNodes)
            {
                if (relationNode.RelationType == RelationType.PARENT)
                    HandleParent(relationNode);
                if (relationNode.RelationType == RelationType.FOLLOWS)
                    HandleFollows(relationNode);

            }
        }

        private void HandleFollows(RelationNode relationFollowsNode)
        {
            ArgumentNode arg1 = relationFollowsNode.Arguments[0];
            ArgumentNode arg2 = relationFollowsNode.Arguments[1];
            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetFollows(int.Parse(line)).Select(s => s.ProgramLine.ToString());
                    if (result.Any())
                    {
                        foreach (var arg2Line in result)
                            _resultTable.AddRelationResult(arg1.Name, line, arg2.Name, arg2Line);
                        arg2Candidates.AddRange(result);
                        arg2Candidates = arg2Candidates.Distinct().ToList();
                    }
                    else
                        arg1CandidatesToRemove.Add(line);
                }

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !arg2Candidates.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg1.Name))
            {
                List<string> result = _pkbStore.GetFollowed(int.Parse(arg2.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result = _pkbStore.GetFollows(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = _pkbStore.GetFollows(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void HandleParent(RelationNode relationParentNode)
        {
            ArgumentNode arg1 = relationParentNode.Arguments[0];
            ArgumentNode arg2 = relationParentNode.Arguments[1];
            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetChildren(int.Parse(line)).Select(s => s.ProgramLine.ToString());
                    if (result.Any())
                    {
                        foreach (var arg2Line in result)
                            _resultTable.AddRelationResult(arg1.Name, line, arg2.Name, arg2Line);
                        arg2Candidates.AddRange(result);
                        arg2Candidates = arg2Candidates.Distinct().ToList();
                    }
                    else
                        arg1CandidatesToRemove.Add(line);
                }
                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !arg2Candidates.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg1.Name))
            {
                List<string> parents = _pkbStore.GetParents(int.Parse(arg2.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in parents)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !parents.Contains(w)).ToList(); ;
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> children = _pkbStore.GetChildren(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg2Line in children)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !children.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> children = _pkbStore.GetChildren(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                if (!children.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void RemoveCandidates(string synonimName, List<string> candidatesToRemove)
        {
            if (_candidates.TryGetValue(synonimName, out List<string> candidates))
            {
                foreach (var candidateToRemove in candidatesToRemove)
                {
                    _candidates[synonimName].Remove(candidateToRemove);
                }
                _resultTable.RefreshCandidates(synonimName, _candidates[synonimName]);
            }
        }

        private void PrepareCandidatesDictionary(Dictionary<string, RelationArgumentType> declarations)
        {
            foreach (var declarationsPair in declarations)
            {
                ExpressionType? expressionType = ToExpressionType(declarationsPair.Value);
                if (expressionType.HasValue)
                {
                    _candidates[declarationsPair.Key] = new List<string>();
                    _candidates[declarationsPair.Key].AddRange(_pkbStore.GetChildren(0, expressionType.Value).Select(s => s.ProgramLine.ToString()).Distinct().ToList() ?? new List<string>());
                }
                else
                {
                    _candidates[declarationsPair.Key] = new List<string>();
                    _candidates[declarationsPair.Key].AddRange(_pkbStore.GetChildren(0, ExpressionType.NULL).Select(s => s.ProgramLine.ToString()).Distinct().ToList() ?? new List<string>());
                }
            }
        }

        private ExpressionType? ToExpressionType(RelationArgumentType relationArgumentType)
        {
            return relationArgumentType switch
            {
                RelationArgumentType.Assign => ExpressionType.ASSIGN,
                RelationArgumentType.Call => ExpressionType.CALL,
                RelationArgumentType.Constant => ExpressionType.CONST,
                RelationArgumentType.If => ExpressionType.IF,
                RelationArgumentType.Procedure => ExpressionType.PROCEDURE,
                RelationArgumentType.Statement => null,
                RelationArgumentType.Variable => ExpressionType.VAR,
                RelationArgumentType.While => ExpressionType.WHILE,
                _ => null
            };
        }

    }
}
