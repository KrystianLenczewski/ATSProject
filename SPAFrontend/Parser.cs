using System;
using System.Collections.Generic;
using Shared;
using PKB;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace SPAFrontend
{
    public static class Parser
    {
        public static void ParseCode(this PKBStore pkb, string code)
        {
            JObject final = new JObject();

            using (StringReader reader = new StringReader(code))
            {
                string currentPath = "";
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("procedure"))
                    {
                        final.Add("procedure", "");
                        currentPath = "$.procedure";
                        var obj = CreateObjectForPath(currentPath + ".name", (line.Split(' ')[1]));
                        final.Merge(obj);
                        currentPath += ".stmtList";
                    }
                    else if (line.Contains("while"))
                    {
                        currentPath += ".while";
                        var obj = CreateObjectForPath(currentPath + ".param", (line.Split(' ')[2]));
                        final.Merge(obj);
                        var obj2 = CreateObjectForPath(currentPath + ".rownum", (line.Split(' ')[0].Trim('.')));
                        final.Merge(obj2);
                        currentPath += ".stmtList";
                    } 
                    else if (line.Contains("if"))
                    {
                        currentPath += ".if";
                        var obj = CreateObjectForPath(currentPath + ".param", (line.Split(' ')[2]));
                        final.Merge(obj);
                        var obj2 = CreateObjectForPath(currentPath + ".rownum", (line.Split(' ')[0].Trim('.')));
                        final.Merge(obj2);
                        currentPath += ".stmtList";
                    }
                    else if (line.Contains("else"))
                    {
                        currentPath += ".else.stmtList";
                    }
                    else if (line.Contains(";") && !line.Contains("}"))
                    {
                        var obj = CreateObjectForPath(currentPath + ".assignment", (line.Split(' ')[0] + " " + line.Split(' ')[1].Trim(';')));
                        final.Merge(obj);
                    }
                    else
                    {
                        var obj = CreateObjectForPath(currentPath + ".assignment", (line.Split(' ')[0] + " " + line.Split(' ')[1].Trim(';')));
                        final.Merge(obj);

                        for (int i = 0; i < line.Length - line.Replace("}", "").Length; i++)
                        {
                            currentPath = currentPath.Remove(currentPath.LastIndexOf('.'));
                            currentPath = currentPath.Remove(currentPath.LastIndexOf('.'));
                        }
                    }
                }
                Console.WriteLine(final);
            }

            Console.Write(code);
        }

        private static JObject CreateObjectForPath(string target, object newValue)
        {
            var json = new StringBuilder();

            json.Append(@"{");

            var paths = target.Split('.');

            var i = -1;
            var objCount = 0;

            foreach (string path in paths)
            {
                i++;

                if (paths[i] == "$") continue;

                json.Append('"');
                json.Append(path);
                json.Append('"');
                json.Append(": ");

                if (i + 1 != paths.Length)
                {
                    json.Append("{");
                    objCount++;
                }
            }

            json.Append("\"" + newValue + "\"");

            for (int level = 1; level <= objCount; level++)
            {
                json.Append(@"}");
            }

            json.Append(@"}");
            var jsonString = json.ToString();
            var obj = JObject.Parse(jsonString);
            return obj;

//             JObject ass = new JObject();

//             Console.WriteLine(code);

//             StringReader strReader = new StringReader(code);
//             string line = strReader.ReadLine();
//             string[] lineWords = line.Trim(';').Split('=');

//             string[] right = lineWords[1].Trim().Split(' ');
//             for (int i = 0; i < right.Length; i++)
//             {
//                 if(right.Length == 1)
//                 {
//                     //  add JObject lineWords[0] --- assig --- right[i]
//                 } else
//                 {
//                     switch(right[i+1])
//                     {
//                         case "+":
//                             //  add JObject right[i] --- right[i+1] --- check_if_var_or_+
//                             i++;
//                             break;
//                         default:
//                             //  add JObject right[i] --- 
//                             break;
//                     }
//                 }
//             }

//             throw new NotImplementedException();
        }
    }
}
