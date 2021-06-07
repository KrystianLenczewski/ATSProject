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
            string query = "select BOOLEAN such that Next* (9,9)";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator(PKBInitializer.InitializePKB());
            var result = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
