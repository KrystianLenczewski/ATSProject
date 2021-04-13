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

        public static void SetParent(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {
            throw new NotImplementedException();
        }

        public static void SetParent(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2, int index)
        {
            var value = KeyValuePair.Create(s1, s2);
            if (IsValidParentProcedure(s1.Type, s2.Type, index) || IsValidParentStmtLst(s1.Type, s2.Type, index))
            {
                pkb.ParentList.Add(new ParentModel(s1,s2,index));
            }

            // TODO: ...
        }

        public static void SetModify(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {

        }

        public static void SetUses(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {

        }

        private static bool IsValidParentProcedure(ExpressionType parentType, ExpressionType childType, int index) => (parentType == ExpressionType.PROCEDURE) && ((index == 0 && childType == ExpressionType.VAR) || (index == 1 && childType == ExpressionType.STMTLST));
        private static bool IsValidParentStmtLst(ExpressionType parentType, ExpressionType childType, int index) => (parentType == ExpressionType.STMTLST) && Enum.IsDefined(typeof(StatementType), (int)childType) && (index == 0);

    }
}
