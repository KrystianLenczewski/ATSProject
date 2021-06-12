using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQueryProcessor.Data
{
    public class ParentQueriesData : IEnumerable<object[]>
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
                KeyValuePair.Create("stmt s,s1; select s such that Parent (s,s1) with s1.stmt#=2", new List<string>()),
                KeyValuePair.Create("stmt s,s1; select s such that Parent (s,s1) with s1.stmt#=10", new List<string>{ "s:8 " }),
                KeyValuePair.Create("stmt s,s1; select s such that Parent (s,s1) with s1.stmt#=11", new List<string>{ "s:10 " }),
                KeyValuePair.Create("stmt s,s1; select s such that Parent (s,s1) with s1.stmt#=20", new List<string>{ "s:18 " }),
                KeyValuePair.Create("stmt s; select s such that Parent (s,2)", new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Parent (s,10)", new List<string>{ "s:8 " }),
                KeyValuePair.Create("stmt s; select s such that Parent (s,11)", new List<string>{ "s:10 " }),
                KeyValuePair.Create("stmt s; select s such that Parent (s,20)", new List<string>{ "s:18 " }),
                KeyValuePair.Create("stmt s; select s such that Parent (2,s)", new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Parent (8,s)", new List<string>{"s:10 ","s:13 ","s:14 ","s:15 ","s:16 ","s:9 " }),
                KeyValuePair.Create("stmt s; select s such that Parent (9,s)", new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Parent (25,s)", new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Parent* (s,2)", new List<string>()),
                KeyValuePair.Create("stmt s; select s such that Parent* (s,10)", new List<string>{ "s:8 " }),
                KeyValuePair.Create("stmt s; select s such that Parent* (s,11)", new List<string>{ "s:8 ", "s:10 " }),
                KeyValuePair.Create("stmt s; select s such that Parent* (s,20)", new List<string>{ "s:18 " }),
                KeyValuePair.Create("stmt s; while w; select w such that Parent* (s,2)", new List<string>()),
                KeyValuePair.Create("stmt s; while w; select w such that Parent* (s,10)", new List<string>{ "w:[10,18,23] " }),
                KeyValuePair.Create("stmt s; while w; select w such that Parent* (s,11)", new List<string>{ "w:[10,18,23] " }),
                KeyValuePair.Create("stmt s; while w; select w such that Parent* (s,20)", new List<string>{ "w:[10,18,23] " }),
                KeyValuePair.Create("while w; select w such that Parent* (w,9)", new List<string>()),
                KeyValuePair.Create("while w; select w such that Parent* (w,11)", new List<string>{ "w:10 " }),
                KeyValuePair.Create("while w; select w such that Parent* (w,13)", new List<string>()),
                KeyValuePair.Create("while w; select w such that Parent* (w,21)", new List<string>{ "w:18 " }),
                KeyValuePair.Create("while w; select BOOLEAN such that Parent (18,19)", new List<string>{ "True" }),
                KeyValuePair.Create("while w; select BOOLEAN such that Parent (19,18)", new List<string>{ "False" }),
            };
        }
    }
}
