using QueryProcessor;

namespace PQLTestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryPreprocessor = new QueryPreprocessor();
            string query = "stmt s,s1; select s with s1.stmt#=11";
            var queryTree = queryPreprocessor.ParseQuery(query);
        }
    }
}
