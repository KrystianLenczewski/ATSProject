using QueryProcessor.Enums;
using System;
using System.Collections.Generic;
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
            _relTableItems.Add(new RelTableItem(RelationType.CALLS, 2, new List<RelationArgumentType> { RelationArgumentType.Procedure,
                RelationArgumentType.Discard,RelationArgumentType.String }, new List<RelationArgumentType> { RelationArgumentType.Procedure,
                RelationArgumentType.Discard,RelationArgumentType.String }));
        }

        public bool ValidateRelation(RelationType relationType, int argumentsCount, params RelationArgumentType[] argumentTypes)
        {
            throw new NotImplementedException();
        }
    }
}
