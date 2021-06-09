using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PKB;
using SPAFrontend;
using QueryProcessor;
using QueryProcessor.Moqs;
using QueryProcessor.QueryProcessing;
using QueryProcessor.ResultGeneration;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding(852);

            if (args.Length == 1)
            {
                //string text = File.ReadAllText(args[0], Encoding.GetEncoding(852));
                string text = File.ReadAllText("simple.txt", Encoding.GetEncoding(852));
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IPKBStore, PKBStore>()
                    .BuildServiceProvider();

                var pkb = serviceProvider.GetService<IPKBStore>();
                pkb.ParseCode(text);

                Console.WriteLine("Ready");

                while (true)
                {
                    var declarations = Console.ReadLine(); // example: stmt s;
                    var command = Console.ReadLine(); // example: Select s such that Modifies(s,"x")

                    var queryPreprocessor = new QueryPreprocessor();
                    try
                    {
                        string query = declarations.ToString() + " " + command.ToString();

                        QueryTree queryTree = queryPreprocessor.ParseQuery(query);
                        QueryEvaluator queryEvaluator = new QueryEvaluator(pkb);

                        string result = queryEvaluator.GetQueryResultsRawPipeTester(queryTree);

                        Console.WriteLine(result);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"#{string.Join("\n", queryPreprocessor.GetValidationErrors())}");
                    }                    
                }
            }
        }
    }
}