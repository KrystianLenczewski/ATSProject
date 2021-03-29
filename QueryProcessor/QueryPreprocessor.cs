using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor
{
    public class QueryPreprocessor
    {
        QueryTree queryTree = new QueryTree();
        RelTable relTable = new RelTable();
        public QueryPreprocessor()
        {

        }

        //stmt s1, s2;
        //Select s1 such that Follows(s1, s2)
       public QueryTree ParseQuery(string query)
        {
            //dostanie tylko nazwe relacji i zapytanie czy dana relacja posiada 
            string relationName=null;
            int argumentsCount=2;
            bool isQueryValid=relTable.ValidateRelation(relationName, argumentsCount,RelationArgumentType.Statement, RelationArgumentType.Statement);
            
            if(isQueryValid)
            {
               // queryTree.createQueryTree()
            }

            throw new ArgumentException();
     
            return new QueryTree();
        }


    }
}
