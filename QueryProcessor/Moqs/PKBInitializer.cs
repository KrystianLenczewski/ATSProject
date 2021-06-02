using PKB;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.Moqs
{
    public static class PKBInitializer
    {
        public static PKBStore InitializePKB()
        {
            var pkb = new PKBStore();

            // parents/children
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 1), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 2), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 3), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.CALL, 4), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 5), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.CALL, 6), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 7), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.IF, 8), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.WHILE, 10), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 17), 1));

            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.WHILE, 10), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 13), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 14), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.CALL, 15), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 16), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 9), 1));
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
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 9), new ExpressionModel(StatementType.WHILE, 10)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(StatementType.ASSIGN, 13)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 11), new ExpressionModel(StatementType.ASSIGN, 12)));
            pkb.FollowsList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.CALL, 17)));

            return pkb;
        }
    }
}
