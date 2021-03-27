using System;
using System.Collections.Generic;
using Shared;
using PKB;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SPAFrontend
{
    public static class Parser
    {
        public static void ParseCode(this PKBStore pkb, string code)
        {
            JObject final = new JObject();

            using (StringReader reader = new StringReader(code))
            {
                string currentPath = "$.";
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("procedure"))
                    {
                        final.Add("procedure", "");
                        currentPath += "procedure.";
                        final[currentPath + "name"] = (line.Split(' ')[1]);
                    }
                    else if (line.Contains("while"))
                    {

                    } 
                    else if (line.Contains("if"))
                    {

                    }
                    else if (line.Contains("else"))
                    {

                    }
                    else if (line.Contains(";") && !line.Contains("}"))
                    {

                    }
                    else if (line.Contains(";"))
                    {

                    }
                    else
                    {
                        Console.WriteLine("ch00y-nya");
                    }
                }
            }

            Console.Write(code);
        }
    }

    public class StatementInstance
    {
        public Statement type { get; set; }
        public List<string> args { get; set; }
        public List<StatementInstance> children { get; set; }

        StatementInstance() { }
    }
}
