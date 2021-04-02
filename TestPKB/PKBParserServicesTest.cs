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
            var s1Obj = new ExpressionModel(s1);
            var s2Obj = new ExpressionModel(s2);
            var pkb = new PKBStore();
            pkb.SetFollowsAction(s1Obj, s2Obj);
            Assert.Collection(pkb.FollowsList, item => Assert.Equal(KeyValuePair.Create(s1Obj, s2Obj), item));
        }

        [Theory]
        [InlineData(SpecialType.STMTLST)]
        [InlineData(SpecialType.PROCEDURE)]
        public void SetParentProcedureListTest(SpecialType s)
        {
            var procedureObj = new ExpressionModel(SpecialType.PROCEDURE);
            var sObj = new ExpressionModel(s);
            var pkb = new PKBStore();
            var x = new ExpressionModel(FactorType.VAR, "x");
            pkb.SetParentAction(procedureObj, x, 0);
            pkb.SetParentAction(procedureObj, sObj, 1);
            if (s == SpecialType.STMTLST) Assert.Equal(pkb.ParentList[1].Child, sObj);
            else Assert.Equal(pkb.ParentList.Last(), pkb.ParentList.First());
        }

        [Theory]
        [InlineData(FactorType.CONST)]
        [InlineData(FactorType.VAR)]
        public void SetParentProcedureFactorTest(FactorType s)
        {
            var parentObj = new ExpressionModel(SpecialType.PROCEDURE);
            var sObj = new ExpressionModel(s, "x");
            var pkb = new PKBStore();
            var testObj = new ParentModel(parentObj, sObj, 0);
            pkb.SetParentAction(testObj.Parent, testObj.Child, testObj.Index);
            if (ExpressionType.VAR == sObj.Type) Assert.Collection(pkb.ParentList, item =>
            {
                Assert.Equal(testObj.Child, item.Child); 
                Assert.Equal(testObj.Index, item.Index);
                Assert.Equal(testObj.Parent, item.Parent);
            });
            else Assert.Empty(pkb.ParentList);
        }

        [Theory]
        [InlineData(StatementType.ASSIGN)]
        [InlineData(StatementType.WHILE)]
        public void SetParentStmtLstTest(StatementType s)
        {
            var parentObj = new ExpressionModel(SpecialType.STMTLST);
            var sObj = new ExpressionModel(s);
            var pkb = new PKBStore();
            var testObj = new ParentModel(parentObj, sObj, 0);
            pkb.SetParentAction(testObj.Parent, testObj.Child, testObj.Index);
            Assert.Collection(pkb.ParentList, item =>
            {
                Assert.Equal(testObj.Child, item.Child);
                Assert.Equal(testObj.Index, item.Index);
                Assert.Equal(testObj.Parent, item.Parent);
            });
        }
    }
}
