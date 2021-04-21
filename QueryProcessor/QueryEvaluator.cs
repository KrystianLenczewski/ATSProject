using PKB;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor
{
    public class QueryEvaluator
    {
        private readonly PKBStore _pkbStore;

        public QueryEvaluator()
        {
            //_pkbStore = PKBStore.Instance;
        }

        public string GetResultsRaw(QueryTree queryTree)
        {
            throw new NotImplementedException();
        }
    }
}
