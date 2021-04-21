using QueryProcessor.Enums;
using QueryProcessor.QueryTreeNodes;
using Shared.PQLModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.Infrastructure
{
    public static class IntegrationModelCreator
    {
        public static Statement CreateStatementForArgumentNode(ArgumentNode argumentNode)
        {
            if (argumentNode.RelationArgumentType == RelationArgumentType.Integer)
                return new Statement(int.Parse(argumentNode.Value));

            if (argumentNode.RelationArgumentType == RelationArgumentType.Assign)
                return new Assign();
            else if (argumentNode.RelationArgumentType == RelationArgumentType.If)
                return new StatementIf();
            else if (argumentNode.RelationArgumentType == RelationArgumentType.While)
                return new StatementWhile();
            else if (argumentNode.RelationArgumentType == RelationArgumentType.Call)
                return new Call();
            else
                return new Statement();
        }

        public static Variable CreateVariableForArgumentNode(ArgumentNode argumentNode)
        {
            if (argumentNode.RelationArgumentType == RelationArgumentType.String)
                return new Variable(argumentNode.Value);
            return new Variable();
        }
        public static Procedure CreateProcedureForArgumentNode(ArgumentNode argumentNode)
        {
            if (argumentNode.RelationArgumentType == RelationArgumentType.Procedure)
                return new Procedure(argumentNode.Value);
            return new Procedure();
        }
    }
}
