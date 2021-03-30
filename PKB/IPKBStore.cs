using Shared;
using System.Collections.Generic;

namespace PKB
{
    public interface IPKBStore
    {
        void SetFollows(Expression s1, Expression s2);
        void SetParent(Expression s1, Expression s2);

        List<KeyValuePair<Expression, Expression>> ModifiesList { get; }
        List<KeyValuePair<Expression, Expression>> FollowsList { get; }
        List<KeyValuePair<Expression, Expression>> ParentList { get; }
    }
}
