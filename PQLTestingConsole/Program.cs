using QueryProcessor;
using QueryProcessor.Moqs;
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
            string query = "variable v; stmt s,s2,s3; if ifstat; assign a; while w; select BOOLEAN such that Parent (s,s2) with s.stmt#=8 and s2.stmt#=11";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator(PKBInitializer.InitializePKB());
            var result = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
