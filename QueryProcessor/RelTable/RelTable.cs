using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor.RelTable
{
    internal class RelTable
    {
        private readonly List<RelTableItem> _relTableItems = new List<RelTableItem>();

        public RelTable()
        {
            Initialize();
        }

        private void Initialize()
        {
            List<RelationArgumentType> callsArg1Types = new List<RelationArgumentType> { RelationArgumentType.Procedure, RelationArgumentType.Discard, RelationArgumentType.String };
            List<RelationArgumentType> callsArg2Types = new List<RelationArgumentType> { RelationArgumentType.Procedure, RelationArgumentType.Discard, RelationArgumentType.String };

            //CALLS
            _relTableItems.Add(new RelTableItem(RelationType.CALLS, 2, callsArg1Types, callsArg2Types));

            //CALLS_STAR
            _relTableItems.Add(new RelTableItem(RelationType.CALLS_STAR, 2, callsArg2Types, callsArg2Types));

            List<RelationArgumentType> modifiesArg1Types = new List<RelationArgumentType> { RelationArgumentType.Procedure, RelationArgumentType.Prog_line, RelationArgumentType.Statement,
                    RelationArgumentType.Assign, RelationArgumentType.While, RelationArgumentType.String, RelationArgumentType.Integer, RelationArgumentType.Call, RelationArgumentType.If };
            List<RelationArgumentType> modifiesArg2Types = new List<RelationArgumentType> { RelationArgumentType.Variable, RelationArgumentType.Discard, RelationArgumentType.String };

            //MODIFIES
            _relTableItems.Add(new RelTableItem(RelationType.MODIFIES, 2, modifiesArg1Types, modifiesArg2Types));

            List<RelationArgumentType> usesArg1Types = new List<RelationArgumentType> { RelationArgumentType.Procedure, RelationArgumentType.Prog_line, RelationArgumentType.Statement,
                    RelationArgumentType.Assign, RelationArgumentType.While, RelationArgumentType.String, RelationArgumentType.Integer, RelationArgumentType.Call, RelationArgumentType.If };
            List<RelationArgumentType> usesArg2Types = new List<RelationArgumentType> { RelationArgumentType.Variable, RelationArgumentType.Discard, RelationArgumentType.String };


            //USES            
            _relTableItems.Add(new RelTableItem(RelationType.USES, 2, usesArg1Types, usesArg2Types));

            List<RelationArgumentType> parentArg1Types = new List<RelationArgumentType> { RelationArgumentType.Procedure, RelationArgumentType.Prog_line, RelationArgumentType.Statement,
                    RelationArgumentType.Assign, RelationArgumentType.While, RelationArgumentType.String, RelationArgumentType.Integer, RelationArgumentType.Call, RelationArgumentType.If };
            List<RelationArgumentType> parentArg2Types = new List<RelationArgumentType> { RelationArgumentType.Procedure, RelationArgumentType.Prog_line, RelationArgumentType.Statement,
                    RelationArgumentType.Assign, RelationArgumentType.While, RelationArgumentType.String, RelationArgumentType.Integer, RelationArgumentType.Call, RelationArgumentType.If };

            //PARENT
            _relTableItems.Add(new RelTableItem(RelationType.PARENT, 2, parentArg1Types, parentArg2Types));

            //PARENT_STAR            
            _relTableItems.Add(new RelTableItem(RelationType.PARENT_STAR, 2, parentArg1Types, parentArg2Types));

            List<RelationArgumentType> followsArg1Types = new List<RelationArgumentType> { RelationArgumentType.Procedure, RelationArgumentType.Prog_line, RelationArgumentType.Statement,
                    RelationArgumentType.Assign, RelationArgumentType.While, RelationArgumentType.String, RelationArgumentType.Integer, RelationArgumentType.Call, RelationArgumentType.If };
            List<RelationArgumentType> followsArg2Types = new List<RelationArgumentType> { RelationArgumentType.Procedure, RelationArgumentType.Prog_line, RelationArgumentType.Statement,
                    RelationArgumentType.Assign, RelationArgumentType.While, RelationArgumentType.String, RelationArgumentType.Integer, RelationArgumentType.Call, RelationArgumentType.If };

            //FOLLOWS
            _relTableItems.Add(new RelTableItem(RelationType.FOLLOWS, 2, followsArg1Types, followsArg2Types));
            
            //FOLLOWS_STAR
            _relTableItems.Add(new RelTableItem(RelationType.FOLLOWS_STAR, 2, followsArg1Types, followsArg2Types));

            /*
            
            Program design entity relationships:
                Modifies (procedure, variable)
                Modifies (stmt, variable)
                Uses (procedure, variable)
                Uses (stmt, variable)
                Calls (procedure 1, procedure 2)
                Calls* (procedure 1, procedure 2)
                Parent (stmt 1, stmt 2)
                Parent* (stmt 1, stmt 2)
                Follows (stmt 1, stmt 2)
                Follows* (stmt 1, stmt 2)
                Next (prog_line 1, prog_line 2)
                Next* (prog_line 1, prog_line 2)
                Affects (assign 1, assign2)
                Affects* (assign 1, assign2)

            W 1 ITERACJI:
                relRef : ModifiesS | UsesS | Parent | ParentT | Follows | FollowsT
                ModifiesS : ‘Modifies’ ‘(’ stmtRef ‘,’ entRef ‘)’
                UsesS : ‘Uses’ ‘(’ stmtRef ‘,’ entRef ‘)’
                Parent : ‘Parent’ ‘(’ stmtRef ‘,’ stmtRef ‘)’
                ParentT : ‘Parent*’ ‘(’ stmtRef ‘,’ stmtRef ‘)’
                Follows : ‘Follows’ ‘(’ stmtRef ‘,’ stmtRef ‘)’
                FollowsT : ‘Follows*’ ‘(’ stmtRef ‘,’ stmtRef ‘)’
            
             */

        }

        public bool ValidateRelation(RelationType relationType, int argumentsCount, params RelationArgumentType[] argumentTypes)
        {
            
            RelTableItem rel = _relTableItems.FirstOrDefault(x => x.RelationType == relationType);
            
            if( rel == null) return false;
            if (argumentsCount != rel.ArgumentsCount) return false;
            if (argumentTypes.Count() != rel.ArgumentsCount) return false;
            if (!rel.Argument1Types.Contains(argumentTypes[0])) return false;
            if (!rel.Argument2Types.Contains(argumentTypes[1])) return false;            
            else return true;

        }
        public RelationType GetRelationType(string relationName)
        {
            if (relationName.Equals("Modifies", StringComparison.OrdinalIgnoreCase)) return RelationType.MODIFIES;
            else if (relationName.Equals("Follows", StringComparison.OrdinalIgnoreCase)) return RelationType.FOLLOWS;
            else if (relationName.Equals("Follows*", StringComparison.OrdinalIgnoreCase)) return RelationType.FOLLOWS_STAR;
            else if (relationName.Equals("Parent", StringComparison.OrdinalIgnoreCase)) return RelationType.PARENT;
            else if (relationName.Equals("Parent*", StringComparison.OrdinalIgnoreCase)) return RelationType.PARENT_STAR;
            else throw new ArgumentException("Nie rozpoznano relacji.");
        }

    }
}
