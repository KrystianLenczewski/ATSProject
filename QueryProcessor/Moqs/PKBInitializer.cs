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

            //Circle
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 1), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 2), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 3), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.CALL, 4), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 5), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.CALL, 6), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 7), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.IF, 8), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(StatementType.ASSIGN, 17), 1));

            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 9), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.WHILE, 10), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 13), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 14), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.CALL, 15), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(StatementType.ASSIGN, 16), 1));
          
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(StatementType.ASSIGN, 11), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(StatementType.ASSIGN, 12), 1));

            //Rectangle
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(StatementType.WHILE, 18), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(StatementType.ASSIGN, 22), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(StatementType.ASSIGN, 19), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(StatementType.CALL, 20), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(StatementType.ASSIGN, 21), 1));

            //Triangle
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(StatementType.WHILE, 23), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(StatementType.ASSIGN, 27), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(StatementType.IF,24), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(StatementType.ASSIGN,25), 1));
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(StatementType.ASSIGN,26), 1));

            //Hexagon
            pkb.ParentList.Add(new ParentModel(new ExpressionModel(WithNameType.PROCEDURE, "Hexagon"), new ExpressionModel(StatementType.ASSIGN, 28), 1));







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


            //modifies
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,1), new ExpressionModel(FactorType.VAR,"t")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,2), new ExpressionModel(FactorType.VAR,"a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,3), new ExpressionModel(FactorType.VAR,"d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,5), new ExpressionModel(FactorType.VAR,"b")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,7), new ExpressionModel(FactorType.VAR,"b")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,9), new ExpressionModel(FactorType.VAR,"k")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE,10), new ExpressionModel(FactorType.VAR,"c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,11), new ExpressionModel(FactorType.VAR,"d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,12), new ExpressionModel(FactorType.VAR,"c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,13), new ExpressionModel(FactorType.VAR,"a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,14), new ExpressionModel(FactorType.VAR,"a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN,16), new ExpressionModel(FactorType.VAR,"c")));

            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 19), new ExpressionModel(FactorType.VAR, "t")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 21), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 22), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 25), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 26), new ExpressionModel(FactorType.VAR, "a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 27), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 28), new ExpressionModel(FactorType.VAR, "t")));

            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "c")));

            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 6), new ExpressionModel(FactorType.VAR, "t")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 15), new ExpressionModel(FactorType.VAR, "t")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "t")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "t")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "d")));

            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "c")));

            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "c")));

            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "t")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "t")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "b")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "d")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "a")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "c")));
            pkb.ModifiesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Hexagon"), new ExpressionModel(FactorType.VAR, "t")));

            //uses
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 2), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 3), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 3), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 4), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 5), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 5), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 6), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 6), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 7), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 7), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 8), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 9), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 9), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 10), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 11), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 11), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 12), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 13), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 13), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 14), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 14), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 15), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 15), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 16), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 17), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 18), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 19), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 19), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 19), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.CALL, 20), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 21), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 22), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.WHILE, 23), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.IF, 24), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 25), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 26), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 26), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 26), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 26), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 26), new ExpressionModel(FactorType.VAR, "b")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 27), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 27), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 27), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 28), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(StatementType.ASSIGN, 28), new ExpressionModel(FactorType.VAR, "t")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Circle"), new ExpressionModel(FactorType.VAR, "b")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Rectangle"), new ExpressionModel(FactorType.VAR, "b")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "c")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "d")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "t")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "k")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Triangle"), new ExpressionModel(FactorType.VAR, "b")));

            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Hexagon"), new ExpressionModel(FactorType.VAR, "a")));
            pkb.UsesList.Add(KeyValuePair.Create(new ExpressionModel(WithNameType.PROCEDURE, "Hexagon"), new ExpressionModel(FactorType.VAR, "t")));

            return pkb;
        }
    }
}
