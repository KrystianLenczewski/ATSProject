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
            string query = "assign a,a1; stmt s,s1; variable v; procedure p; constant c; select p such that Modifies (p,v) with p.procName=\"proce\"";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            List<object> queryResultsRaw = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
