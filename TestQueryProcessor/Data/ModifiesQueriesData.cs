using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQueryProcessor.Data
{
    public class ModifiesQueriesData: IEnumerable<object[]>
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
                KeyValuePair.Create("variable v; select v such that Modifies (3,v)",  new List<string>{ "v:d " }),
                KeyValuePair.Create("variable v; select v such that Modifies (4,v)",  new List<string>{ "v:a ","v:c ","v:d " }),
                KeyValuePair.Create("variable v; select v such that Modifies (6,v)",  new List<string>{ "v:t " }),
                KeyValuePair.Create("variable v; select v such that Modifies (18,v)",  new List<string>{ "v:a ","v:c ","v:d ", "v:t "}),
                //KeyValuePair.Create("variable v; select v such that Modifies (24,v)",  new List<string>{ "v:a ","v:d "}),
                KeyValuePair.Create("variable v; select v such that Modifies (28,v)",  new List<string>{ "v:t "}),
                KeyValuePair.Create("while w; select w such that Modifies (w,\"d\")",  new List<string>{ "w:10 ","w:18 ","w:23 "}),
                KeyValuePair.Create("while w; select w such that Modifies (w,\"c\")",  new List<string>{ "w:10 ","w:18 "}),
                KeyValuePair.Create("variable v; select v such that Modifies (\"Rectangle\",v)",  new List<string>{ "v:a ","v:c ","v:d ","v:t "}),
                KeyValuePair.Create("assign a; select a such that Modifies (a,\"a\") and Uses (a,\"a\")",  new List<string>{ "a:26 "}),
               // KeyValuePair.Create("assign a; select a such that Modifies (a,\"d\") and Uses (a,\"d\")",  new List<string>{ "a:11 "}),
                KeyValuePair.Create("assign a; select a such that Modifies (a,\"b\") and Uses (a,\"b\")",  new List<string>()),
                //KeyValuePair.Create("assign a; select a such that Modifies (a,\"c\") and Uses (a,\"c\")",  new List<string>{"a:16 ","a:21 "}),
                KeyValuePair.Create("while w; assign a; select a such that Modifies (a,\"t\") and Parent (w,a)",  new List<string>{"a:19 "}),
                KeyValuePair.Create("while w; assign a; select a such that Parent (w,a) and Modifies (a,\"t\")",  new List<string>{"a:19 "}),
                //KeyValuePair.Create("while w; assign a; select a such that and Modifies (a,\"t\") such that Parent (w,a)",  new List<string>{"a:19 "}),
                 
            };
        }
    }
}
