using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQueryProcessor.Data
{
    public class FollowsQueriesData : IEnumerable<object[]>
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
               
                KeyValuePair.Create("stmt s; select s such that Follows (s,1)",  new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Follows (s,8)",  new List<string>{ "s:7 "}),
                KeyValuePair.Create("stmt s; select s such that Follows (s,9)",  new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Follows (s,10)",  new List<string>{ "s:9 "}),
                KeyValuePair.Create("stmt s; select s such that Follows (s,12)",  new List<string>{ "s:11 "}),
                KeyValuePair.Create("stmt s; select s such that Follows (s,13)",  new List<string>{ "s:10 "}),
                KeyValuePair.Create("stmt s; select s such that Follows (s,23)",  new List<string>()),
                KeyValuePair.Create("assign a; select a such that Follows (a,1)",  new List<string>()),
                KeyValuePair.Create("assign a; select a such that Follows (a,8)",  new List<string>{ "a:7 "}),
                KeyValuePair.Create("assign a; select a such that Follows (a,9)",  new List<string>()),
                KeyValuePair.Create("assign a; select a such that Follows (a,10)",  new List<string>{ "a:9 "}),
                KeyValuePair.Create("assign a; select a such that Follows (a,12)",  new List<string>{ "a:11 "}),
                KeyValuePair.Create("assign a; select a such that Follows (a,13)",  new List<string>()),
                KeyValuePair.Create("while w; stmt s; select w such that Follows* (s,w)",  new List<string>{ "w:10 "}),
                KeyValuePair.Create("while w; stmt s; select w such that Follows* (w,s)",  new List<string>{ "w:10 ","w:18 ","w:23 "}),
                KeyValuePair.Create("stmt s; select s such that Follows* (s,1)",  new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Follows* (s,8)",  new List<string>{ "s:1 ","s:2 ","s:3 ","s:4 ","s:5 ","s:6 ","s:7 "}),
                KeyValuePair.Create("stmt s; select s such that Follows* (s,9)",  new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Follows* (s,13)",  new List<string>{ "s:10 ","s:9 "}),
                KeyValuePair.Create("stmt s; select s such that Follows* (s,19)",  new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Follows* (s,22)",  new List<string>{ "s:18 "}),
                KeyValuePair.Create("if ifstat; select ifstat such that Follows* (ifstat,8)",  new List<string>()),
                KeyValuePair.Create("if ifstat; select ifstat such that Follows* (ifstat,17)",  new List<string>{ "ifstat:8 "}),
                KeyValuePair.Create("if ifstat; select ifstat such that Follows* (ifstat,25)",  new List<string>()),
                KeyValuePair.Create("assign a; select a such that Follows* (a,6)",  new List<string>{ "a:1 ","a:2 ","a:3 ","a:5 "}),
                KeyValuePair.Create("assign a; select a such that Follows* (a,9)",  new List<string>()),
                KeyValuePair.Create("assign a; select a such that Follows* (a,10)",  new List<string>{ "a:9 "}),
                KeyValuePair.Create("assign a; select a such that Follows* (a,12)",  new List<string>{ "a:11 "}),
                KeyValuePair.Create("assign a; select a such that Follows* (a,17)",  new List<string>{ "a:1 ","a:2 ","a:3 ","a:5 ","a:7 "}),
                KeyValuePair.Create("assign a; select a such that Follows* (a,28)",  new List<string>()),

            };
        }
    }
}
