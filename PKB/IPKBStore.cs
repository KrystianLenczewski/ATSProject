using Shared;
using System.Collections.Generic;

namespace PKB
{
    public interface IPKBStore
    {
        void SetFollows(ExpressionModel s1, ExpressionModel s2);
        void SetParent(ExpressionModel s1, ExpressionModel s2);

        List<KeyValuePair<ExpressionModel, ExpressionModel>> ModifiesList { get; }
        List<KeyValuePair<ExpressionModel, ExpressionModel>> FollowsList { get; }
        List<ParentModel> ParentList { get; }
    }
}
