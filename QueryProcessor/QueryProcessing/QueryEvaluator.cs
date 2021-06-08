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
        private readonly IPKBStore _pkbStore;
        private ResultTable _resultTable;
        private readonly Dictionary<string, List<string>> _candidates = new Dictionary<string, List<string>>();


        public QueryEvaluator(IPKBStore pkbStore)
        {
            _pkbStore = pkbStore;
        }

        public List<string> GetQueryResultsRaw(QueryTree queryTree)
        {
            PrepareCandidatesDictionary(queryTree.GetDeclarations() ?? new Dictionary<string, RelationArgumentType>());
            _resultTable = new ResultTable(queryTree.GetDeclarations().Keys.ToList() ?? new List<string>());

            List<RelationNode> relations = queryTree.GetRelationNodes();
            HandleRelations(relations);
            List<string> resultSynonimNames = queryTree.GetResultNodeChildrens().Select(s=>s.Name).ToList();
            ApplyWithRestrictions(queryTree.GetAttributeNodes());

            if (resultSynonimNames.FirstOrDefault()?.ToLower() == "boolean".ToLower())
                return new List<string> { _resultTable.GetBooleanResult().ToString() };
            return _resultTable.GetResult(_candidates, resultSynonimNames.ToArray());
        }

        private void HandleRelations(List<RelationNode> relationNodes)
        {
            foreach (RelationNode relationNode in relationNodes)
            {
                if (relationNode.RelationType == RelationType.PARENT)
                    HandleParent(relationNode);
                else if (relationNode.RelationType == RelationType.FOLLOWS)
                    HandleFollows(relationNode);
                else if (relationNode.RelationType == RelationType.FOLLOWS_STAR)
                    HandleFollowsStar(relationNode);
                else if (relationNode.RelationType == RelationType.PARENT_STAR)
                    HandleParentStar(relationNode);
                else if (relationNode.RelationType == RelationType.MODIFIES)
                    HandleModifies(relationNode);
                else if (relationNode.RelationType == RelationType.USES)
                    HandleUses(relationNode);
                else if (relationNode.RelationType == RelationType.NEXT)
                    HandleNext(relationNode);
                else if (relationNode.RelationType == RelationType.NEXT_STAR)
                    HandleNextStar(relationNode);
                else if (relationNode.RelationType == RelationType.CALLS)
                    HandleCalls(relationNode);
                else if (relationNode.RelationType == RelationType.CALLS_STAR)
                    HandleCallsStar(relationNode);
                //affects

                _resultTable.SetQueryHasRelations();
            }
        }

        private void HandleAffectsStar(RelationNode relationAffectsNode)
        {
            ArgumentNode arg1 = relationAffectsNode.Arguments[0];
            ArgumentNode arg2 = relationAffectsNode.Arguments[1];

            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetAffects_(int.Parse(line)).Select(s => s.ProgramLine.ToString());
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
                List<string> result = _pkbStore.GetAffected_(int.Parse(arg2.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result = _pkbStore.GetAffects_(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = _pkbStore.GetAffects_(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void HandleAffects(RelationNode relationAffectsNode)
        {
            ArgumentNode arg1 = relationAffectsNode.Arguments[0];
            ArgumentNode arg2 = relationAffectsNode.Arguments[1];

            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetAffects(int.Parse(line)).Select(s => s.ProgramLine.ToString());
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
                List<string> result = _pkbStore.GetAffected(int.Parse(arg2.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result = _pkbStore.GetAffects(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = _pkbStore.GetAffects(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void HandleCallsStar(RelationNode relationCallsNode)
        {
            ArgumentNode arg1 = relationCallsNode.Arguments[0];
            ArgumentNode arg2 = relationCallsNode.Arguments[1];

            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetCalls_(line);
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
                List<string> result = _pkbStore.GetCalled_(arg2.Value).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result = _pkbStore.GetCalls_(arg1.Value).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = _pkbStore.GetCalls_(arg1.Value).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void HandleCalls(RelationNode relationCallsNode)
        {
            ArgumentNode arg1 = relationCallsNode.Arguments[0];
            ArgumentNode arg2 = relationCallsNode.Arguments[1];

            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetCalls(line);
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
                List<string> result = _pkbStore.GetCalled(arg2.Value).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result = _pkbStore.GetCalls(arg1.Value).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = _pkbStore.GetCalls(arg1.Value).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void HandleNextStar(RelationNode relationNextNode)
        {
            ArgumentNode arg1 = relationNextNode.Arguments[0];
            ArgumentNode arg2 = relationNextNode.Arguments[1];

            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetNext_(int.Parse(line)).Select(s => s.ProgramLine.ToString());
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
                List<string> result = _pkbStore.GetPrevious_(int.Parse(arg2.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result = _pkbStore.GetNext_(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = _pkbStore.GetNext_(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void HandleNext(RelationNode relationNextNode)
        {
            ArgumentNode arg1 = relationNextNode.Arguments[0];
            ArgumentNode arg2 = relationNextNode.Arguments[1];

            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetNext(int.Parse(line)).Select(s => s.ProgramLine.ToString());
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
                List<string> result = _pkbStore.GetPrevious(int.Parse(arg2.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result = _pkbStore.GetNext(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = _pkbStore.GetNext(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void HandleFollowsStar(RelationNode relationFollowsNode)
        {
            ArgumentNode arg1 = relationFollowsNode.Arguments[0];
            ArgumentNode arg2 = relationFollowsNode.Arguments[1];
            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetFollows_(int.Parse(line)).Select(s => s.ProgramLine.ToString());
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
                List<string> result = _pkbStore.GetFollowed_(int.Parse(arg2.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result = _pkbStore.GetFollows_(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = _pkbStore.GetFollows_(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
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

        private void HandleParentStar(RelationNode relationParentNode)
        {
            ArgumentNode arg1 = relationParentNode.Arguments[0];
            ArgumentNode arg2 = relationParentNode.Arguments[1];
            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetChildren_(int.Parse(line)).Select(s => s.ProgramLine.ToString());
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
                List<string> parents = _pkbStore.GetParents_(int.Parse(arg2.Value), ExpressionType.NULL).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in parents)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !parents.Contains(w)).ToList(); ;
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> children = _pkbStore.GetChildren_(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg2Line in children)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !children.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> children = _pkbStore.GetChildren_(int.Parse(arg1.Value)).Select(s => s.ProgramLine.ToString()).ToList();
                if (!children.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }


        private void HandleModifies(RelationNode relationModifiesNode)
        {
            ArgumentNode arg1 = relationModifiesNode.Arguments[0];
            ArgumentNode arg2 = relationModifiesNode.Arguments[1];
            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetModifies(int.Parse(line));
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
                List<string> result = new List<string>();
                if (int.TryParse(arg2.Value, out int progLine)) 
                    result = _pkbStore.GetModified(progLine).Select(s => s.ProgramLine.ToString()).ToList();
                else
                    result = _pkbStore.GetModified(arg2.Value, ExpressionType.PROCEDURE).Select(s => s.ProgramLine.ToString()).ToList();// do zamiany metoda GetModified
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {

                List<string> result = new List<string>();
                if (arg1.RelationArgumentType == RelationArgumentType.Integer)
                    result = _pkbStore.GetModifies(Convert.ToInt32(arg1.Value)).ToList();
                else
                    result = _pkbStore.GetModifies(arg1.Value).ToList();
                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result = new List<string>();
                if (int.TryParse(arg1.Value, out int progLine))
                    result = _pkbStore.GetModifies(progLine).ToList();
                else
                    result = _pkbStore.GetModifies(arg1.Value).ToList();
                if (!result.Contains(arg2.Value))
                    _resultTable.SetFalseBoolResult();
            }
        }

        private void HandleUses(RelationNode relationUsesNode)
        {
            ArgumentNode arg1 = relationUsesNode.Arguments[0];
            ArgumentNode arg2 = relationUsesNode.Arguments[1];
            if (_candidates.ContainsKey(arg1.Name) && _candidates.ContainsKey(arg2.Name))
            {
                List<string> arg1CandidatesToRemove = new List<string>();
                List<string> arg2Candidates = new List<string>();
                foreach (var line in _candidates[arg1.Name])
                {
                    var result = _pkbStore.GetUsed(int.Parse(line));
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
                List<string> result = _pkbStore.GetUses(arg2.Value).Select(s => s.ProgramLine.ToString()).ToList();
                foreach (string arg1Line in result)
                    _resultTable.AddRelationResult(arg1.Name, arg1Line);

                List<string> arg1CandidatesToRemove = _candidates[arg1.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg1.Name, arg1CandidatesToRemove);
            }
            else if (_candidates.ContainsKey(arg2.Name))
            {
                List<string> result;
                if (arg1.RelationArgumentType == RelationArgumentType.Integer)
                    result = _pkbStore.GetUsed(int.Parse(arg1.Value)).ToList();
                else
                    result = _pkbStore.GetUsed(arg1.Value).ToList();

                foreach (string arg2Line in result)
                    _resultTable.AddRelationResult(arg2.Name, arg2Line);

                List<string> arg2CandidatesToRemove = _candidates[arg2.Name].Where(w => !result.Contains(w)).ToList();
                RemoveCandidates(arg2.Name, arg2CandidatesToRemove);
            }
            else
            {
                List<string> result;
                if (arg1.RelationArgumentType == RelationArgumentType.Integer)
                    result = _pkbStore.GetUsed(int.Parse(arg1.Value)).ToList();
                else
                    result = _pkbStore.GetUsed(arg1.Value).ToList();

                if (!result.Contains(arg2.Value))
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

                    if (expressionType == ExpressionType.VAR)
                        _candidates[declarationsPair.Key].AddRange(_pkbStore.VarList);
                    else if (expressionType == ExpressionType.PROCEDURE)
                        _candidates[declarationsPair.Key].AddRange(_pkbStore.ProcList);
                    else if (expressionType == ExpressionType.CONST)
                        _candidates[declarationsPair.Key].AddRange(_pkbStore.ConstList);
                    else
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

        private void ApplyWithRestrictions(List<AttributeNode> attributeNodes)
        {
            foreach(AttributeNode attributeNode in attributeNodes)
            {
                if(attributeNode.AttributeValue is string attributeStringValue)
                {
                    List<string> candidatesToRemove = _candidates[attributeNode.SynonimNode.Name].Where(w => w != attributeStringValue).ToList();
                    RemoveCandidates(attributeNode.SynonimNode.Name, candidatesToRemove);
                }
                else if(attributeNode.AttributeValue is AttributeNode relatedAttributeNode)
                {
                    List<string> intersection = _candidates[attributeNode.SynonimNode.Name].Intersect(_candidates[relatedAttributeNode.SynonimNode.Name]).ToList();
                    List<string> firstAttributeCandidatesToRemove = _candidates[attributeNode.SynonimNode.Name].Where(w => !intersection.Contains(w)).ToList();
                    List<string> secondtAttributeCandidatesToRemove = _candidates[relatedAttributeNode.SynonimNode.Name].Where(w => !intersection.Contains(w)).ToList();

                    RemoveCandidates(attributeNode.SynonimNode.Name, firstAttributeCandidatesToRemove);
                    RemoveCandidates(relatedAttributeNode.SynonimNode.Name, secondtAttributeCandidatesToRemove);
                }
            }
        }
    }
}
