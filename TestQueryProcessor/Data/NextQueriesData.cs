using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQueryProcessor.Data
{
    public class NextQueriesData : IEnumerable<object[]>
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

                KeyValuePair.Create("select BOOLEAN such that Next (1,2)",  new List<string>{ "True"}),
                KeyValuePair.Create("select BOOLEAN such that Next (6,8)",  new List<string>{ "False"}),
                KeyValuePair.Create("select BOOLEAN such that Next (8,9)",  new List<string>{ "True"}),
                KeyValuePair.Create("select BOOLEAN such that Next (10,11)",  new List<string>{ "True"}),
                KeyValuePair.Create("select BOOLEAN such that Next (12,10)",  new List<string>{ "True"}),
                KeyValuePair.Create("select BOOLEAN such that Next (12,11)",  new List<string>{ "False"}),
                KeyValuePair.Create("select BOOLEAN such that Next (13,14)",  new List<string>{ "False"}),
                KeyValuePair.Create("select BOOLEAN such that Next (17,18)",  new List<string>{ "False"}),
                KeyValuePair.Create("select BOOLEAN such that Next (23,27)",  new List<string>{ "True"}),
                KeyValuePair.Create("select BOOLEAN such that Next* (9,9)",  new List<string>{ "False"}),
                KeyValuePair.Create("select BOOLEAN such that Next* (10,10)",  new List<string>{ "True"}),
                KeyValuePair.Create("select BOOLEAN such that Next* (12,12)",  new List<string>{ "True"}),
                KeyValuePair.Create("select BOOLEAN such that Next* (20,20)",  new List<string>{ "True"}),

            };
        }
    }
}
