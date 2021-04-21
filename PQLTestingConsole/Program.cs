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
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            // List<object> queryResultsRaw = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
