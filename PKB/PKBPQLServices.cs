using Shared;
using Shared.PQLModels;
using System;
using System.Collections.Generic;

namespace PKB
{
    public static class PKBPQLServices
    {
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


    }
}
