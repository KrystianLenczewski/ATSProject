using QueryProcessor;
using QueryProcessor.QueryProcessing;
using QueryProcessor.ResultGeneration;
using System.Collections.Generic;

namespace PQLTestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryPreprocessor = new QueryPreprocessor();
            string query = "stmt s,s2,s3; select s,s2,s3 such that Parent (s,s2) and Parent (s2,11)";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            var result = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
