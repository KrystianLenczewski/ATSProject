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
            /* Proszę mi nie ruszać obsługi Pipetester'a jeśli się nie wie co to */
            /* Jak potrzebujecie konsolki do testowania to zróbcie sobie podprojekt do tego! */
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding(852);

            if (args.Length == 1)
            {
                string text = File.ReadAllText(args[0], Encoding.GetEncoding(852));

                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IPKBStore, PKBStore>()
                    .BuildServiceProvider();

                var pkb = serviceProvider.GetService<IPKBStore>();
                pkb.ParseCode(text);
                pkb.Extract(pkb.ModifiesList);

                while (true)
                {
                    var declarations = Console.ReadLine(); // example: stmt s;
                    var command = Console.ReadLine(); // example: Select s such that Modifies(s,"x")

                    try
                    {
                        var queryPreprocessor = new QueryPreprocessor();
                        string query = declarations.ToString() + " " + command.ToString();

                        QueryTree queryTree = queryPreprocessor.ParseQuery(query);
                        QueryEvaluator queryEvaluator = new QueryEvaluator(PKBInitializer.InitializePKB());

                        var result = queryEvaluator.GetQueryResultsRaw(queryTree);

                        Console.WriteLine(result);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("#" + ex.Message);
                    }                    
                }
            }
        }
    }
}