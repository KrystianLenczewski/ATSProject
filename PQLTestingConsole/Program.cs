﻿using QueryProcessor;
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
            string query = "variable v; stmt s,s2,s3; if ifstat; assign a; while w; select v,s such that Modifies (\"Rectangle\",v) and Follows* (s,6)";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            var result = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
