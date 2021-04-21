using QueryProcessor;
using System.Collections.Generic;

namespace PQLTestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryPreprocessor = new QueryPreprocessor();
            string query = "assign s,s1; variable v; select s such that Modifies (s,v)";
            var queryTree = queryPreprocessor.ParseQuery(query);
            var queryEvaluator = new QueryEvaluator();
            List<object> queryResultsRaw = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
