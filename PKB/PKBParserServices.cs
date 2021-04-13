using Shared;
using System;
using System.Collections.Generic;

namespace PKB
{
    public static class PKBParserServices
    {
        public static void SetFollows(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {
            var stmt1 = new List<ExpressionType> { ExpressionType.WHILE, ExpressionType.ASSIGN }.Contains(s1.Type);
            var stmt2 = new List<ExpressionType> { ExpressionType.WHILE, ExpressionType.ASSIGN }.Contains(s2.Type);
            if (!(stmt1 && stmt2)) return;
            var value = KeyValuePair.Create(s1, s2);
            pkb.FollowsList.Add(value);
        }

        public static void SetParent(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2, int index)
        {
            if (IsValidParentProcedureOrWhile(s1.Type, s2.Type, index) || IsValidParentStmtLst(s1.Type, s2.Type, index) || IsValidAssign(s1.Type, s2.Type, index) || IsValidOperation(s1.Type, s2.Type))
            {
                pkb.ParentList.Add(new ParentModel(s1, s2, index));
            }
        }

        public static void SetModify(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {
            // TODO: VALIDATION
            pkb.ModifiesList.Add(new KeyValuePair<ExpressionModel, ExpressionModel>(s1, s2));
        }

        public static void SetUses(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {
            // TODO: VALIDATION
            pkb.UsesList.Add(new KeyValuePair<ExpressionModel, ExpressionModel>(s1, s2));
        }

        private static bool ExpressionName(int index, ExpressionType childType) => (index == 0 && childType == ExpressionType.VAR);
        private static bool IsDefinedIn(Type type, ExpressionType childType) => Enum.IsDefined(type, (int)childType);
        private static bool IsValidParentProcedureOrWhile(ExpressionType parentType, ExpressionType childType, int index) => 
            ((parentType == ExpressionType.PROCEDURE) || (parentType == ExpressionType.WHILE)) && (ExpressionName(index,childType) || (index == 1 && childType == ExpressionType.STMTLST));
        private static bool IsValidParentStmtLst(ExpressionType parentType, ExpressionType childType, int index) => 
            (parentType == ExpressionType.STMTLST) && IsDefinedIn(typeof(StatementType), childType) && (index == 0);
        private static bool IsValidAssign(ExpressionType parentType, ExpressionType childType, int index) => 
            (parentType == ExpressionType.ASSIGN) && (ExpressionName(index, childType) || (index == 1 && IsDefinedIn(typeof(OperationsType), childType)));
        private static bool IsValidOperation(ExpressionType parentType, ExpressionType childType) =>
            IsDefinedIn(typeof(OperationsType), parentType) && IsDefinedIn(typeof(FactorType), childType);

    }
}
