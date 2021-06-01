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
            string query = "stmt s,s2,s3; select s,s2,s3 such that Parent (s,s2) and Parent (s2,s3)";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            var result = queryEvaluator.GetQueryResultsRaw(queryTree);

            //ResultTable resultTable = new ResultTable(new List<string> { "s1", "s2", "s3", "w1" });
            //resultTable.AddRelationResult("s1", "3", "w1", "5");

            //resultTable.AddRelationResult("w1", "5", "s2", "6");
            //resultTable.AddRelationResult("w1", "5", "s2", "7");
            //resultTable.AddRelationResult("w1", "5", "s2", "10");

            //resultTable.RemoveCandidate("s2", "6");
            //resultTable.RemoveCandidate("s2", "10");
            //resultTable.AddRelationResult("s2", "7", "s3", "8");
            //resultTable.AddRelationResult("s2", "7", "s3", "9");

            //resultTable.AddRelationResult("w1", "55");
            //resultTable.DisplayTable();
        }
    }
}
