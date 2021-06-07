﻿using PKB;
using QueryProcessor.QueryProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQueryProcessor.Data;
using TestQueryProcessor.Initializers;
using Xunit;

namespace TestQueryProcessor
{
    public class QueryEvaluatorTests
    {
        [Theory]
        [ClassData(typeof(ParentQueriesData))]
        public void ParentRelationHandlingWorksProperly(string query, List<string> expectedResult)
        {
            List<string> result = GetQueryResultRaw(query, PKBStoreInitializer.InitializePKB());
            Assert.True(SequenceEqualsIgnoringOrder(expectedResult, result));
        }

        private List<string> GetQueryResultRaw(string query, IPKBStore pkbStore)
        {
            QueryPreprocessor queryPreprocessor = new QueryPreprocessor();
            QueryTree queryTree = queryPreprocessor.ParseQuery(query);
            QueryEvaluator queryEvaluator = new QueryEvaluator(PKBStoreInitializer.InitializePKB());

            return queryEvaluator.GetQueryResultsRaw(queryTree);
        }

        private bool SequenceEqualsIgnoringOrder(List<string> firstList, List<string> secondList)
        {
            return Enumerable.SequenceEqual(firstList.OrderBy(o => o), secondList.OrderBy(o => o));
        }
    }
}
