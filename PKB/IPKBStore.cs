using Shared;

namespace PKB
{
    interface IPKBStore
    {
        void SetFollows(Statement s1, Statement s2);
        void SetParent(Statement s1, Statement s2);
    }
}
