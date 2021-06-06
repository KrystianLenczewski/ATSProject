using PKB;
using Shared;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestPKB
{
    public class PKBParserServicesTest
    {
        [Fact]
        public void ModifiesTest()
        {
            var pkb = new PKBStore();
            pkb.SetModify(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "a"));
            pkb.SetModify(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "b"));

            Assert.Equal(2, pkb.ModifiesList.Count);
        }

        [Fact]
        public void FollowsTest()
        {
            var pkb = new PKBStore();
            pkb.SetFollows(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "a"));
            pkb.SetFollows(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "b"));

            Assert.Equal(2, pkb.FollowsList.Count);
        }

        [Fact]
        public void UsesTest()
        {
            var pkb = new PKBStore();
            pkb.SetUses(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "a"));
            pkb.SetUses(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "b"));

            Assert.Equal(2, pkb.UsesList.Count);
        }

        [Fact]
        public void NextTest()
        {
            var pkb = new PKBStore();
            pkb.SetNext(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "a"));
            pkb.SetNext(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "b"));

            Assert.Equal(2, pkb.NextList.Count);
        }

        [Fact]
        public void AffectsTest()
        {
            var pkb = new PKBStore();
            pkb.SetAffects(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "a"));
            pkb.SetAffects(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "b"));

            Assert.Equal(2, pkb.AffectsList.Count);
        }

        [Fact]
        public void CallsTest()
        {
            var pkb = new PKBStore();
            pkb.SetCalls(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "a"));
            pkb.SetCalls(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "b"));

            Assert.Equal(2, pkb.CallsList.Count);
        }

        [Fact]
        public void ParentsTest()
        {
            var pkb = new PKBStore();
            pkb.SetParent(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "a"), 1);
            pkb.SetParent(
                new ExpressionModel(StatementType.ASSIGN, 1),
                new ExpressionModel(FactorType.VAR, "b"), 1);

            Assert.Equal(2, pkb.ParentList.Count);
        }
    }
}
