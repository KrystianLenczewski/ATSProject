using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Shared;
using SPAFrontend;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(852);

            if (args.Length == 1)
            {
                string text = File.ReadAllText(args[0], Encoding.GetEncoding(852));
                List<Statement> statements = Parser.ParseCode(text); // TODO1: Implementacja ParseCode
                List<Statement> extractedStatements = DesignExtractor.ParseStataments(statements); // TODO2: Implementacja ParseStataments lub zmiana interfejsów
                // var pkb = TODO3: wysłanie do parsera - return: wypełnione PKB, argument wejściowy (List<Statement> extractedStatements)
                Console.WriteLine("Ready");
                while (true)
                {
                    var declarations = Console.ReadLine(); // example: stmt s;
                    var command = Console.ReadLine(); // example: Select s such that Modifies(s,"x")
                    // var result = TODO4: wyszukiwarka PQL, return: lista odpowiedzi (List<string>) lub string z odpowiedziami, argumenty wejściowe (this PKB pkb, string declarations, string command)
                    Console.WriteLine(/*result*/); // tutaj do wypisania result TODO5
                }
            }
        }
    }
}
/*
 * parser, PBK czy PQL można jako biblioteki .NET Standard zrobić
 */
