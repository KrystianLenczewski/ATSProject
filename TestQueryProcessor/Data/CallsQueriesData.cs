using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQueryProcessor.Data
{
    public class CallsQueriesData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var queryWithResult in GetQueriesWithResult())
                yield return new object[] { queryWithResult.Key, queryWithResult.Value };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<KeyValuePair<string, List<string>>> GetQueriesWithResult()
        {
            return new List<KeyValuePair<string, List<string>>>
            {

                //KeyValuePair.Create("procedure p; select p such that Calls* (p,\"Triangle\")",  new List<string>{ "p:Circle ", "p:Rectangle "}),
               // KeyValuePair.Create("procedure p; Select p such that Calls (\"Circle\",p) and Modifies (p,\"c\") and Uses (p,\"a\")",  new List<string>{ "p:Triangle ", "p:Rectangle "}),
                //KeyValuePair.Create("procedure p; select p such that Calls* (\"Circle\",p) and Modifies (p,\"c\")",  new List<string>{ "p:Triangle ", "p:Rectangle "}),
                 KeyValuePair.Create("calls c; Select c",  new List<string>{ "p:Triangle ", "p:Rectangle "}),

            };
        }
    }
}
