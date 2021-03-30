using Shared;
using System;
using System.Collections.Generic;

namespace PKB
{
    public static class PKBParserServices
    {
        public static void SetFollowsAction(this IPKBStore pkb, Expression s1, Expression s2)
        {
            var stmt1 = new List<ExpressionType> { ExpressionType.WHILE, ExpressionType.ASSIGN }.Contains(s1.Type);
            var stmt2 = new List<ExpressionType> { ExpressionType.WHILE, ExpressionType.ASSIGN }.Contains(s2.Type);
            if (!(stmt1 && stmt2)) return;
            var value = KeyValuePair.Create(s1, s2);
            pkb.FollowsList.Add(value);
        }

        public static void SetParentAction(this IPKBStore pkb, Expression s1, Expression s2)
        {
            throw new NotImplementedException();
        }

        public static void SetParentAction(this IPKBStore pkb, Expression s1, Expression s2, int index)
        {
            var value = KeyValuePair.Create(s1, s2);
            if (s1.Type == ExpressionType.PROCEDURE && IsValidProcedure(s2.Type, index))
            {
                pkb.ParentList.Insert(index, value);
                s2.Index = index;
            }
            else if (s1.Type == ExpressionType.STMTLST && IsValidStmtLst(s2.Type, index))
            {
                pkb.ParentList.Insert(index, value);
                s2.Index = index;
            }

            // TODO: ...
        }

        private static bool IsValidProcedure(ExpressionType type, int index) => (index == 0 && type == ExpressionType.CONST) || (index == 1 && type == ExpressionType.STMTLST);
        private static bool IsValidStmtLst(ExpressionType type, int index) => (index == 0 && (type == ExpressionType.ASSIGN || type == ExpressionType.WHILE));

    }
}
