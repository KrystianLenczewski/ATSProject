﻿using Shared;
using Shared.PQLModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKB
{
    public static class PKBPQLServices
    {
        private static bool WhereConditionIsTrue(int line = 0, int rowLine = 0, ExpressionType type = ExpressionType.NULL, ExpressionType rowType = ExpressionType.NULL)
            => ((line == 0) || (rowLine == line)) && ((type == ExpressionType.NULL) || (type == rowType));

        public static IEnumerable<Statement> GetChildren(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var children = pkb.ParentList
                .Where(x => WhereConditionIsTrue(line, x.Parent.Line, type, x.Child.Type))
                .Select(x => CreateStatamentOfType(x.Child.Type, x.Child.Line)).ToList();
            return children;
        }

        public static IEnumerable<Statement> GetChildren_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var children = pkb.GetChildren(line, type);
            foreach (var child in children)
            {
                children = children.Concat(pkb.GetChildren_(child.ProgramLine, type)).ToList();
            }

            return children;
        }

        /// <summary>
        /// Get parents list of statement set on line specified in argument filtered by type argument
        /// </summary>
        /// <param name="pkb"></param>
        /// <param name="line"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Statement> GetParents(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var parent = pkb.ParentList
                .Where(x => WhereConditionIsTrue(line, x.Child.Line, type, x.Parent.Type))
                .Select(x => CreateStatamentOfType(x.Parent.Type, x.Parent.Line)).ToList();
            return parent;
        }

        public static IEnumerable<Statement> GetParents_(this IPKBStore pkb, int line, ExpressionType type)
        {
            var parents = pkb.GetParents(line, type);
            foreach (var parent in parents)
            {
                parents = parents.Concat(pkb.GetParents_(parent.ProgramLine, type)).ToList();
            }
            return parents;
        }

        public static IEnumerable<Statement> GetFollows(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var follows = pkb.FollowsList
                .Where(x => WhereConditionIsTrue(line, x.Key.Line, type, x.Key.Type))
                .Select(x => CreateStatamentOfType(x.Value.Type, x.Value.Line)).ToList();
            return follows;
        }

        public static IEnumerable<Statement> GetFollowed(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var follows = pkb.FollowsList
                .Where(x => WhereConditionIsTrue(line, x.Value.Line, type, x.Value.Type))
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line)).ToList();
            return follows;
        }

        public static IEnumerable<Statement> GetFollows_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var follows = pkb.GetFollows(line, type);
            foreach (var follow in follows)
            {
                follows = follows.Concat(pkb.GetFollows_(follow.ProgramLine, type)).ToList();
            }
            return follows;
        }

        public static IEnumerable<Statement> GetFollowed_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var followed = pkb.GetFollowed(line, type);
            foreach (var item in followed)
            {
                followed = followed.Concat(pkb.GetFollowed_(item.ProgramLine, type)).ToList();
            }
            return followed;
        }

        public static IEnumerable<string> GetModifies(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var modifies = pkb.ModifiesList
                .Where(x => WhereConditionIsTrue(line, x.Key.Line, type, x.Key.Type))
                .Select(x => x.Value.Name).ToList();
            return modifies;
        }

        public static IEnumerable<string> GetModifies(this IPKBStore pkb, string procName, ExpressionType type = ExpressionType.NULL)
        {
            var modifies = pkb.ModifiesList
                .Where(x => x.Key.Name == procName && (type == ExpressionType.NULL || type == x.Value.Type))
                .Select(x => x.Value.Name).ToList().Distinct();
            return modifies;
        }

        public static List<Statement> GetModified(this IPKBStore pkb, int line, ExpressionType type = ExpressionType.NULL)
        {
            var modified = pkb.ModifiesList
                .Where(x => WhereConditionIsTrue(line, x.Value.Line, type, x.Value.Type))
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line)).ToList();
            return modified;
        }

        public static List<Statement> GetModified(this IPKBStore pkb, string varName, ExpressionType type = ExpressionType.NULL)
        {
            var modified = pkb.ModifiesList
                .Where(x => x.Value.Name == varName && (type == ExpressionType.NULL || type == x.Key.Type))
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line)).ToList();
            return modified;
        }

        public static List<Variable> GetAllVariables(this IPKBStore pkb)
        {
            throw new NotImplementedException();
        }

        public static List<Statement> GetAllStatements(this IPKBStore pkb)
        {
            throw new NotImplementedException();
        }

        public static List<Procedure> GetAllProcedures(this IPKBStore pkb)
        {
            throw new NotImplementedException();
        }

        public static List<int> GetAllConstants(this IPKBStore pkb)
        {
            throw new NotImplementedException();
        }

        private static Statement CreateStatamentOfType(ExpressionType type, int programLine) => type switch
            {
                ExpressionType.ASSIGN => new Assign { ProgramLine = programLine },
                ExpressionType.WHILE => new StatementWhile { ProgramLine = programLine },
                ExpressionType.IF => new StatementIf { ProgramLine = programLine },
                ExpressionType.CALL => new Call { ProgramLine = programLine },
                _ => new Statement { ProgramLine = programLine }
            };

        //next

        public static List<Statement> GetNext(this IPKBStore pkb, Statement stmt1)
        {
            throw new NotImplementedException();
        }

        public static List<Statement> GetNextStar(this IPKBStore pkb, Statement stmt1)
        {
            throw new NotImplementedException();
        }

        public static List<Statement> GetPrevious(this IPKBStore pkb, Statement stmt1)
        {
            throw new NotImplementedException();
        }

        public static List<Statement> GetPreviousStar(this IPKBStore pkb, Statement stmt1)
        {
            throw new NotImplementedException();
        }

        public static bool IsNext(this IPKBStore pkb, Statement stmt1, Statement stmt2)
        {
            throw new NotImplementedException();
        }

        public static bool IsNextStar(this IPKBStore pkb, Statement stmt1, Statement stmt2)
        {
            throw new NotImplementedException();
        }

        //Uses

        public static List<Variable> GetUsed(this IPKBStore pkb, Statement statement)
        {
            throw new NotImplementedException();
        }
        public static List<Variable> GetUsed(this IPKBStore pkb, Procedure procedure)
        {
            throw new NotImplementedException();
        }
        public static List<Statement> GetUsesStatements(this IPKBStore pkb, Variable variable)
        {
            throw new NotImplementedException();
        }
        public static List<Procedure> GetUsesProcedures(this IPKBStore pkb, Variable variable)
        {
            throw new NotImplementedException();
        }
        public static bool IsUses(this IPKBStore pkb, Variable variable, Statement statement)
        {
            throw new NotImplementedException();
        }
        public static bool IsUses(this IPKBStore pkb, Variable variable, Procedure procedure)
        {
            throw new NotImplementedException();
        }

        //Calls
        //which procedures calls procedure p?
        public static List<Procedure> GetCalls(this IPKBStore pkb, Procedure p)
        {
            throw new NotImplementedException();
        }
        //Which procedures are called from procedure p?
        public static List<Procedure> GetCalledFrom(this IPKBStore pkb, Procedure p)
        {
            throw new NotImplementedException();
        }
        //Does procedure p call q?
        public static bool IsCalls(this IPKBStore pkb, Procedure p, Procedure q)
        {
            throw new NotImplementedException();
        }
        //which procedures call procedure p?
        public static List<Procedure> GetCallsStar(this IPKBStore pkb, Procedure p)
        {
            throw new NotImplementedException();
        }
        //Which procedures are called from p?
        public static List<Procedure> GetCalledStarFrom(this IPKBStore pkb, Procedure p)
        {
            throw new NotImplementedException();
        }
        //Does procedure p call q?
        public static bool IsCallsStar(this IPKBStore pkb, Procedure p, Procedure q)
        {
            throw new NotImplementedException();
        }

    }
}
