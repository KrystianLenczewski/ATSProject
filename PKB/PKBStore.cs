using PKB.Relations;
using Shared;
using System.Collections.Generic;

namespace PKB
{
    public class PKBStore
    {
        private static readonly PKBStore _instance;
        private List<Modifies> listModifies;
        public List<Modifies> ListModifies { get=> listModifies; private set=>listModifies=value; }
        private List<Follows> listFollows;
        public List<Follows> ListFollows { get => listFollows; private set => listFollows = value; }
        private List<FollowStar> listFollowStar;
        public List<FollowStar> ListFollowStar { get => listFollowStar; private set => listFollowStar = value; }
        private List<Parent> listParent;
        public List<Parent> ListParent { get => listParent; private set => listParent = value; }
        private List<ParentStar> listParentStar;
        public List<ParentStar> ListParentStar { get => listParentStar; private set => listParentStar = value; }
        private List<Uses> listUses;
        public List<Uses> ListUses { get => listUses; private set => listUses = value; }


        private PKBStore() { }

        static PKBStore() => _instance = new PKBStore();

        public static PKBStore Instance { get { return _instance; } }

    

        #region pql services
        // TODO: ...
        #endregion
    }
}
