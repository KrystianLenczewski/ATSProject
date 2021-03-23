namespace PKB
{
    public class PKBStore
    {
        private static readonly PKBStore _instance;

        private PKBStore() { }

        static PKBStore() => _instance = new PKBStore();

        public static PKBStore Instance { get { return _instance; } }
    }
}
