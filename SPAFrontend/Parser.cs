using System;
using System.Collections.Generic;
using Shared;
using PKB;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using SPAFrontend.mdoels;
using System.Linq;

namespace SPAFrontend
{
    public static class Parser
    {
        public static void ParseCode(this IPKBStore pkb, string code)
        {
            JObject final = new JObject();
            List<ExpressionModel> ungroupedChildren = new List<ExpressionModel>();
            HashSet<KeyValuePair<ExpressionModel, ExpressionModel>> callsHolder = new HashSet<KeyValuePair<ExpressionModel, ExpressionModel>>(new ExpressionModelKeyPairComparer());
            HashSet<string> varHolder = new HashSet<string>();
            HashSet<string> constHolder = new HashSet<string>();

            using (StringReader reader = new StringReader(code))
            {
                string currentPath = "";
                string line = "";
                int counter = 1;
                int procedureCounter = 0;
                string lineWithNumber = string.Empty;
                List<DepthRownum> depthRownumPair = new List<DepthRownum>();
                string parentProcedureHolder = string.Empty;

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)) { }
                    else if (line.Contains("procedure"))
                    {
                        procedureCounter++;
                        var tmpPath = $"procedure-{procedureCounter}";
                        final.Add(tmpPath, "");
                        currentPath = $"$.{tmpPath}";
                        var obj = CreateObjectForPath(currentPath + ".name", (line.Split(' ')[1]));
                        final.Merge(obj);
                        currentPath += ".stmtList";

                        parentProcedureHolder = line.Split(' ')[1];
                        PKBParserServices.SetProcList(pkb, line.Split(' ')[1]);
                    }
                    else if (line.Contains("while"))
                    {
                        string rownum = counter.ToString();
                        counter++;

                        PKBParserServices.SetUses(pkb,
                            new ExpressionModel(StatementType.WHILE, Int32.Parse(rownum)),
                            new ExpressionModel(FactorType.VAR, line.Split(' ')[2]));

                        PKBParserServices.SetAllStatements(pkb, new ExpressionModel(StatementType.WHILE, Int32.Parse(rownum)));

                        varHolder.Add(line.Split(' ')[1]);

                        if (currentPath.Equals($"$.procedure-{procedureCounter}.stmtList"))
                        {
                            ungroupedChildren.Add(new ExpressionModel(StatementType.WHILE, Int32.Parse(rownum)));
                        }

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
                        var obj = CreateObjectForPath(currentPath + ".param", (line.Split(' ')[1]));
                        final.Merge(obj);
                        var obj2 = CreateObjectForPath(currentPath + ".rownum", (rownum));
                        final.Merge(obj2);
                        currentPath += ".stmtList";
                    }
                    else if (line.Contains("call"))
                    {
                        string rownum = counter.ToString();
                        counter++;

                        if (currentPath.Equals($"$.procedure-{procedureCounter}.stmtList"))
                        {
                            ungroupedChildren.Add(new ExpressionModel(StatementType.CALL, Int32.Parse(rownum)));
                        }

                        PKBParserServices.SetAllStatements(pkb, new ExpressionModel(StatementType.CALL, Int32.Parse(rownum)));

                        string[] stmtTypePath = currentPath.Substring(0, currentPath.LastIndexOf('.')).Split('.');
                        if (stmtTypePath[stmtTypePath.Length - 1].Contains("if") ||
                            stmtTypePath[stmtTypePath.Length - 1].Contains("else"))
                        {
                            DepthRownum drp = depthRownumPair.Find(p => p.depth == currentPath.Split('.').Length);
                            if (drp != null)
                            {
                                PKBParserServices.SetParent(pkb,
                                    new ExpressionModel(SpecialType.STMTLST, drp.rownum),
                                    new ExpressionModel(StatementType.CALL, Int32.Parse(rownum)),
                                    0);
                            }
                        }
                        else if (stmtTypePath[stmtTypePath.Length - 1].Contains("while") &&
                            Int32.TryParse(stmtTypePath[stmtTypePath.Length - 1].Split('-')[1], out int rn))
                        {
                            PKBParserServices.SetParent(pkb,
                                new ExpressionModel(StatementType.WHILE, rn),
                                new ExpressionModel(StatementType.CALL, Int32.Parse(rownum)),
                                0);
                        }

                        callsHolder.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, parentProcedureHolder), new ExpressionModel(WithNameType.PROCEDURE, line.Split(' ')[1].Replace(";", string.Empty))));

                        currentPath += $".call-{rownum}";
                        var obj = CreateObjectForPath(currentPath + ".param", line.Split(' ')[1].Replace(";", string.Empty));
                        final.Merge(obj);
                        var obj2 = CreateObjectForPath(currentPath + ".rownum", (rownum));
                        final.Merge(obj2);
                        currentPath = currentPath.Remove(currentPath.LastIndexOf('.'));
                    }
                    else if (line.Contains("if"))
                    {
                        string rownum = counter.ToString();
                        counter++;

                        if (currentPath.Equals($"$.procedure-{procedureCounter}.stmtList"))
                        {
                            ungroupedChildren.Add(new ExpressionModel(StatementType.IF, Int32.Parse(rownum)));
                        }

                        PKBParserServices.SetAllStatements(pkb, new ExpressionModel(StatementType.IF, Int32.Parse(rownum)));

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

                        PKBParserServices.SetUses(pkb,
                            new ExpressionModel(StatementType.IF, Int32.Parse(rownum)),
                            new ExpressionModel(FactorType.VAR, line.Split(' ')[2]));

                        varHolder.Add(line.Split(' ')[1]);

                        currentPath += $".if-{rownum}";
                        var obj = CreateObjectForPath(currentPath + ".param", (line.Split(' ')[1]));
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
                        string rownumNumber = counter.ToString();
                        counter++;

                        lineWithNumber = rownumNumber.ToString() + ". " + line;

                        if (currentPath.Equals($"$.procedure-{procedureCounter}.stmtList"))
                        {
                            ungroupedChildren.Add(new ExpressionModel(StatementType.ASSIGN, Int32.Parse(rownumNumber)));
                        }

                        PKBParserServices.SetAllStatements(pkb, new ExpressionModel(StatementType.ASSIGN, Int32.Parse(rownumNumber)));

                        var rownum = CreateObjectForPath(currentPath + $".assignment-{rownumNumber}.rownum", (rownumNumber));
                        final.Merge(rownum);
                        var variable = CreateObjectForPath(currentPath + $".assignment-{rownumNumber}.variable", (lineWithNumber.Split(' ')[1].Split('=')[0].Trim()));
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
                                new ExpressionModel(FactorType.VAR, line.Split(' ')[1].Split('=')[0].Trim()));

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

                        var tempAssignment = ParseAssignment(pkb,
                            lineWithNumber.Split(' ')[0] + " " + lineWithNumber.Remove(0, lineWithNumber.Split('.')[0].Length + 2).Replace(";", "").Replace(" ", ""),
                            final,
                            currentPath + $".assignment-{rownumNumber}.value");

                        varHolder.UnionWith(tempAssignment.Item2);
                    }
                    else
                    {
                        string rownumNumber = counter.ToString();
                        counter++;

                        if (currentPath.Equals($"$.procedure-{procedureCounter}.stmtList"))
                        {
                            ungroupedChildren.Add(new ExpressionModel(StatementType.ASSIGN, Int32.Parse(rownumNumber)));
                        }

                        PKBParserServices.SetAllStatements(pkb, new ExpressionModel(StatementType.ASSIGN, Int32.Parse(rownumNumber)));

                        lineWithNumber = rownumNumber + ". " + line;
                        var rownum = CreateObjectForPath(currentPath + $".assignment-{rownumNumber}.rownum", (rownumNumber));
                        final.Merge(rownum);
                        var variable = CreateObjectForPath(currentPath + $".assignment-{rownumNumber}.variable", (lineWithNumber.Split(' ')[1].Split('=')[0].Trim()));
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
                                new ExpressionModel(FactorType.VAR, line.Split(' ')[1].Split('=')[0].Trim()));

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

                        var tempAssignment = ParseAssignment(pkb, lineWithNumber.Split(' ')[0] + " " + lineWithNumber.Remove(0, lineWithNumber.Split('.')[0].Length + 2).Replace("}", "").Replace(" ", "").Replace(";", ""), final, currentPath + $".assignment-{rownumNumber}.value");

                        varHolder.UnionWith(tempAssignment.Item2);

                        for (int i = 0; i < line.Length - line.Replace("}", "").Length; i++)
                        {
                            if (stmtTypePath[stmtTypePath.Length - 1].Trim().Contains("else"))
                                depthRownumPair.RemoveAll(p => p.depth == currentPath.Split('.').Length);
                            currentPath = currentPath.Remove(currentPath.LastIndexOf('.'));
                            currentPath = currentPath.Remove(currentPath.LastIndexOf('.'));
                        }
                    }

                    var parsedLine = line.Split(' ');
                    foreach(var parsedLineItem in parsedLine)
                    {
                        int num;
                        if(Int32.TryParse(parsedLineItem.Replace(";", string.Empty), out num))
                        {
                            constHolder.Add(num.ToString());
                        }
                    }
                }
                Console.WriteLine(final);
            }

            PKBParserServices.SetCallsRange(pkb, callsHolder);
            PKBParserServices.SetVarList(pkb, varHolder);
            PKBParserServices.SetConstRange(pkb, constHolder);
            PKBParserServices.RebuildParentListIndexes(pkb);

            //setUses (get upper parents of processed child)
            var tmpUsesList = pkb.UsesList.GetRange(0, pkb.UsesList.Count);
            foreach (var item in tmpUsesList)
            {
                ExpressionModel tmpModel;
                if (item.Value.Name.Equals(string.Empty))
                {
                    tmpModel = new ExpressionModel((StatementType)item.Value.Type, item.Value.Line);
                }
                else
                {
                    tmpModel = new ExpressionModel((FactorType)item.Value.Type, item.Value.Name);
                }

                var parentsList = FindAllParentsRecursively(pkb, item.Key).ToList();
                foreach (var parent in parentsList)
                {
                    pkb.SetUses(new ExpressionModel((StatementType)parent.Type, parent.Line), tmpModel);
                }
            }
            tmpUsesList = pkb.UsesList.GetRange(0, pkb.UsesList.Count);
            foreach (var item in tmpUsesList)
            {
                if (item.Key.Type.Equals(ExpressionType.STMTLST))
                {
                    pkb.UsesList.Remove(item);
                }
            }

            //setFollows (group by parent, then get previous children of each child)
            var tmpParents = pkb.ParentList.GetRange(0, pkb.ParentList.Count);
            var tmpChildren = tmpParents.Select(x => x.Child);
            var groupedParents = tmpParents.GroupBy(x => x.Parent.Line).Select(g => g.FirstOrDefault().Parent).ToList();
            var children = new List<ExpressionModel>();

            foreach (var x in groupedParents)
            {
                children = new List<ExpressionModel>();
                foreach (var y in tmpParents)
                {
                    if (y.Parent.Line == x.Line)
                    {
                        children.Add(y.Child);
                    }
                }

                if (children.Count > 1)
                    for (int i = 0; i < children.Count - 1; i++)
                    {
                        PKBParserServices.SetFollows(pkb,
                                children[i],
                                children[i + 1]);
                    }
            }

            //need to remember to handle ungrouped childen (their parent is default scope)
            if (ungroupedChildren.Count > 1)
                for (int i = 0; i < ungroupedChildren.Count - 1; i++)
                {
                    PKBParserServices.SetFollows(pkb,
                            ungroupedChildren[i],
                            ungroupedChildren[i + 1]);
                }

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

        private static (string, HashSet<string>) ParseAssignment(this IPKBStore pkb, string assignment, JObject jObj, string path)
        {
            HashSet<string> varList = new HashSet<string>();
            assignment += ';';
            string parsedAssignment = "";

            string rownum = assignment.Split(' ')[0].Trim('.');
            string variable = assignment.Split(' ')[1].Split('=')[0];
            PKBParserServices.SetModify(pkb, new ExpressionModel(StatementType.ASSIGN, Int32.Parse(rownum)), new ExpressionModel(FactorType.VAR, variable));

            varList.Add(variable);

            string right = assignment.Split(' ')[1].Split('=')[1];
            bool readToOperation = false;
            string buffor = "";
            char operation = '\0';

            foreach (char x in right)
            {
                if (char.IsLetter(x))
                {
                    PKBParserServices.SetUses(pkb,
                        new ExpressionModel(StatementType.ASSIGN, Int32.Parse(rownum)),
                        new ExpressionModel(FactorType.VAR, x.ToString()));

                    varList.Add(x.ToString());
                }
            }

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

            return (parsedAssignment, varList);
        }

        private static HashSet<ExpressionModel> FindAllParentsRecursively(this IPKBStore pkb, ExpressionModel child)
        {
            HashSet<ExpressionModel> res = new HashSet<ExpressionModel>();

            foreach (var item in pkb.ParentList)
            {
                if (item.Child.Equals(child))
                {
                    res.Add(item.Parent);
                    res.UnionWith(FindAllParentsRecursively(pkb, item.Parent));
                }
            }

            return res;
        }
    }

    public class ExpressionModelKeyPairComparer : IEqualityComparer<KeyValuePair<ExpressionModel, ExpressionModel>>
    {
        public bool Equals(KeyValuePair<ExpressionModel, ExpressionModel> x, KeyValuePair<ExpressionModel, ExpressionModel> y)
        {
            if (x.Key == null || x.Value == null || y.Key == null || y.Value == null)
                return false;

            return x.Key.Name.Equals(y.Key.Name) && x.Key.Line.Equals(y.Key.Line) && x.Key.Type.Equals(y.Key.Type)
                && x.Value.Name.Equals(y.Value.Name) && x.Value.Line.Equals(y.Value.Line) && x.Value.Type.Equals(y.Value.Type);
        }

        public int GetHashCode(KeyValuePair<ExpressionModel, ExpressionModel> obj)
        {
            return (obj.Key.Name + obj.Key.Line + obj.Key.Type + obj.Value.Name + obj.Value.Line + obj.Value.Type).GetHashCode();
        }
    }
}