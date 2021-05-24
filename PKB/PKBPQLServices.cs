using Shared.PQLModels;
using System;
using System.Collections.Generic;

namespace PKB
{
    public static class PKBPQLServices
    {
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
        //modifies
        public static List<Variable> GetModified(this IPKBStore pkb, Statement statement)
        {
            throw new NotImplementedException();
        }
        public static List<Variable> GetModified(this IPKBStore pkb, Procedure procedure)
        {
            throw new NotImplementedException();
        }
        public static List<Statement> GetModifiesStatements(this IPKBStore pkb, Variable variable)
        {
            throw new NotImplementedException();
        }
        public static List<Procedure> GetModifiesProcedures(this IPKBStore pkb, Variable variable)
        {
            throw new NotImplementedException();
        }
        public static bool IsModified(this IPKBStore pkb, Variable variable, Statement statement)
        {
            throw new NotImplementedException();
        }
        public static bool IsModified(this IPKBStore pkb, Variable variable, Procedure procedure)
        {
            throw new NotImplementedException();
        }
        //follows
        public static Statement GetFollows(this IPKBStore pkb, Statement statement)
        {
            throw new NotImplementedException();
        }
        public static Statement GetFollowed(this IPKBStore pkb, Statement statement)
        {
            throw new NotImplementedException();
        }
        public static List<Statement> GetFollowsStar(this IPKBStore pkb, Statement statement)
        {
            throw new NotImplementedException();
        }
        public static List<Statement> GetFollowedStar(this IPKBStore pkb, Statement statement)
        {
            throw new NotImplementedException();
        }
        public static bool IsFollows(this IPKBStore pkb, Statement stmt1, Statement stmt2)
        {
            throw new NotImplementedException();
        }

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
