using Shared.PQLModels;
using System;
using System.Collections.Generic;

namespace PKB
{
    public static class PKBPQLServices
    {
        public static List<Variable> GetAllVariables()
        {
            throw new NotImplementedException();
        }

        public static List<Statement> GetAllStatements()
        {
            throw new NotImplementedException();
        }

        public static List<Procedure> GetAllProcedures()
        {
            throw new NotImplementedException();
        }

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
    }
}
