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
          
            string query = "procedure p,q; while w1; select p with p.procName=\"Second\" such that Modifies (p,\"z\") and Calls (p,q)";
            //select p with p.procName=\"Second\" such that Calls(p,q) such that Modifies() with 
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            string queryResultsRaw = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
