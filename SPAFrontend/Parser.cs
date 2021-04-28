﻿using System;
using System.Collections.Generic;
using Shared;
using PKB;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using SPAFrontend.mdoels;

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
                List<DepthRownum> depthRownumPair = new List<DepthRownum>();

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
                        string rownum = line.Split(' ')[0].Trim('.');

                        string[] stmtTypePath = currentPath.Substring(0, currentPath.LastIndexOf('.')).Split('.');
                        if (stmtTypePath[stmtTypePath.Length - 1].Contains("if") ||
                            stmtTypePath[stmtTypePath.Length - 1].Contains("else"))
                        {
                            DepthRownum drp = depthRownumPair.Find(p => p.depth == currentPath.Split('.').Length);
                            if (drp != null)
                            {
                                PKBParserServices.SetParent(pkb,
                                    new ExpressionModel(SpecialType.STMTLST, drp.rownum),
                                    new ExpressionModel(StatementType.WHILE, Int32.Parse(rownum)),
                                    0);
                            }
                        }
                        else if (stmtTypePath[stmtTypePath.Length - 1].Contains("while") &&
                            Int32.TryParse(stmtTypePath[stmtTypePath.Length - 1].Split('-')[1], out int rn))
                        {
                            PKBParserServices.SetParent(pkb,
                                new ExpressionModel(StatementType.WHILE, rn),
                                new ExpressionModel(StatementType.WHILE, Int32.Parse(rownum)),
                                0);
                        }

                        currentPath += $".while-{rownum}";
                        var obj = CreateObjectForPath(currentPath + ".param", (line.Split(' ')[2]));
                        final.Merge(obj);
                        var obj2 = CreateObjectForPath(currentPath + ".rownum", (rownum));
                        final.Merge(obj2);
                        currentPath += ".stmtList";
                    }
                    else if (line.Contains("call"))
                    {
                        string rownum = line.Split(' ')[0].Trim('.');
                        currentPath += $".call-{rownum}";
                        var obj = CreateObjectForPath(currentPath + ".param", (line.Split(' ')[1]));
                        final.Merge(obj);
                        var obj2 = CreateObjectForPath(currentPath + ".rownum", (rownum));
                        final.Merge(obj2);
                        currentPath = currentPath.Remove(currentPath.LastIndexOf('.'));
                    }
                    else if (line.Contains("if"))
                    {
                        string rownum = line.Split(' ')[0].Trim('.');

                        string[] stmtTypePath = currentPath.Substring(0, currentPath.LastIndexOf('.')).Split('.');
                        if (stmtTypePath[stmtTypePath.Length - 1].Contains("if") ||
                            stmtTypePath[stmtTypePath.Length - 1].Contains("else"))
                        {
                            DepthRownum drp = depthRownumPair.Find(p => p.depth == currentPath.Split('.').Length);
                            if (drp != null)
                            {
                                PKBParserServices.SetParent(pkb,
                                    new ExpressionModel(SpecialType.STMTLST, drp.rownum),
                                    new ExpressionModel(StatementType.WHILE, Int32.Parse(rownum)),
                                    0);
                            }
                        }
                        else if (stmtTypePath[stmtTypePath.Length - 1].Contains("while") &&
                            Int32.TryParse(stmtTypePath[stmtTypePath.Length - 1].Split('-')[1], out int rn))
                        {
                            PKBParserServices.SetParent(pkb,
                                new ExpressionModel(StatementType.WHILE, rn),
                                new ExpressionModel(StatementType.WHILE, Int32.Parse(rownum)),
                                0);
                        }

                        currentPath += $".if-{rownum}";
                        var obj = CreateObjectForPath(currentPath + ".param", (line.Split(' ')[2]));
                        final.Merge(obj);
                        var obj2 = CreateObjectForPath(currentPath + ".rownum", (rownum));
                        final.Merge(obj2);
                        currentPath += ".stmtList";

                        depthRownumPair.Add(new DepthRownum(currentPath.Split('.').Length, Int32.Parse(rownum)));
                    }
                    else if (line.Contains("else"))
                    {
                        currentPath += ".else.stmtList";
                    }
                    else if (line.Contains(";") && !line.Contains("}"))
                    {
                        string rownumNumber = line.Split(' ')[0].Trim('.');
                        var rownum = CreateObjectForPath(currentPath + $".assignment-{rownumNumber}.rownum", (rownumNumber));
                        final.Merge(rownum);
                        var variable = CreateObjectForPath(currentPath + $".assignment-{rownumNumber}.variable", (line.Split(' ')[1].Split('=')[0].Trim()));
                        final.Merge(variable);

                        string[] stmtTypePath = currentPath.Substring(0, currentPath.LastIndexOf('.')).Split('.');
                        var rownumForPKB = final.SelectToken(currentPath.Substring(0, currentPath.LastIndexOf('.')) + ".rownum");
                        if (stmtTypePath[stmtTypePath.Length - 1].Trim().Contains("while") &&
                            rownumForPKB != null &&
                            Int32.TryParse(rownumForPKB.ToString(), out int rnFPKB) &&
                            Int32.TryParse(rownumNumber.ToString(), out int rn))
                        {
                            PKBParserServices.SetModify(pkb,
                                new ExpressionModel(StatementType.WHILE, rnFPKB),
                                new ExpressionModel(FactorType.VAR, line.Split(' ')[1].Split('=')[0].Trim(), rn));

                            PKBParserServices.SetParent(pkb,
                                new ExpressionModel(StatementType.WHILE, rnFPKB),
                                new ExpressionModel(StatementType.ASSIGN, rn),
                                0);
                        }
                        else if (stmtTypePath[stmtTypePath.Length - 1].Trim().Contains("if") &&
                            rownumForPKB != null &&
                            Int32.TryParse(rownumForPKB.ToString(), out int rnFPKB2) &&
                            Int32.TryParse(rownumNumber.ToString(), out int rn2))
                        {
                            PKBParserServices.SetParent(pkb,
                                new ExpressionModel(SpecialType.STMTLST, rnFPKB2),
                                new ExpressionModel(StatementType.ASSIGN, rn2),
                                0);
                        }
                        else if (stmtTypePath[stmtTypePath.Length - 1].Trim().Contains("else") &&
                            stmtTypePath[stmtTypePath.Length - 2].Trim().Contains("stmtList") &&
                            Int32.TryParse(rownumNumber.ToString(), out int rn3))
                        {
                            DepthRownum drp = depthRownumPair.Find(p => p.depth == currentPath.Split('.').Length);
                            if (drp != null)
                            {
                                PKBParserServices.SetParent(pkb,
                                    new ExpressionModel(SpecialType.STMTLST, drp.rownum),
                                    new ExpressionModel(StatementType.ASSIGN, rn3),
                                    0);
                            }
                        }

                        ParseAssignment(pkb,
                            line.Split(' ')[0] + " " + line.Remove(0, line.Split('.')[0].Length + 2).Replace(";", "").Replace(" ", ""),
                            final,
                            currentPath + $".assignment-{rownumNumber}.value");
                    }
                    else
                    {
                        string rownumNumber = line.Split(' ')[0].Trim('.');
                        var rownum = CreateObjectForPath(currentPath + $".assignment-{rownumNumber}.rownum", (rownumNumber));
                        final.Merge(rownum);
                        var variable = CreateObjectForPath(currentPath + $".assignment-{rownumNumber}.variable", (line.Split(' ')[1].Split('=')[0].Trim()));
                        final.Merge(variable);

                        string[] stmtTypePath = currentPath.Substring(0, currentPath.LastIndexOf('.')).Split('.');
                        var rownumForPKB = final.SelectToken(currentPath.Substring(0, currentPath.LastIndexOf('.')) + ".rownum");
                        if (stmtTypePath[stmtTypePath.Length - 1].Trim().Contains("while") &&
                            rownumForPKB != null &&
                            Int32.TryParse(rownumForPKB.ToString(), out int rnFPKB) &&
                            Int32.TryParse(rownumNumber.ToString(), out int rn))
                        {
                            PKBParserServices.SetModify(pkb,
                                new ExpressionModel(StatementType.WHILE, rnFPKB),
                                new ExpressionModel(FactorType.VAR, line.Split(' ')[1].Split('=')[0].Trim(), rn));

                            PKBParserServices.SetParent(pkb,
                                new ExpressionModel(StatementType.WHILE, rnFPKB),
                                new ExpressionModel(StatementType.ASSIGN, rn),
                                0);
                        }
                        else if (stmtTypePath[stmtTypePath.Length - 1].Trim().Contains("if") &&
                           rownumForPKB != null &&
                           Int32.TryParse(rownumForPKB.ToString(), out int rnFPKB2) &&
                            Int32.TryParse(rownumNumber.ToString(), out int rn2))
                        {
                            PKBParserServices.SetParent(pkb,
                                new ExpressionModel(SpecialType.STMTLST, rnFPKB2),
                                new ExpressionModel(StatementType.ASSIGN, rn2),
                                0);
                        }
                        else if (stmtTypePath[stmtTypePath.Length - 1].Trim().Contains("else") &&
                            stmtTypePath[stmtTypePath.Length - 2].Trim().Contains("stmtList"))
                        {
                            DepthRownum drp = depthRownumPair.Find(p => p.depth == currentPath.Split('.').Length);
                            if (drp != null &&
                            Int32.TryParse(rownumNumber, out int rn3))
                            {
                                PKBParserServices.SetParent(pkb,
                                    new ExpressionModel(SpecialType.STMTLST, drp.rownum),
                                    new ExpressionModel(StatementType.ASSIGN, rn3),
                                    0);
                            }
                        }

                        ParseAssignment(pkb, line.Split(' ')[0] + " " + line.Remove(0, line.Split('.')[0].Length + 2).Replace("}", "").Replace(" ", "").Replace(";", ""), final, currentPath + $".assignment-{rownumNumber}.value");

                        for (int i = 0; i < line.Length - line.Replace("}", "").Length; i++)
                        {
                            if (stmtTypePath[stmtTypePath.Length - 1].Trim().Contains("else"))
                                depthRownumPair.RemoveAll(p => p.depth == currentPath.Split('.').Length);
                            currentPath = currentPath.Remove(currentPath.LastIndexOf('.'));
                            currentPath = currentPath.Remove(currentPath.LastIndexOf('.'));
                        }
                    }
                }
                Console.WriteLine(final);
            }
            PKBParserServices.RebuildParentListIndexes(pkb);
            //Console.Write(code);
            Console.WriteLine(pkb.ToString());
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

        private static string ParseAssignment(this IPKBStore pkb, string assignment, JObject jObj, string path)
        {
            assignment += ';';
            string parsedAssignment = "";

            string rownum = assignment.Split(' ')[0].Trim('.');
            string variable = assignment.Split(' ')[1].Split('=')[0];
            PKBParserServices.SetModify(pkb, new ExpressionModel(StatementType.ASSIGN, Int32.Parse(rownum)), new ExpressionModel(FactorType.VAR, variable, Int32.Parse(rownum)));

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
                            }
                            else
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