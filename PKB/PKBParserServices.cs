using Shared;
using System.Collections.Generic;

namespace PKB
{
    public static class PKBParserServices
    {
        public static void SetFollows(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.FollowsList.Add(KeyValuePair.Create(s1, s2));

        public static void SetParent(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2, int index) => pkb.ParentList.Add(new ParentModel(s1, s2, index));

        public static void SetModify(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.ModifiesList.Add(KeyValuePair.Create(s1, s2));

        public static void SetUses(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2) => pkb.UsesList.Add(KeyValuePair.Create(s1, s2));
    }
}
