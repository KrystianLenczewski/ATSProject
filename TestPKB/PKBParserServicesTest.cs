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
            var s1Obj = new ExpressionModel(s1,0);
            var s2Obj = new ExpressionModel(s2,0);
            var pkb = new PKBStore();
            pkb.SetFollows(s1Obj, s2Obj);
            Assert.Collection(pkb.FollowsList, item => Assert.Equal(KeyValuePair.Create(s1Obj, s2Obj), item));
        }

        [Theory]
        [InlineData(SpecialType.STMTLST)]
        public void SetParentProcedureListTest(SpecialType s)
        {
            var procedureObj = new ExpressionModel(SpecialType.PROCEDURE);
            var sObj = new ExpressionModel(s);
            var pkb = new PKBStore();
            var x = new ExpressionModel(FactorType.VAR, "x");
            pkb.SetParent(procedureObj, x, 0);
            pkb.SetParent(procedureObj, sObj, 1);
            Assert.Equal(pkb.ParentList.Last().Child, sObj);
        }

        [Theory]
        [InlineData(FactorType.VAR)]
        public void SetParentProcedureFactorTest(FactorType s)
        {
            var parentObj = new ExpressionModel(SpecialType.PROCEDURE);
            var sObj = new ExpressionModel(s, "x");
            var pkb = new PKBStore();
            var testObj = new ParentModel(parentObj, sObj, 0);
            pkb.SetParent(testObj.Parent, testObj.Child, testObj.Index);
            Assert.Collection(pkb.ParentList, item =>
            {
                Assert.Equal(testObj.Child, item.Child); 
                Assert.Equal(testObj.Index, item.Index);
                Assert.Equal(testObj.Parent, item.Parent);
            });
        }

        [Theory]
        [InlineData(StatementType.ASSIGN)]
        [InlineData(StatementType.WHILE)]
        public void SetParentStmtLstTest(StatementType s)
        {
            var parentObj = new ExpressionModel(SpecialType.STMTLST);
            var sObj = new ExpressionModel(s,0);
            var pkb = new PKBStore();
            var testObj = new ParentModel(parentObj, sObj, 0);
            pkb.SetParent(testObj.Parent, testObj.Child, testObj.Index);
            Assert.Collection(pkb.ParentList, item =>
            {
                Assert.Equal(testObj.Child, item.Child);
                Assert.Equal(testObj.Index, item.Index);
                Assert.Equal(testObj.Parent, item.Parent);
            });
        }

        /*
        using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKB
{
    public static class PKBParserServices
    {
        public static void SetFollows(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {
            var allowedTypes = Enum.GetValues(typeof(StatementType)).Cast<ExpressionType>();
            var stmt1 = allowedTypes.Contains(s1.Type);
            var stmt2 = allowedTypes.Contains(s2.Type);
            if (!(stmt1 && stmt2)) return;
            var value = KeyValuePair.Create(s1, s2);
            pkb.FollowsList.Add(value);
        }

        public static void SetParent(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2, int index)
        {
            if (IsValidParentProcedureOrWhile(s1.Type, s2.Type, index) || IsValidParentStmtLst(s1.Type, s2.Type, index) || IsValidAssign(s1.Type, s2.Type, index) || IsValidOperation(s1.Type, s2.Type))
            {
                pkb.ParentList.Add(new ParentModel(s1, s2, index));
            }
        }

        public static void SetModify(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {
            if (!AreValidModifyAndUsesParams(s1,s2)) return;
            // TODO: VALIDATION
            pkb.ModifiesList.Add(new KeyValuePair<ExpressionModel, ExpressionModel>(s1, s2));
        }

        public static void SetUses(this IPKBStore pkb, ExpressionModel s1, ExpressionModel s2)
        {
            if (!AreValidModifyAndUsesParams(s1, s2)) return;
            // TODO: VALIDATION
            pkb.UsesList.Add(new KeyValuePair<ExpressionModel, ExpressionModel>(s1, s2));
        }

        private static bool AreValidModifyAndUsesParams(ExpressionModel s1, ExpressionModel s2) => IsDefinedIn(typeof(StatementType), s1.Type) && (s2.Type != ExpressionType.VAR);

        private static bool ExpressionName(int index, ExpressionType childType) => (index == 0 && childType == ExpressionType.VAR);
        private static bool IsDefinedIn(Type type, ExpressionType childType) => Enum.IsDefined(type, (int)childType);
        private static bool IsValidParentProcedureOrWhile(ExpressionType parentType, ExpressionType childType, int index) => 
            ((parentType == ExpressionType.PROCEDURE) || (parentType == ExpressionType.WHILE)) && (ExpressionName(index,childType) || (index == 1 && childType == ExpressionType.STMTLST));
        private static bool IsValidParentStmtLst(ExpressionType parentType, ExpressionType childType, int index) => 
            (parentType == ExpressionType.STMTLST) && IsDefinedIn(typeof(StatementType), childType) && (index == 0);
        private static bool IsValidAssign(ExpressionType parentType, ExpressionType childType, int index) => 
            (parentType == ExpressionType.ASSIGN) && (ExpressionName(index, childType) || (index == 1 && IsDefinedIn(typeof(OperationsType), childType)));
        private static bool IsValidOperation(ExpressionType parentType, ExpressionType childType) =>
            IsDefinedIn(typeof(OperationsType), parentType) && IsDefinedIn(typeof(FactorType), childType);
    }
}

         */
    }
}
