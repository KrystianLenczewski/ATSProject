using PKB;
using QueryProcessor.Enums;
using QueryProcessor.Infrastructure;
using QueryProcessor.QueryTreeNodes;
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
        private readonly ResultProcessor _resultProcessor;

        public QueryEvaluator()
        {
            //_pkbStore = PKBStore.Instance;
            //_resultProcessor = new ResultProcessor(_pkbStore);
        }

        public string GetQueryResultsRaw(QueryTree queryTree)
        {
            queryTree.PrepareTreeForQueryEvaluator();
            List<RelationNode> relationNodes = queryTree.GetRelationNodes();
            List<SynonimNode> resultChildrens = queryTree.GetResultNodeChildrens();

            bool isResultBoolean = queryTree.IsResultBoolean();
            HandleRelations(relationNodes, isResultBoolean, resultChildrens.FirstOrDefault());

            return _resultProcessor.GetResult(resultChildrens.Select(s=>s.SynonimType).ToArray());
        }

        private void HandleRelations(List<RelationNode> relationNodes, bool isResultBoolean, SynonimNode resultSynonim)
        {
            foreach (RelationNode item in relationNodes)
            {
                switch (item.RelationType)
                {
                    case RelationType.MODIFIES:
                        HandleModifies(item, isResultBoolean, resultSynonim);
                        break;
                    case RelationType.FOLLOWS:
                        HandleFollows(item, isResultBoolean, resultSynonim);
                        break;
                    case RelationType.FOLLOWS_STAR:
                        HandleFollowStar(item, isResultBoolean, resultSynonim);
                        break;
                    default:
                        break;
                }
                //Modifies(s1,s2)
            }
        }

        private void HandleModifies(RelationNode relationNode, bool isResultBoolean, SynonimNode resultChildren)
        {

            if (isResultBoolean)
            {
                Variable variable = IntegrationModelCreator.CreateVariableForArgumentNode(relationNode.Arguments[1]);
                if (relationNode.Arguments[0].IsStatement())
                {

                    Statement statement = IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]);
                    // _resultProcessor.AddRelationResult(_pkbStore.IsModified(variable, statement));
                }
                else
                {
                    Procedure procedure = IntegrationModelCreator.CreateProcedureForArgumentNode(relationNode.Arguments[0]);
                    // _resultProcessor.AddRelationResult(_pkbStore.IsModified(variable, procedure));
                }

            }
            else
            {
                if (resultChildren.SynonimType == SynonimType.Variable && relationNode.Arguments[0].IsStatement())
                {
                    // List<Variable> variables = _pkbStore.GetModified(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]));
                    // _resultProcessor.AddRelationResult(SynonimType.Variable, variables);
                }
                else if (resultChildren.SynonimType == SynonimType.Variable && relationNode.Arguments[0].RelationArgumentType == RelationArgumentType.Procedure)
                {
                    // List<Variable> variables = _pkbStore.GetModified(IntegrationModelCreator.CreateProcedureForArgumentNode(relationNode.Arguments[0]));
                    // _resultProcessor.AddRelationResult(SynonimType.Variable, variables);
                }
                else if (resultChildren.IsStamement() && (relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.Variable ||
                    relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.String))
                {
                    // List<Statement> statements = _pkbStore.GetModifiesStatements(IntegrationModelCreator.CreateVariableForArgumentNode(relationNode.Arguments[1]));
                    // _resultProcessor.AddRelationResult(SynonimType.Statement, statements);
                }
                else if (resultChildren.SynonimType == SynonimType.Procedure && (relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.Variable ||
                    relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.String))
                {
                    // List<Procedure> procedures = _pkbStore.GetModifiesProcedures(IntegrationModelCreator.CreateVariableForArgumentNode(relationNode.Arguments[1]));
                    // _resultProcessor.AddRelationResult(SynonimType.Procedure, procedures);
                }

            }
        }

        private void HandleFollows(RelationNode relationNode, bool isResultBoolean, SynonimNode resultChildren)
        {
            if (isResultBoolean)
            {
                Statement stmt1 = IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]);
                Statement stmt2 = IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[1]);
                // _resultProcessor.AddRelationResult(_pkbStore.IsFollows(stmt1, stmt2));
            }
            else
            {
                if (relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.Integer)
                {
                    // Statement statement = _pkbStore.GetFollows(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[1]));
                    // _resultProcessor.AddRelationResult(SynonimType.Statement, new List<Statement> { statement });
                }
                else if (relationNode.Arguments[0].RelationArgumentType == RelationArgumentType.Integer)
                {
                    // Statement statement = _pkbStore.GetFollowed(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]));
                    // _resultProcessor.AddRelationResult(SynonimType.Statement, new List<Statement> { statement });
                }
            }
        }

        private void HandleFollowStar(RelationNode relationNode, bool isResultBoolean, SynonimNode resultChildren)
        {
            if (relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.Integer)
            {
                // List<Statement> statements = _pkbStore.GetFollowsStar(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[1]));
                // _resultProcessor.AddRelationResult(SynonimType.Statement, statements);
            }
            else if (relationNode.Arguments[0].RelationArgumentType == RelationArgumentType.Integer)
            {
                // List<Statement> statements = _pkbStore.GetFollowedStar(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]));
                // _resultProcessor.AddRelationResult(SynonimType.Statement, statements);
            }
        }
    }

    //zwracala varset, stmtset,
    //private  GetResultForModifies
    //getModified (STMT stmt) - mozemy przekazac stmt bez typu, albo z typem, 
    //getModifies (VAR var) - mozemy podac tylko anzwe zmiennej 
    //pamietaj ze mozemy wyslac tylko linie programu bez typu 
}
