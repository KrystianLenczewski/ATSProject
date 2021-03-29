using System;
using System.IO;
using System.Text;
using PKB;
using QueryProcessor;
using SPAFrontend;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
   
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding(852);
            QueryPreprocessor queryPreprocessor = new QueryPreprocessor();
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            QueryResultProjector queryResultProjector = new QueryResultProjector();


            if (args.Length == 1)
            {
                string text = File.ReadAllText(args[0], Encoding.GetEncoding(852));
                var pkb = PKBStore.Instance;
               pkb.ParseCode(text);
          
                // Design Extractor
                Console.WriteLine("Ready");
                while (true)
                {
                    var declarations = Console.ReadLine(); // example: stmt s;
                    var command = File.ReadAllText("C:/Users/Krystian/Desktop/Plik/Query/Query.txt", Encoding.GetEncoding(852));// example: Select s such that Modifies(s,"x")

                    QueryTree result = queryPreprocessor.ParseQuery(command); //TODO4: wyszukiwarka PQL, return: lista odpowiedzi (List<string>) lub string z odpowiedziami, argumenty wejściowe (this PKB pkb, string declarations, string command)
                    QueryResultRaw queryResultRaw=queryEvaluator.EvaluateQuery(result);
                    queryResultProjector.FormatQueryResults(queryResultRaw);

                    Console.WriteLine(/*result*/); // tutaj do wypisania result TODO5
                }
            }
        }
    }
}
/*
 * parser, PBK czy PQL można jako biblioteki .NET Standard zrobić
 */
