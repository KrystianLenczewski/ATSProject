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

                KeyValuePair.Create("prog_line n2; select BOOLEAN such that Next (1,2)",  new List<string>{ "true "}),

            };
        }
    }
}
