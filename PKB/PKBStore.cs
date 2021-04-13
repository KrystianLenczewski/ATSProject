using Shared;
using System.Collections.Generic;

namespace PKB
{
    public class PKBStore : IPKBStore
    {
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> ModifiesList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> FollowsList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> UsesList { get; set; }
        public List<ParentModel> ParentList { get; set; } = new List<ParentModel>();
    }
}
