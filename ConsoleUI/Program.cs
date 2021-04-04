using System;
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
            string query = "assign a;variable v;procedure p; select p,a,v such that Follows (v,a)";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
        }
    }
}