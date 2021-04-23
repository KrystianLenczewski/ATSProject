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

        public QueryEvaluator()
        {
            //_pkbStore = PKBStore.Instance;
        }

        public List<object> GetQueryResultsRaw(QueryTree queryTree)
        {
            queryTree.PrepareTreeForQueryEvaluator();
            List<RelationNode> relationNodes = queryTree.GetRelationNodes();
            List<SynonimNode> resultChildrens = queryTree.GetResultNodeChildrens();

            bool isResultBoolean = queryTree.IsResultBoolean();
            List<object> results = HandleRelations(relationNodes, isResultBoolean, resultChildrens.FirstOrDefault());

            return results;


        }

        private List<object> HandleRelations(List<RelationNode> relationNodes, bool isResultBoolean, SynonimNode resultSynonim)
        {
            List<object> results = new List<object>();

            foreach (RelationNode item in relationNodes)
            {
                switch (item.RelationType)
                {
                    case RelationType.MODIFIES:
                        results.Add(GetModifies(item, isResultBoolean, resultSynonim));
                        break;
                    case RelationType.FOLLOWS:
                        results.Add(GetFollows(item, isResultBoolean, resultSynonim));
                        break;
                    case RelationType.FOLLOWS_STAR:
                        results.Add(GetFollowStar(item, isResultBoolean, resultSynonim));
                        break;
                    default:
                        break;
                }
                //Modifies(s1,s2)


            }
            return results;

        }

        private object GetModifies(RelationNode relationNode, bool isResultBoolean, SynonimNode resultChildren)
        {

            if (isResultBoolean)
            {
                Variable variable = IntegrationModelCreator.CreateVariableForArgumentNode(relationNode.Arguments[1]);
                if (relationNode.Arguments[0].IsStatement())
                {

                    Statement statement = IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]);
                    return _pkbStore.IsModified(variable, statement);
                }
                else
                {
                    Procedure procedure = IntegrationModelCreator.CreateProcedureForArgumentNode(relationNode.Arguments[0]);
                    return _pkbStore.IsModified(variable, procedure);
                }

            }
            else
            {
                if (resultChildren.SynonimType == SynonimType.Variable && relationNode.Arguments[0].IsStatement())
                {
                    return _pkbStore.GetModified(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]));
                }
                else if (resultChildren.SynonimType == SynonimType.Variable && relationNode.Arguments[0].RelationArgumentType == RelationArgumentType.Procedure)
                {
                    return _pkbStore.GetModified(IntegrationModelCreator.CreateProcedureForArgumentNode(relationNode.Arguments[0]));
                }
                else if (resultChildren.IsStamement() && (relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.Variable ||
                    relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.String))
                {
                    return _pkbStore.GetModifiesStatements(IntegrationModelCreator.CreateVariableForArgumentNode(relationNode.Arguments[1]));
                }
                else if (resultChildren.SynonimType == SynonimType.Procedure && (relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.Variable ||
                    relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.String))
                {
                    return _pkbStore.GetModifiesProcedures(IntegrationModelCreator.CreateVariableForArgumentNode(relationNode.Arguments[1]));
                }

            }

            return new List<Statement>();

        }

        private object GetFollows(RelationNode relationNode, bool isResultBoolean, SynonimNode resultChildren)
        {
            if (isResultBoolean)
            {
                Statement stmt1 = IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]);
                Statement stmt2 = IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[1]);
                return _pkbStore.IsFollows(stmt1, stmt2);
            }
            else
            {
                if (relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.Integer)
                {
                    return _pkbStore.GetFollows(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[1]));
                }
                else if (relationNode.Arguments[0].RelationArgumentType == RelationArgumentType.Integer)
                {
                    return _pkbStore.GetFollowed(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]));
                }

            }
            return null;
        }

        private object GetFollowStar(RelationNode relationNode, bool isResultBoolean, SynonimNode resultChildren)
        {
            if (relationNode.Arguments[1].RelationArgumentType == RelationArgumentType.Integer)
            {
                return _pkbStore.GetFollowsStar(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[1]));
            }
            else if (relationNode.Arguments[0].RelationArgumentType == RelationArgumentType.Integer)
            {
                return _pkbStore.GetFollowedStar(IntegrationModelCreator.CreateStatementForArgumentNode(relationNode.Arguments[0]));
            }
            return null;
        }
    }

    //zwracala varset, stmtset,
    //private  GetResultForModifies
    //getModified (STMT stmt) - mozemy przekazac stmt bez typu, albo z typem, 
    //getModifies (VAR var) - mozemy podac tylko anzwe zmiennej 
    //pamietaj ze mozemy wyslac tylko linie programu bez typu 
}
