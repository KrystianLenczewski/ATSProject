using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace PKB.Relations
{
    public class Modifies
    {
        void setModifies(Statement stmt, Variable var)
        {

        }
        void setModifies(Procedure proc, Variable var)
        {

        }
        List<Variable> getModified(Statement stmt)
        {
            throw new NotImplementedException();
        }
        List<Variable> getModified(Procedure proc)
        {
            throw new NotImplementedException();
        }
        List<Statement> getModifiesForStatement(Variable var)
        {
            throw new NotImplementedException();
        }
        List<Procedure> getModifiesForProcedure(Variable var)
        {
            throw new NotImplementedException();
        }
        bool isModified(Variable var, Statement sta)
        {
            throw new NotImplementedException();
        }
        bool isModified(Variable var, Procedure proc)
        {
            throw new NotImplementedException();
        }
    }
}
