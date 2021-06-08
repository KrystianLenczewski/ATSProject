using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQueryProcessor.Data
{
    public class UsesQueriesData : IEnumerable<object[]>
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
                //z pracy domowej 1
                KeyValuePair.Create("stmt s; select s such that Uses (s,\"d\")",  new List<string>{ "s:10 ","s:11 ","s:12 ","s:13 ","s:14 ","s:17 ","s:18 ","s:19 ","s:20 ","s:23 ","s:24 ","s:26 ","s:27 ","s:4 ","s:8 ","s:9 " }),
                KeyValuePair.Create("stmt s; select s such that Uses (s,\"c\")",  new List<string>{ "s:10 ","s:16 ","s:17 ","s:18 ","s:19 ","s:21 ","s:8 "}),
                KeyValuePair.Create("variable v; select v such that Uses (10,v)",  new List<string>{ "v:c ","v:d ","v:t "}),
                KeyValuePair.Create("variable v; select v such that Uses (18,v)",  new List<string>{ "v:a ","v:b ","v:c ","v:d ","v:k ","v:t "}),
                KeyValuePair.Create("variable v; select v such that Uses (23,v)",  new List<string>{ "v:a ","v:b ","v:d ","v:k ","v:t "}),
                KeyValuePair.Create("assign a; variable v; select v such that Uses (a,v)",  new List<string>{ "v:a ","v:b ","v:c ","v:d ","v:k ","v:t "}),
               
                 
            };
        }
    }
}
