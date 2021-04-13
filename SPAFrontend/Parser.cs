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
        public static void ParseCode(this IPKBStore pkb, string code)
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
                        var rownum = CreateObjectForPath(currentPath + ".assignment.rownum", (line.Split(' ')[0].Trim('.')));
                        final.Merge(rownum);
                        var variable = CreateObjectForPath(currentPath + ".assignment.variable", (line.Split(' ')[1].Split('=')[0].Trim()));
                        final.Merge(variable);
                        ParseAssignment(line.Split(' ')[0] + " " + line.Remove(0, line.Split('.')[0].Length + 2).Replace(";", "").Replace(" ", ""), final, currentPath + ".assignment.value");
                    }
                    else
                    {
                        var rownum = CreateObjectForPath(currentPath + ".assignment.rownum", (line.Split(' ')[0].Trim('.')));
                        final.Merge(rownum);
                        var variable = CreateObjectForPath(currentPath + ".assignment.variable", (line.Split(' ')[1].Split('=')[0].Trim()));
                        final.Merge(variable);
                        ParseAssignment(line.Split(' ')[0] + " " + line.Remove(0, line.Split('.')[0].Length + 2).Replace("}", "").Replace(" ", "").Replace(";", ""), final, currentPath + ".assignment.value");

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
        }

        private static string ParseAssignment(string assignment, JObject jObj, string path)
        {
            assignment += ';';
            string parsedAssignment = "";
            string left = assignment.Split(' ')[1].Split('=')[0];
            string right = assignment.Split(' ')[1].Split('=')[1];
            bool readToOperation = false;
            string buffor = "";
            char operation = '\0';

            for (int i = 0; i < right.Length; i++)
            {
                if (!readToOperation && (Char.IsDigit(right[i]) || Char.IsLetter(right[i])))
                {
                    readToOperation = true;
                }

                if (readToOperation)
                {
                    switch (right[i])
                    {
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                            readToOperation = false;
                            operation = right[i];
                            break;
                        case ';':
                            readToOperation = false;
                            operation = right[i];

                            if (right.IndexOfAny("+-*/".ToCharArray()) == -1)
                            {
                                var child1 = CreateObjectForPath(path + ".child1", buffor);
                                jObj.Merge(child1);
                            } else
                            {
                                var child2 = CreateObjectForPath(path + ".child2", buffor);
                                jObj.Merge(child2);
                            }

                            buffor = "";
                            break;
                        default:
                            if (operation.Equals('\0')) buffor += right[i].ToString();
                            else if (path.Split('.')[path.Split('.').Length - 1].Equals("add"))
                            {
                                buffor += operation.ToString() + right[i].ToString();
                                path += ".child2.add";

                                var child1 = CreateObjectForPath(path + ".child1", buffor.Split(operation)[0]);
                                jObj.Merge(child1);
                                var child2 = CreateObjectForPath(path + ".child2", buffor.Split(operation)[1]);
                                jObj.Merge(child2);

                                buffor = buffor.Split(operation)[1];
                            }
                            else
                            {
                                buffor += operation.ToString() + right[i].ToString();
                                path += ".add";

                                var child1 = CreateObjectForPath(path + ".child1", buffor.Split(operation)[0]);
                                jObj.Merge(child1);
                                var child2 = CreateObjectForPath(path + ".child2", buffor.Split(operation)[1]);
                                jObj.Merge(child2);

                                buffor = buffor.Split(operation)[1];
                            }
                            operation = '\0';
                            break;
                    }
                }

            }

            return parsedAssignment;
        }
    }
}
