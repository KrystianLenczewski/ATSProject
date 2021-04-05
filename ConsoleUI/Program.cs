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
            string query = "stmt s,s1; select s with s1.stmt#=11";
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
        }
    }
}