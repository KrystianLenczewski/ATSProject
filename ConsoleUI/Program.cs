using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PKB;
using QueryProcessor;
using SPAFrontend;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryPreprocessor queryPreprocessor = new QueryPreprocessor();
            string query = "assign s,s1; variable v; select s such that Modifies (s,v)";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            List<object> queryResultsRaw = queryEvaluator.GetQueryResultsRaw(queryTree);

        }
    }
}