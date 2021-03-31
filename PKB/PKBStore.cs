using Shared;
using System.Collections.Generic;

namespace PKB
{
    public class PKBStore : IPKBStore
    {
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> ModifiesList { get; private set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> FollowsList { get; private set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
        public List<ParentModel> ParentList { get; private set; } = new List<ParentModel>();

        #region parser services
        public void SetFollows(ExpressionModel s1, ExpressionModel s2) => this.SetFollowsAction(s1, s2);
        public void SetParent(ExpressionModel s1, ExpressionModel s2) => this.SetParentAction(s1, s2);
        // TODO: ...
        #endregion

        #region pql services
        // TODO: ...
        #endregion
    }
}
