using QueryProcessor;
using QueryProcessor.QueryProcessing;
using System.Collections.Generic;

namespace PQLTestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryPreprocessor = new QueryPreprocessor();

            string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p such that Modifies (p,v) and Modifies (p2,v2) and Modifies (p3,v3) with p.procName=v.varName and p3.procname=\"abc\" and p2.procName=\"def\""; // zapytanie z 3 WITH i 3 SUCH_THAT

            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p with p.procName=v.varName"; // zapytanie z 1 WITH
            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p such that Modifies (p,v)"; // zapytanie z 1 SUCH_THAT
            //string query = "assign a,a1; stmt s,s1; variable v; procedure p; constant c; select p such that Modifies (p,v) with p.procName=\"proce\""; // zapytanie z 1 WITH i 1 SUCH_THAT

            // niepoprawnie zapisane zapytania - wykrywa błąd
            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p such that Modifies (p,v) aned Modifies (p2,v2) with p.procName=v.varName aned p3.procname=\"abc\" and p2.procName=\"def\""; // zapytanie z blednie zapisanym AND (walidacja)
            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p with p.procName=v.varName aned p3.procname=\"abc\" and p2.procName=\"def\""; // zapytanie z blednie zapisanym AND (walidacja)
            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p such that Modifies (p,v) and"; // zapytanie z AND na koncu SUCH THAT
            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p with p.procName=v.varName and p3.procname=\"abc\" and with "; // zapytanie ze zlym arg po AND w klauzurze WITH
            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p with p.procName=v.varName and p3.procname=\"abc\" and"; // zapytanie z AND na koncu WTIH
            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p such that Modifies (p,v) and x"; // zapytanie ze zlym arg po AND w klauzurze SUCH THAT
            //string query = "assign a,a1; stmt s,s1; variable v,v2,v3; procedure p,p2,p3; constant c; select p such that Modifies (p,v) and with p.procName=v.varName and p3.procname=\"abc\" with p.procName=v.varName and p3.procname=\"abc\" and x"; // zapytanie ze zlym arg po AND w klauzurze WITH

            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator();
            List<object> queryResultsRaw = queryEvaluator.GetQueryResultsRaw(queryTree);
        }
    }
}
