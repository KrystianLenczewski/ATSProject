using System;
using System.IO;
using System.Text;
using PKB;
using SPAFrontend;

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
                string text = File.ReadAllText("D:\\Kacper\\Sztudien\\II-stopień\\ATS\\Source.txt", Encoding.GetEncoding(852));
                var pkb = PKBStore.Instance;
                pkb.ParseCode(text);
                /*pkb.ParseStataments(pkb.ModifiesList);
                // Design Extractor
                Console.WriteLine("Ready");
                while (true)
                {
                    var declarations = Console.ReadLine(); // example: stmt s;
                    var command = Console.ReadLine(); // example: Select s such that Modifies(s,"x")
                    // var result = TODO4: wyszukiwarka PQL, return: lista odpowiedzi (List<string>) lub string z odpowiedziami, argumenty wejściowe (this PKB pkb, string declarations, string command)
                    Console.WriteLine("result"); // tutaj do wypisania result TODO5
                }*/
                Console.ReadKey();
            }
        }
    }
}
/*
 * parser, PBK czy PQL można jako biblioteki .NET Standard zrobić
 */
