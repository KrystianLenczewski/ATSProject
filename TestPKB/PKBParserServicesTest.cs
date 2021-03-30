using PKB;
using Shared;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestPKB
{
    public class PKBParserServicesTest
    {
        [Theory]
        [InlineData(StatementType.WHILE, StatementType.ASSIGN)]
        [InlineData(StatementType.ASSIGN, StatementType.WHILE)]
        [InlineData(StatementType.WHILE, StatementType.WHILE)]
        [InlineData(StatementType.ASSIGN, StatementType.ASSIGN)]

        public void SetFollowsTest(StatementType s1, StatementType s2)
        {
            var s1Obj = new Expression(s1);
            var s2Obj = new Expression(s2);
            var pkb = new PKBStore();
            pkb.SetFollowsAction(s1Obj, s2Obj);
            Assert.Collection(pkb.FollowsList, item => Assert.Equal(KeyValuePair.Create(s1Obj, s2Obj), item));
        }

        [Theory]
        [InlineData(SpecialType.STMTLST)]
        [InlineData(SpecialType.PROCEDURE)]
        public void SetParentProcedureListTest(SpecialType s)
        {
            var procedureObj = new Expression(SpecialType.PROCEDURE);
            var sObj = new Expression(s);
            var pkb = new PKBStore();
            var x = new Expression(FactorType.CONST, "x");
            pkb.SetParentAction(procedureObj, x, 0);
            pkb.SetParentAction(procedureObj, sObj, 1);
            if (s == SpecialType.STMTLST) Assert.Equal(pkb.ParentList[1].Value, sObj);
            else Assert.Equal(pkb.ParentList.Last(), pkb.ParentList.First());
        }

        [Theory]
        [InlineData(FactorType.CONST)]
        [InlineData(FactorType.VAR)]
        public void SetParentProcedureFactorTest(FactorType s)
        {
            var parentObj = new Expression(SpecialType.PROCEDURE);
            var sObj = new Expression(s, "x");
            var pkb = new PKBStore();
            pkb.SetParentAction(parentObj, sObj, 0);
            if (ExpressionType.CONST == sObj.Type) Assert.Collection(pkb.ParentList, item => Assert.Equal(KeyValuePair.Create(parentObj, sObj), item));
            else Assert.Empty(pkb.ParentList);
        }

        [Theory]
        [InlineData(StatementType.ASSIGN)]
        [InlineData(StatementType.WHILE)]
        public void SetParentStmtLstTest(StatementType s)
        {
            var parentObj = new Expression(SpecialType.STMTLST);
            var sObj = new Expression(s);
            var pkb = new PKBStore();
            pkb.SetParentAction(parentObj, sObj, 0);
            Assert.Collection(pkb.ParentList, item => Assert.Equal(KeyValuePair.Create(parentObj, sObj), item));
        }
    }
}
