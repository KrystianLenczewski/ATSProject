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
            string query = "stmt s,s2,s3; if ifstat; assign a; while w; select s such that Parent (8,s)";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            var result = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
