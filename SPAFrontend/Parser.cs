using System;
using System.IO;
using Newtonsoft.Json.Linq;
using PKB;

namespace SPAFrontend
{
    public static class Parser
    {
        public static void ParseCode(this PKBStore pkb, string code)
        {
            JObject ass = new JObject();

            Console.WriteLine(code);

            StringReader strReader = new StringReader(code);
            string line = strReader.ReadLine();
            string[] lineWords = line.Trim(';').Split('=');

            string[] right = lineWords[1].Trim().Split(' ');
            for (int i = 0; i < right.Length; i++)
            {
                if(right.Length == 1)
                {
                    //  add JObject lineWords[0] --- assig --- right[i]
                } else
                {
                    switch(right[i+1])
                    {
                        case "+":
                            //  add JObject right[i] --- right[i+1] --- check_if_var_or_+
                            i++;
                            break;
                        default:
                            //  add JObject right[i] --- 
                            break;
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
