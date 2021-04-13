using Shared;
using System.Collections.Generic;

namespace PKB
{
    public interface IPKBStore
    {
        List<KeyValuePair<ExpressionModel, ExpressionModel>> ModifiesList { get; set; }
        List<KeyValuePair<ExpressionModel, ExpressionModel>> FollowsList { get; set; }
        List<KeyValuePair<ExpressionModel, ExpressionModel>> UsesList { get; set; }
        List<ParentModel> ParentList { get; set; }
    }
}
