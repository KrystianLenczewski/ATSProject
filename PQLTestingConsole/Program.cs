using QueryProcessor;
using QueryProcessor.QueryProcessing;
using System.Collections.Generic;

namespace PQLTestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryPreprocessor = new QueryPreprocessor();
          
            string query = "select BOOLEAN such that Next* (20,620)";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            // List<object> queryResultsRaw = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
