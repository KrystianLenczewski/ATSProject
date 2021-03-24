using Shared;
using System.Collections.Generic;

namespace PKB
{
    public class PKBStore : IPKBStore
    {
        private static readonly PKBStore _instance;
        public List<KeyValuePair<Statement, Statement>> ModifiesList { get; private set; }
        public List<KeyValuePair<Statement, Statement>> FollowsList { get; private set; }

        public List<KeyValuePair<Statement, Statement>> ParentList { get; private set; }

        private PKBStore() { }

        static PKBStore() => _instance = new PKBStore();

        public static PKBStore Instance { get { return _instance; } }

        #region parser services
        public void SetFollows(Statement s1, Statement s2) => _instance.SetFollows(s1, s2);

        public void SetParent(Statement s1, Statement s2) => _instance.SetParent(s1, s2);

        // TODO: ...
        #endregion

        #region pql services
        // TODO: ...
        #endregion
    }
}
