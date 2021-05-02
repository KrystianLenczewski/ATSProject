using Shared;
using Shared.PQLModels;
using System.Collections.Generic;
using System.Linq;

namespace PKB
{
    public static class PKBPQLServices
    {
        public static List<Variable> GetModified(this IPKBStore pkb, Statement statement)
        {
            var modified = pkb.ModifiesList.Where(x => x.Value.Line == statement.ProgramLine).Select(x => new Variable
            {
                Name = x.Value.Name,
            });
            return modified.ToList();
        }
        public static List<Variable> GetModified(this IPKBStore pkb, Procedure procedure)
        {
            var modified = pkb.ModifiesList.Where(x => x.Value.Name == procedure.Name).Select(x => new Variable
            {
                Name = x.Value.Name
            });
            return modified.ToList();
        }
        public static List<Statement> GetModifiesStatements(this IPKBStore pkb, Variable variable)
        {
            var modifies = pkb.ModifiesList.Where(x => x.Key.Name == variable.Name).Select(x => new Statement
            {
                ProgramLine = x.Key.Line
            });
            return modifies.ToList();
        }
        public static List<Procedure> GetModifiesProcedures(this IPKBStore pkb, Variable variable)
        {
            var modifies = pkb.ModifiesList.Where(x => x.Key.Name == variable.Name).Select(x => new Procedure
            {
                Name = x.Key.Name
            });
            return modifies.ToList();
        }
        public static bool IsModified(this IPKBStore pkb, Variable variable, Statement statement)
        {
            return pkb.ModifiesList.Any(x => x.Value.Line == statement.ProgramLine && x.Key.Name == variable.Name);
        }
        public static bool IsModified(this IPKBStore pkb, Variable variable, Procedure procedure)
        {
            return pkb.ModifiesList.Any(x => x.Value.Name == procedure.Name && x.Key.Name == variable.Name);
        }
        //follows
        public static Statement GetFollows(this IPKBStore pkb, Statement statement)
        {
            var follows = pkb.FollowsList.Where(x => x.Value.Line == statement.ProgramLine).Select(x => new Statement
            {
                ProgramLine = x.Value.Line
            });
            return follows.FirstOrDefault();
        }
        public static Statement GetFollowed(this IPKBStore pkb, Statement statement)
        {
            var followed = pkb.FollowsList.Where(x => x.Key.Line == statement.ProgramLine).Select(x => new Statement
            {
                ProgramLine = x.Key.Line
            });
            return followed.FirstOrDefault();
        }
        public static List<Statement> GetFollowsStar(this IPKBStore pkb, Statement statement)
        {
            var nodes = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
            KeyValuePair<ExpressionModel, ExpressionModel>? follows = null;
            do
            {
                follows = pkb.FollowsList.FirstOrDefault(x => x.Value.Line == statement.ProgramLine);
                if (follows.HasValue) nodes.Add(follows.Value);
            } while (follows.HasValue);

            return nodes.Select(x => new Statement
            {
                ProgramLine = x.Value.Line
            }).ToList();
        }
        public static List<Statement> GetFollowedStar(this IPKBStore pkb, Statement statement)
        {
            var nodes = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
            KeyValuePair<ExpressionModel, ExpressionModel>? followed = null;
            do
            {
                followed = pkb.FollowsList.FirstOrDefault(x => x.Key.Line == statement.ProgramLine);
                if (followed.HasValue) nodes.Add(followed.Value);
            } while (followed.HasValue);

            return nodes.Select(x => new Statement
            {
                ProgramLine = x.Key.Line
            }).ToList();
        }
        public static bool IsFollows(this IPKBStore pkb, Statement stmt1, Statement stmt2)
        {
            return pkb.FollowsList.Any(x => x.Key.Line == stmt1.ProgramLine && x.Value.Line == stmt2.ProgramLine);
        }
    }
}
