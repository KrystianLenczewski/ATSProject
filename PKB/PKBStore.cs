using Shared;
using System.Collections.Generic;

namespace PKB
{
    public class PKBStore : IPKBStore
    {
        private static readonly PKBStore _instance;
        private List<KeyValuePair<Statement, Statement>> ModifiesList { get; set; }
        private List<KeyValuePair<Statement, Statement>> FollowsList { get; set; }

        private List<KeyValuePair<Statement, Statement>> ParentList { get; set; }

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
