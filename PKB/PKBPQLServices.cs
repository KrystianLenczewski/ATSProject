using Shared;
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

        public static IEnumerable<Statement> GetChildren_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL, List<Statement> lockList = null)
        {
            if (lockList == null) lockList = new List<Statement>();
            var children = pkb.GetChildren(line, type);
            var filtered = children.Where(x => !lockList.Select(x => x.ProgramLine).Contains(x.ProgramLine)).ToList();
            lockList.AddRange(filtered);
            foreach (var child in filtered)
            {
                children = children.Concat(pkb.GetChildren_(child.ProgramLine, type, lockList));
            }

            return children;
        }

        public static IEnumerable<Statement> GetParents(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var parent = pkb.ParentList
                .Where(x => WhereConditionIsTrue(line, x.Child.Line, type, x.Parent.Type) && x.Parent.Line > 0)
                .Select(x => CreateStatamentOfType(x.Parent.Type, x.Parent.Line));
            return parent;
        }

        public static IEnumerable<Statement> GetParents_(this IPKBStore pkb, int line, ExpressionType type, List<Statement> lockList = null)
        {
            if (lockList == null) lockList = new List<Statement>();
            var parents = pkb.GetParents(line, type);
            var filtered = parents.Where(x => !lockList.Select(x => x.ProgramLine).Contains(x.ProgramLine)).ToList();
            lockList.AddRange(filtered);
            foreach (var parent in filtered)
            {
                parents = parents.Concat(pkb.GetParents_(parent.ProgramLine, type, lockList));
            }
            return parents;
        }

        public static IEnumerable<Statement> GetFollows(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var follows = pkb.FollowsList
                .Where(x => WhereConditionIsTrue(line, x.Key.Line, type, x.Key.Type))
                .Select(x => CreateStatamentOfType(x.Value.Type, x.Value.Line));
            return follows;
        }

        public static IEnumerable<Statement> GetFollowed(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var follows = pkb.FollowsList
                .Where(x => WhereConditionIsTrue(line, x.Value.Line, type, x.Value.Type))
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line));
            return follows;
        }

        public static IEnumerable<Statement> GetFollows_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL, List<Statement> lockList = null)
        {
            if (lockList == null) lockList = new List<Statement>();
            var follows = pkb.GetFollows(line, type);
            var filtered = follows.Where(x => !lockList.Select(x => x.ProgramLine).Contains(x.ProgramLine)).ToList();
            lockList.AddRange(filtered);
            foreach (var follow in filtered)
            {
                follows = follows.Concat(pkb.GetFollows_(follow.ProgramLine, type, lockList));
            }
            return follows;
        }

        public static IEnumerable<Statement> GetFollowed_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL, List<Statement> lockList = null)
        {
            if (lockList == null) lockList = new List<Statement>();
            var followed = pkb.GetFollowed(line, type);
            var filtered = followed.Where(x => !lockList.Select(x => x.ProgramLine).Contains(x.ProgramLine)).ToList();
            lockList.AddRange(filtered);
            foreach (var item in filtered)
            {
                followed = followed.Concat(pkb.GetFollowed_(item.ProgramLine, type, lockList));
            }
            return followed;
        }

        public static IEnumerable<string> GetModifies(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var modifies = pkb.ModifiesList
                .Where(x => WhereConditionIsTrue(line, x.Key.Line, type, x.Key.Type))
                .Select(x => x.Value.Name);
            return modifies;
        }

        public static IEnumerable<string> GetModifies(this IPKBStore pkb, string procName, ExpressionType type = ExpressionType.NULL)
        {
            var modifies = pkb.ModifiesList
                .Where(x => x.Key.Name == procName && (type == ExpressionType.NULL || type == x.Value.Type))
                .Select(x => x.Value.Name);
            return modifies;
        }

        public static IEnumerable<Statement> GetModified(this IPKBStore pkb, int line, ExpressionType type = ExpressionType.NULL)
        {
            var modified = pkb.ModifiesList
                .Where(x => WhereConditionIsTrue(line, x.Value.Line, type, x.Value.Type))
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line));
            return modified;
        }

        public static IEnumerable<Statement> GetModified(this IPKBStore pkb, string varName, ExpressionType type = ExpressionType.NULL)
        {
            var modified = pkb.ModifiesList
                .Where(x => x.Value.Name == varName && (type == ExpressionType.NULL || type == x.Key.Type))
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line));
            return modified;
        }

        public static IEnumerable<string> GetUsed(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var used = pkb.UsesList
                .Where(x => WhereConditionIsTrue(line, x.Key.Line, type, x.Key.Type))
                .Select(x => x.Value.Name);
            return used;
        }

        public static IEnumerable<string> GetUsed(this IPKBStore pkb, string name, ExpressionType type = ExpressionType.NULL)
        {
            var used = pkb.UsesList
                .Where(x => x.Key.Name == name && (type == x.Value.Type || type == ExpressionType.NULL))
                .Select(x => x.Value.Name);
            return used;
        }

        public static IEnumerable<Statement> GetUses(this IPKBStore pkb, string name, ExpressionType type = ExpressionType.NULL)
        {
            var uses = pkb.UsesList
                .Where(x => x.Value.Name == name && (type == ExpressionType.NULL || type == x.Key.Type) && x.Key.Line > 0)
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line));
            return uses;
        }

        public static IEnumerable<Statement> GetNext(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var next = pkb.NextList
                .Where(x => WhereConditionIsTrue(line, x.Key.Line, type, x.Key.Type))
                .Select(x => CreateStatamentOfType(x.Value.Type, x.Value.Line));
            return next;
        }

        public static IEnumerable<Statement> GetNext_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL, List<Statement> lockList = null)
        {
            if (lockList == null) lockList = new List<Statement>();
            var next = pkb.GetNext(line, type);
            var filtered = next.Where(x => !lockList.Select(x => x.ProgramLine).Contains(x.ProgramLine)).ToList();
            lockList.AddRange(filtered);
            foreach (var item in filtered)
            {
                next = next.Concat(pkb.GetNext_(item.ProgramLine, type, lockList));
            }
            return next;
        }

        public static IEnumerable<Statement> GetPrevious(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var previous = pkb.NextList
                .Where(x => WhereConditionIsTrue(line, x.Value.Line, type, x.Value.Type) && x.Key.Line > 0)
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line));
            return previous;
        }

        public static IEnumerable<Statement> GetPrevious_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL, List<Statement> lockList = null)
        {
            if (lockList == null) lockList = new List<Statement>();
            var previous = pkb.GetPrevious(line, type);
            var filtered = previous.Where(x => !lockList.Select(x => x.ProgramLine).Contains(x.ProgramLine)).ToList();
            lockList.AddRange(filtered);
            foreach (var item in filtered)
            {
                previous = previous.Concat(pkb.GetPrevious_(item.ProgramLine, type, lockList));
            }
            return previous;
        }

        public static IEnumerable<Statement> GetAffects(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var affects = pkb.AffectsList
                .Where(x => WhereConditionIsTrue(line, x.Key.Line, type, x.Key.Type))
                .Select(x => CreateStatamentOfType(x.Value.Type, x.Value.Line));
            return affects;
        }

        public static IEnumerable<Statement> GetAffects_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL, List<Statement> lockList = null)
        {
            if (lockList == null) lockList = new List<Statement>();
            var affects = pkb.GetAffects(line, type);
            var filtered = affects.Where(x => !lockList.Select(x => x.ProgramLine).Contains(x.ProgramLine)).ToList();
            lockList.AddRange(filtered);
            foreach (var item in filtered)
            {
                affects = affects.Concat(pkb.GetAffects_(item.ProgramLine, type, lockList));
            }
            return affects;
        }

        public static IEnumerable<Statement> GetAffected(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var affects = pkb.AffectsList
                .Where(x => WhereConditionIsTrue(line, x.Value.Line, type, x.Value.Type))
                .Select(x => CreateStatamentOfType(x.Key.Type, x.Key.Line));
            return affects;
        }

        public static IEnumerable<Statement> GetAffected_(this IPKBStore pkb, int line = 0, ExpressionType type = ExpressionType.NULL, List<Statement> lockList = null)
        {
            if (lockList == null) lockList = new List<Statement>();
            var affected = pkb.GetAffected(line, type);
            var filtered = affected.Where(x => !lockList.Select(x => x.ProgramLine).Contains(x.ProgramLine)).ToList();
            lockList.AddRange(filtered);
            foreach (var item in filtered)
            {
                affected = affected.Concat(pkb.GetAffected_(item.ProgramLine, type, lockList));
            }
            return affected;
        }

        public static IEnumerable<string> GetCalls(this IPKBStore pkb, string procName)
        {
            var procedures = pkb.CallsList
                .Where(x => x.Key.Name == procName)
                .Select(x => x.Value.Name);
            return procedures;
        }

        public static IEnumerable<string> GetCalls_(this IPKBStore pkb, string procName, List<string> lockList = null)
        {
            if (lockList == null) lockList = new List<string>();
            var procedures = pkb.GetCalls(procName);
            var filtered = procedures.Where(x => !lockList.Contains(x)).ToList();
            lockList.AddRange(filtered);
            foreach (var item in filtered)
            {
                procedures = procedures.Concat(pkb.GetCalls_(item, lockList));
            }
            return procedures;
        }

        public static IEnumerable<string> GetCalled(this IPKBStore pkb, string procName)
        {
            var procedures = pkb.CallsList
                .Where(x => x.Value.Name == procName)
                .Select(x => x.Key.Name);
            return procedures;
        }

        public static IEnumerable<string> GetCalled_(this IPKBStore pkb, string procName, List<string> lockList = null)
        {
            if (lockList == null) lockList = new List<string>();
            var procedures = pkb.GetCalled(procName);
            var filtered = procedures.Where(x => !lockList.Contains(x)).ToList();
            lockList.AddRange(filtered);
            foreach (var item in filtered)
            {
                procedures = procedures.Concat(pkb.GetCalled_(item, lockList));
            }
            return procedures;
        }

        private static Statement CreateStatamentOfType(ExpressionType type, int programLine = 0) => type switch
            {
                ExpressionType.ASSIGN => new Assign { ProgramLine = programLine },
                ExpressionType.WHILE => new StatementWhile { ProgramLine = programLine },
                ExpressionType.IF => new StatementIf { ProgramLine = programLine },
                ExpressionType.CALL => new Call { ProgramLine = programLine },
                _ => new Statement { ProgramLine = programLine }
            };
    }
}
