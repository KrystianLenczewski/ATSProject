using Shared;
using System.Collections.Generic;

namespace PKB
{
    public class PKBStore : IPKBStore
    {
        public List<KeyValuePair<Expression, Expression>> ModifiesList { get; private set; } = new List<KeyValuePair<Expression, Expression>>();
        public List<KeyValuePair<Expression, Expression>> FollowsList { get; private set; } = new List<KeyValuePair<Expression, Expression>>();
        public List<KeyValuePair<Expression, Expression>> ParentList { get; private set; } = new List<KeyValuePair<Expression, Expression>>();

        #region parser services
        public void SetFollows(Expression s1, Expression s2) => this.SetFollowsAction(s1, s2);
        public void SetParent(Expression s1, Expression s2) => this.SetParentAction(s1, s2);
        // TODO: ...
        #endregion

        #region pql services
        // TODO: ...
        #endregion
    }
}
