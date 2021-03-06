using PKB;
using Shared;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestPKB
{
    public class PKBPQLServicesTest
    {
        [Theory]
        [InlineData(new int[0], 2)]
        [InlineData(new int[] { 8 }, 10)]
        [InlineData(new int[] { 10 }, 11)]
        [InlineData(new int[] { 18 }, 20)]
        [InlineData(new int[] { 10, 18, 23 }, 0, ExpressionType.WHILE)]
        public void GetParentTest(int[] results, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetParents(line, type);
            Assert.Equal(results.ToList(), result.Select(x => x.ProgramLine).ToList());
        }

        [Theory]
        [InlineData(new int[0], 2)]
        [InlineData(new int[] { 8 }, 10)]
        [InlineData(new int[] { 10 }, 11, ExpressionType.WHILE)]
        [InlineData(new int[] { 18 }, 20)]
        [InlineData(new int[] { 10, 18, 23 }, 0, ExpressionType.WHILE)]

        public void GetParent_Test(int[] results, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetParents_(line, type);
            Assert.Equal(results.ToList(), result.Select(x => x.ProgramLine).ToList());
        }

        [Theory]
        [InlineData(new int[0], 2)]
        [InlineData(new int[] { 10, 13, 14, 15, 16, 9 }, 8)]
        [InlineData(new int[0], 9)]
        [InlineData(new int[0], 25)]
        [InlineData(new int[] { 11 }, 10)]
        public void GetChildrenTest(int[] results, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetChildren(line, type);
            Assert.Equal(results.ToList(), result.Select(x => x.ProgramLine).ToList());
        }

        [Theory]
        [InlineData(11, 8)]
        public void GetChildren_Test(int results, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetChildren_(line, type);
            Assert.Contains(results, result.Select(x => x.ProgramLine).ToList());
        }

        [Theory]
        [InlineData(12, 11)]
        [InlineData(10, 9)]
        public void GetFollowsTest(int results, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetFollows(line, type);
            Assert.Contains(results, result.Select(x => x.ProgramLine).ToList());
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(7, 8)]
        public void GetFollowedTest(int results, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetFollowed(line, type);
            if (results == 0) Assert.Empty(result);
            else Assert.Contains(results, result.Select(x => x.ProgramLine).ToList());
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 8)]
        public void GetFollowed_Test(int[] results, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetFollowed_(line, type);
            Assert.Equal(results.ToList(), result.Select(x => x.ProgramLine).OrderBy(x => x).ToList());
        }

        [Theory]
        [InlineData(new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 1)]
        public void GetFollows_Test(int[] results, int line = 0, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetFollows_(line, type);
            Assert.Equal(results.ToList(), result.Select(x => x.ProgramLine).OrderBy(x => x).ToList());
        }

        [Theory]
        [InlineData(new string[] { "a", "c", "c" }, "Rectangle")]
        public void GetModifiesByProcnameTest(string[] results, string procName, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetModifies(procName, type);
            Assert.Equal(results.ToList(), result.OrderBy(x => x).ToList());
        }

        [Theory]
        [InlineData(new string[] { "a", "d" }, 24)]
        public void GetModifiesByParentLineTest(string[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetModifies(line, type);
            Assert.Equal(results.ToList(), result.OrderBy(x => x).ToList());
        }

        [Theory]
        [InlineData(new int[] { 10, 18, 23, 24 }, "d")]
        [InlineData(new int[] { 10, 18, 23 }, "d", ExpressionType.WHILE)]
        public void GetModifiedTest(int[] results, string varName, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetModified(varName, type);
            Assert.Equal(results.ToList(), result.Select(x => x.ProgramLine).OrderBy(x => x).ToList());
        }

        [Theory]
        [InlineData(new string[] { "c", "d", "t" }, 10)]
        [InlineData(new string[] { "a", "b", "c", "d", "k", "t" }, 18)]
        [InlineData(new string[] { "a", "b", "d", "k", "t" }, 23)]
        [InlineData(new string[] { "a", "b", "c", "d", "k", "t" }, 0, ExpressionType.ASSIGN)]
        public void GetUsedByLineTest(string[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetUsed(line, type);
            Assert.Equal(results.ToList(), result.OrderBy(x => x).Distinct().ToList());
        }

        [Theory]
        [InlineData(new string[] { "a", "d" }, "Rectangle")]
        public void GetUsedByProcedureTest(string[] results, string name, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetUsed(name, type);
            Assert.Equal(results.ToList(), result.OrderBy(x => x).Distinct().ToList());
        }

        [Theory]
        [InlineData(new int[] { 4, 8, 9, 10, 11, 12, 13, 14, 17, 18, 19, 20, 23, 24, 26, 27 }, "d")]
        public void GetUsesTest(int[] results, string varName, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetUses(varName, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new int[] { 2 }, 1)]
        [InlineData(new int[] { 3 }, 2)]
        public void GetNextTest(int[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetNext(line, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new int[] { 2, 3 }, 1)]
        [InlineData(new int[] { 10, 11, 11, 12 }, 10)]
        public void GetNext_Test(int[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetNext_(line, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new int[] { 1 }, 2)]
        [InlineData(new int[] { 2 }, 3)]
        public void GetPreviousTest(int[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetPrevious(line, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new int[] { 1, 2 }, 3)]
        public void GetPrevious_Test(int[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetPrevious_(line, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new int[] { 2, 3 }, 1)]
        [InlineData(new int[] { 1 }, 3)]
        public void GetAffects(int[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetAffects(line, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 5 }, 1)]
        [InlineData(new int[] { 1, 2, 3, 5 }, 3)]
        public void GetAffects_(int[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetAffects_(line, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult.Distinct().ToList());
        }

        [Theory]
        [InlineData(new int[] { 1 }, 2)]
        [InlineData(new int[] { 1 }, 3)]
        public void GetAffected(int[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetAffected(line, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 5)]
        public void GetAffected_(int[] results, int line, ExpressionType type = ExpressionType.NULL)
        {
            var pkb = PreparePKB();
            var result = pkb.GetAffected_(line, type);
            var preparedResult = result.Select(x => x.ProgramLine).OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult.Distinct().ToList());
        }

        [Theory]
        [InlineData(new string[] {"Circle"}, "Triangle")]
        public void GetCalls(string[] results, string procName)
        {
            var pkb = PreparePKB();
            var result = pkb.GetCalls(procName);
            var preparedResult = result.OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new string[] { "Circle", "Squere" }, "Triangle")]
        [InlineData(new string[] { "LoopProc1", "LoopProc2" }, "LoopProc1")]
        public void GetCalls_(string[] results, string procName)
        {
            var pkb = PreparePKB();
            var result = pkb.GetCalls_(procName);
            var preparedResult = result.OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult.Distinct().ToList());
        }

        [Theory]
        [InlineData(new string[] { "Triangle" }, "Circle")]
        [InlineData(new string[] { "Circle" }, "Squere")]
        public void GetCalled(string[] results, string procName)
        {
            var pkb = PreparePKB();
            var result = pkb.GetCalled(procName);
            var preparedResult = result.OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult);
        }

        [Theory]
        [InlineData(new string[] { "Circle", "Triangle"}, "Squere")]
        [InlineData(new string[] { "LoopProc1", "LoopProc2" }, "LoopProc1")]
        public void GetCalled_(string[] results, string procName)
        {
            var pkb = PreparePKB();
            var result = pkb.GetCalled_(procName);
            var preparedResult = result.OrderBy(x => x).ToList();
            Assert.Equal(results.ToList(), preparedResult.Distinct().ToList());
        }

        private IPKBStore PreparePKB()
        {
            var pkb = new PKBStore();

            // parents/children
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 1), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 2), 2));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 3), 3));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.CALL, 4), 4));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 5), 5));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.CALL, 6), 6));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 7), 7));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.IF, 8), 8));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 17), 10));

            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.WHILE, 10), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.WHILE, 13), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.WHILE, 14), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.WHILE, 15), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.WHILE, 16), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.WHILE, 9), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(StatementType.ASSIGN, 11), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(StatementType.CALL, 20), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(StatementType.IF, 24), 1));

            // follows/followed
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 1), new ExpressionModel(StatementType.ASSIGN, 2)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 2), new ExpressionModel(StatementType.ASSIGN, 3)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 3), new ExpressionModel(StatementType.CALL, 4)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(StatementType.ASSIGN, 5)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 5), new ExpressionModel(StatementType.CALL, 6)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 6), new ExpressionModel(StatementType.ASSIGN, 7)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 7), new ExpressionModel(StatementType.IF, 8)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 9)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 9), new ExpressionModel(StatementType.WHILE, 10)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 11), new ExpressionModel(StatementType.ASSIGN, 12)));

            // modifies/modified
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(StatementType.ASSIGN, "a", 2)));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(StatementType.ASSIGN, "c", 12)));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(StatementType.ASSIGN, "c", 16)));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(StatementType.ASSIGN, "d", 25)));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(StatementType.ASSIGN, "a", 26)));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(StatementType.ASSIGN, "d", 11)));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(StatementType.ASSIGN, "d", 22)));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(StatementType.ASSIGN, "d", 25)));

            // uses/used
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "t")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "t")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "t")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 2), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 14), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 5), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 16), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 9), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 28), new ExpressionModel(FactorType.VAR, "t")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 9), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 11), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 12), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 13), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 14), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 19), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 26), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 27), new ExpressionModel(FactorType.VAR, "d")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(StatementType.ASSIGN, "a", 2)));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(StatementType.ASSIGN, "d", 3)));

            // next/previous
            pkb.NextList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(StatementType.ASSIGN, "t", 1)));
            pkb.NextList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, "t", 1), new ExpressionModel(StatementType.ASSIGN, "a", 2)));
            pkb.NextList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, "a", 2), new ExpressionModel(StatementType.ASSIGN, "d", 3)));
            pkb.NextList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(StatementType.ASSIGN, "d", 11)));
            pkb.NextList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, "d", 11), new ExpressionModel(StatementType.ASSIGN, "c", 12)));
            pkb.NextList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, "c", 12), new ExpressionModel(StatementType.WHILE, "c", 10)));

            // affects/affected
            pkb.AffectsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, "t", 1), new ExpressionModel(StatementType.ASSIGN, "a", 2)));
            pkb.AffectsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, "t", 1), new ExpressionModel(StatementType.ASSIGN, "d", 3)));
            pkb.AffectsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, "d", 3), new ExpressionModel(StatementType.ASSIGN, "t", 1)));
            pkb.AffectsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, "t", 2), new ExpressionModel(StatementType.ASSIGN, "a", 5)));

            // calls/called
            pkb.CallsList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(WithNameType.PROCEDURE, "Circle")));
            pkb.CallsList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(WithNameType.PROCEDURE, "Squere")));
            pkb.CallsList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "LoopProc1"), new ExpressionModel(WithNameType.PROCEDURE, "LoopProc2")));
            pkb.CallsList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "LoopProc2"), new ExpressionModel(WithNameType.PROCEDURE, "LoopProc1")));

            return pkb;
        }
    }
}
