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
