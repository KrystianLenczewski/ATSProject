using Shared;

namespace PKB
{
    public class PKBStore : IPKBStore
    {
        private static readonly PKBStore _instance;

        private PKBStore() { }

        static PKBStore() => _instance = new PKBStore();

        public static PKBStore Instance { get { return _instance; } }

        public void SetFollows(Statement s1, Statement s2)
        {
            throw new System.NotImplementedException();
        }

        public void SetParent(Statement s1, Statement s2)
        {
            throw new System.NotImplementedException();
        }
    }
}
