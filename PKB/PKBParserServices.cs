using Shared;
using SPAFrontend.mdoels;
using System.Collections.Generic;

namespace PKB
{
    public static class PKBParserServices
    {
        public static void SetFollows(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.FollowsList.Add(KeyValuePair.Create(s1, s2));

        public static void SetParent(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2, int index) => pkb.ParentList.Add(new ParentModel(s1, s2, index));

        public static void SetModify(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.ModifiesList.Add(KeyValuePair.Create(s1, s2));

        public static void SetUses(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.UsesList.Add(KeyValuePair.Create(s1, s2));
        public static void SetNext(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.NextList.Add(KeyValuePair.Create(s1, s2));
        public static void SetAffects(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.AffectsList.Add(KeyValuePair.Create(s1, s2));
        public static void SetCalls(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.CallsList.Add(KeyValuePair.Create(s1, s2));
        public static void SetCallsRange(this IPKBStore pkb, HashSet<KeyValuePair<ExpressionModel, ExpressionModel>> s1) => pkb.CallsList.AddRange(s1);
        public static void SetConstRange(this IPKBStore pkb, HashSet<string> s1) => pkb.ConstList.AddRange(s1);
        public static void SetProcList(this IPKBStore pkb, string pr) => pkb.ProcList.Add(pr);
        public static void SetVarList(this IPKBStore pkb, HashSet<string> pr) => pkb.VarList.AddRange(pr);

        public static void SetAllStatements(this IPKBStore pkb, ExpressionModel em) => pkb.StatementList.Add(em);

        public static void RebuildParentListIndexes(this IPKBStore pkb)
        {
            List<RownumIndex> indexes = new List<RownumIndex>();
            pkb.ParentList.ForEach(elem =>
            {
                RownumIndex ri = indexes.Find(i => i.rownum == elem.Parent.Line);
                if (ri != null)
                {
                    ri.index++;
                    elem.Index = ri.index;
                }
                else
                {
                    indexes.Add(new RownumIndex(elem.Parent.Line, 1));
                    elem.Index = 1;
                }
            });
            pkb.ToString();
        }
    }
}
